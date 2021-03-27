using Influxdb2.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Influxdb2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddLogging(c => c.AddConsole());

            services.AddInfuxdb(db =>
            {
                db.DefaultOrg = "v5";
                db.DefaultBucket = "v5";
                db.Host = new Uri("http://v5.taichuan.net:8086");
                db.Token = "jM6KYmfy6iryQc_0Rms16hJnZjVieFYPRW4RrkeENnLiMdaRZMQ_g4mP8Xi_Cbmp6A1varU8E7E8VdC5NmRQaQ==";
            });

            var root = services.BuildServiceProvider();
            using var scope = root.CreateScope();
            var infuxdb = scope.ServiceProvider.GetRequiredService<IInfuxdb>();


            // 写ColumnType标记的实体
            var enitty = new Book
            {
                Serie = "科幻",
                Name = "三体",
                Price = 188.88M,
                SpecialOffer = false,
                CreateTime = DateTimeOffset.Now
            };

            await infuxdb.WriteAsync(enitty);

            // 写动态定义的数据点
            var pointData = DynamicPointData
                .Create($"{nameof(Temperature)}")
                .SetTag($"{nameof(Temperature.Location)}", "west")
                .SetField($"{nameof(Temperature.Value)}", 26D)
                .SetTimestamp(DateTimeOffset.Now);

            await infuxdb.WriteAsync(pointData);


            // 使用Flux对象查询
            var booFlux = Flux
                .From("v5")
                .Range("-24h")
                .Filter(FnBody.R.MeasurementEquals($"{nameof(Book)}"))
                .Limit(10)
                ;

            var bookTables = await infuxdb.QueryAsync(booFlux);
            var books = bookTables.ToModels<Book>();


            // 使用Flux对象查询
            var tempFlux = Flux
                .From("v5")
                .Range(DateTimeOffset.Now.AddDays(-1d))
                .Filter(FnBody.R.MeasurementEquals($"{nameof(Temperature)}"))
                .Limit(10)
                ;

            var tempTables = await infuxdb.QueryAsync(booFlux);
            var temperatures = bookTables.ToModels<Temperature>();

            Console.WriteLine("Hello World!");
        }
    }
}
