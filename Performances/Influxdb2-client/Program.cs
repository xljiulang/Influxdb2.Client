using Influxdb2.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace influxdb2_client
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
                        .AddInfuxdb();
                });
        }
    }
}
