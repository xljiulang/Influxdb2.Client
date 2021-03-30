using Influxdb2.Client;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace influxdb2_client
{
    class BookService
    {
        private readonly IInfuxdb infuxdb;
        private readonly string defaultBucket;

        public BookService(IInfuxdb infuxdb, IOptionsMonitor<InfuxdbOptions> options)
        {
            this.infuxdb = infuxdb;
            this.defaultBucket = options.CurrentValue.DefaultBucket;
        }

        public Task AddAsync(Book book)
        {
            return this.infuxdb.WriteAsync(book);
        }

        public async Task<Book[]> GetBooksAsync()
        {
            var flux = Flux
                .From(defaultBucket)
                .Range("-3d")
                .Filter(FnBody.R.MeasurementEquals($"{nameof(Book)}"))
                .Pivot()
                .Sort(Columns.Time, desc: true)
                .Limit(10)
                ;

            var tables = await infuxdb.QueryAsync(flux);
            return tables.Single().ToModels<Book>();
        }
    }
}
