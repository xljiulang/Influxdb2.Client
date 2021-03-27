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

            var model = new M3
            {
                Age = 30,
                CoId = "coid001",
                CreateTime = DateTimeOffset.Now,
                LabelId = "lb001",
                Name = "李4"
            };

            await infuxdb.WriteAsync(model);

            var flux = Flux
                .From("v5")
                .Range("-60h")
                .Filter(FnBody.R.MeasurementEquals("M3"))
                .Limit(10)
                ;

            var tables = await infuxdb.QueryAsync(flux);
            var models = tables.ToModels<M3>();

            Console.WriteLine("Hello World!");
        }
    }
}
