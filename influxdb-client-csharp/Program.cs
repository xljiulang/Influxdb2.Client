using InfluxDB.Client;
using influxdb2_client_csharp.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace influxdb_client_csharp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        /// <summary>
        /// 创建host
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
                .CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddTransient<BookService>()
                        .AddHostedService<BookHostedService>()
                        .Configure<InfuxdbOptions>(context.Configuration.GetSection("Influxdb"))
                        .AddTransient(sp =>
                        {
                            var opt = sp.GetRequiredService<IOptionsMonitor<InfuxdbOptions>>().CurrentValue;
                            return InfluxDBClientFactory.Create(opt.Host.ToString(), opt.Token);
                        });
                });
        }
    }
}
