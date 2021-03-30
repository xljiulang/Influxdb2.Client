using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Influxdb2
{
    class BookHostedService : BackgroundService
    {
        private readonly Random random = new Random();
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<BookHostedService> logger;

        public BookHostedService(IServiceScopeFactory scopeFactory, ILogger<BookHostedService> logger)
        {
            this.scopeFactory = scopeFactory;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<BookService>();

            var count = 1000;
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var tasks = Enumerable.Range(0, count).Select(i => ReadWriteAsync());
            await Task.WhenAll(tasks);

            stopwatch.Stop();
            this.logger.LogInformation($"并发读写各{count}次总耗时：{stopwatch.Elapsed}");
        }


        private async Task ReadWriteAsync()
        {
            using var scope = scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<BookService>();
            var book = new Book
            {
                Serie = "科幻",
                Name = $"{random.Next(1, 100)}体",
                Price = random.NextDouble() * 100d,
                SpecialOffer = random.NextDouble() < 0.5d,
                CreateTime = DateTimeOffset.Now
            };

            await service.AddAsync(book);
            var books = await service.GetBooksAsync();
        }
    }
}
