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
            var f1 = Flux
                .From("v5")
                .Range("-60h")
                .Filter(FilterFn.R.MatchMeasurement("M3"))
                .Pivot()
                .Filter(FilterFn.R.Then("r.Age > 10"));

            var f2 = Flux
                .From("v5")
                .Range("-60h")
                .Filter(FilterFn.R.MatchMeasurement("M3"))
                .Pivot()
                .Filter(FilterFn.R.WhenColumn("Age", "<=", 10));


            var f = f1.Union(f2).Top(3, Columns.Values("Age"));

            var sql = f.ToString();


            var services = new ServiceCollection();
            services.AddLogging(c => c.AddConsole());

            services.AddInfuxdbClient(db =>
            {
                db.Host = new Uri("http://v5.taichuan.net:8086");
                db.Token = "jM6KYmfy6iryQc_0Rms16hJnZjVieFYPRW4RrkeENnLiMdaRZMQ_g4mP8Xi_Cbmp6A1varU8E7E8VdC5NmRQaQ==";
            });

            var root = services.BuildServiceProvider();
            using var scope = root.CreateScope();
            var infuxdb = scope.ServiceProvider.GetRequiredService<IInfuxdbClient>();

            var model = new M3
            {
                Age = 30,
                CoId = "coid001",
                CreateTime = DateTimeOffset.Now,
                LabelId = "lb001",
                Name = "李4"
            };
            await infuxdb.WriteAsync(model, "v5", "v5");

            Console.WriteLine("Hello World!");
        }
    }
}
