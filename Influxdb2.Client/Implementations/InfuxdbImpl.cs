using Influxdb2.Client.Datas;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Influxdb2.Client.Implementations
{
    /// <summary>
    /// IInfuxdb实现
    /// </summary>
    sealed class InfuxdbImpl : IInfuxdb
    {
        private readonly IInfuxdbApi infuxdbApi;
        private readonly InfuxdbOptions options;

        /// <summary>
        /// IInfuxdb实现
        /// </summary>
        /// <param name="infuxdb"></param>
        /// <param name="options"></param>
        public InfuxdbImpl(IInfuxdbApi infuxdb, IOptionsMonitor<InfuxdbOptions> options)
        {
            this.infuxdbApi = infuxdb;
            this.options = options.CurrentValue;
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public Task<IDataTableCollection> QueryAsync(IFlux flux, string? org = null)
        {
            return this.QueryAsync(flux.ToString(), org);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="flux"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public async Task<IDataTableCollection> QueryAsync(string flux, string? org = null)
        {
            var orgValue = (org ?? this.options.DefaultOrg) ?? throw new ArgumentNullException(nameof(org));
            using var content = new FluxContent(flux);
            var stream = await this.infuxdbApi.QueryAsync(content, orgValue);
            var csvReader = new CsvReader(stream, Encoding.UTF8);
            return await DataTableCollection.ParseAsync(csvReader);
        }

        /// <summary>
        /// 写入实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="bucket"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public async Task<int> WriteAsync<TEntity>(TEntity entity, string? bucket = null, string? org = null) where TEntity : notnull
        {
            var point = new Point(entity);
            return await this.WritePointAsync(point);
        }

        /// <summary>
        /// 写入实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="bucket"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public Task<int> WriteAsync<TEntity>(IEnumerable<TEntity> entities, string? bucket = null, string? org = null) where TEntity : notnull
        {
            var points = entities.Select(item => new Point(item));
            return this.WritePointAsync(points, bucket, org);
        }

        /// <summary>
        /// 写入数据点
        /// </summary>
        /// <param name="point"></param>
        /// <param name="bucket"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public Task<int> WritePointAsync(IPoint point, string? bucket = null, string? org = null)
        {
            var points = new[] { point };
            return this.WritePointAsync(points, bucket, org);
        }

        /// <summary>
        /// 写入数据点
        /// </summary>
        /// <param name="points"></param>
        /// <param name="bucket"></param>
        /// <param name="org"></param>
        /// <returns></returns>
        public async Task<int> WritePointAsync(IEnumerable<IPoint> points, string? bucket = null, string? org = null)
        {
            var count = points.Count();
            if (count == 0)
            {
                return 0;
            }

            var index = 0;
            using var content = new LineProtocolContent();
            foreach (var point in points)
            {
                point.WriteTo(content);
                if (++index < count)
                {
                    content.WriteLine();
                }
            }

            var orgValue = (org ?? this.options.DefaultOrg) ?? throw new ArgumentNullException(nameof(org));
            var bucketValue = (bucket ?? this.options.DefaultBucket) ?? throw new ArgumentNullException(nameof(bucket));

            await this.infuxdbApi.WriteAsync(content, bucketValue, orgValue);
            return count;
        }
    }
}
