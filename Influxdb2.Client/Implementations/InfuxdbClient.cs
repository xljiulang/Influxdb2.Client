using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Influxdb2.Client.Implementations
{
    /// <summary>
    ///  Infuxdb客户端
    /// </summary>
    sealed class InfuxdbClient
    {
        private readonly HttpClient httpClient;
        private readonly InfuxdbOptions options;
        private static readonly JsonSerializerOptions jsonOptions = new() { PropertyNameCaseInsensitive = true };

        /// <summary>
        /// Infuxdb客户端
        /// </summary>
        /// <param name="httpClientFactory"></param>
        /// <param name="options"></param>
        public InfuxdbClient(IHttpClientFactory httpClientFactory, IOptionsMonitor<InfuxdbOptions> options)
        {
            this.httpClient = httpClientFactory.CreateClient(nameof(InfuxdbClient));
            this.options = options.CurrentValue;
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="content">flux表达式</param>
        /// <param name="org">组织</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InfluxdbException"></exception>
        /// <returns></returns> 
        public async Task<IDataTableCollection> QueryAsync(FluxContent content, string? org)
        {
            var orgValue = this.GetOrgValue(org);
            var uri = $"/api/v2/query?org={orgValue}";
            var response = await this.httpClient.PostAsync(uri, content);

            // infulxdb只支持utf8，可以不用检测编码
            var stream = await response.Content.ReadAsStreamAsync();
            if (response.IsSuccessStatusCode == false)
            {
                var error = await JsonSerializer.DeserializeAsync<InfuxdbError>(stream, jsonOptions);
                throw new InfluxdbException(error);
            }
            else
            {
                var csvReader = new CsvReader(stream, Encoding.UTF8);
                return await DataTableCollection.ParseAsync(csvReader);
            }
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="content">lineProtocal内容</param>
        /// <param name="bucket">空间名</param>
        /// <param name="org">组织</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InfluxdbException"></exception>
        /// <returns></returns> 
        public async Task WriteAsync(LineProtocolContent content, string? bucket, string? org)
        {
            var orgValue = this.GetOrgValue(org);
            var bucketValue = this.GetBucketValue(bucket);

            var uri = $"/api/v2/write?org={orgValue}&bucket={bucketValue}";
            var response = await this.httpClient.PostAsync(uri, content);

            if (response.IsSuccessStatusCode == false)
            {
                // infulxdb只支持utf8，可以不用检测编码
                var stream = await response.Content.ReadAsStreamAsync();
                var error = await JsonSerializer.DeserializeAsync<InfuxdbError>(stream, jsonOptions);
                throw new InfluxdbException(error);
            }
        }

        /// <summary>
        /// 获取org
        /// </summary>
        /// <param name="org"></param>
        /// <returns></returns>
        private string GetOrgValue(string? org)
        {
            var value = (org ?? this.options.DefaultOrg) ?? throw new ArgumentNullException(nameof(org));
            return Uri.EscapeDataString(value);
        }

        /// <summary>
        /// 获取bucket
        /// </summary>
        /// <param name="bucket"></param>
        /// <returns></returns>
        private string GetBucketValue(string? bucket)
        {
            var value = (bucket ?? this.options.DefaultBucket) ?? throw new ArgumentNullException(nameof(bucket));
            return Uri.EscapeDataString(value);
        }
    }
}
