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
            var m = new M2
            {
                Age = 10,
                CoId = "coid001",
                CreateTime = DateTimeOffset.Now,
                LabelId = "lb001",
                Name = "name"
            };

            var services = new ServiceCollection();
            services.AddInfuxdbClient(db =>
            {
                db.Host = new Uri("http://v5.taichuan.net:8086");
                db.Token = "jM6KYmfy6iryQc_0Rms16hJnZjVieFYPRW4RrkeENnLiMdaRZMQ_g4mP8Xi_Cbmp6A1varU8E7E8VdC5NmRQaQ==";
            });

            services.AddLogging(c => c.AddConsole());

            var sp = services.BuildServiceProvider();

            var client = sp.GetRequiredService<IInfuxdbClient>();
            await client.WriteAsync(m, "v5", "v5");

            Console.WriteLine("Hello World!");
        }
    }

    class M2
    {
        [InfluxdbDataType(DataType.Tag)]
        public string CoId { get; set; }



        [InfluxdbDataType(DataType.Tag)]
        public string LabelId { get; set; }



        [InfluxdbDataType(DataType.Field)]
        public string Name { get; set; }



        [InfluxdbDataType(DataType.Field)]
        public int? Age { get; set; }


        [InfluxdbDataType(DataType.Time)]
        public DateTimeOffset CreateTime { get; set; }
    }
}
