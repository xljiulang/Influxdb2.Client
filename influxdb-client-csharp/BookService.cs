using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using Influxdb2.Client;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace influxdb_client_csharp
{
    class BookService
    {
        private readonly InfluxDBClient infuxdb;
        private readonly InfuxdbOptions options;

        public BookService(InfluxDBClient infuxdb, IOptionsMonitor<InfuxdbOptions> options)
        {
            this.infuxdb = infuxdb;
            this.options = options.CurrentValue;
        }

        public Task AddAsync(Book book)
        {
            var api = this.infuxdb.GetWriteApiAsync();
            return api.WriteMeasurementAsync(options.DefaultBucket, options.DefaultOrg, WritePrecision.Ns, book);
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            // 这里借用Influxdb2.Client的Flux对象辅助生成查询字符串
            var flux = Flux
                .From(options.DefaultBucket)
                .Range("-3d")
                .Filter(FnBody.R.MeasurementEquals($"{nameof(Book)}"))
                .Pivot()
                .Sort(Columns.Time, desc: true)
                .Limit(10)
                ;

            var api = this.infuxdb.GetQueryApi();
            return await api.QueryAsync<Book>(flux.ToString(), options.DefaultOrg);
        }
    }
}
