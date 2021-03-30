using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Influxdb2
{
    class BookHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory scopeFactory;

        public BookHostedService(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<BookService>();


            var book = new Book
            {
                Serie = "科幻",
                Name = "三体",
                Price = 188.88M,
                SpecialOffer = false,
                CreateTime = DateTimeOffset.Now
            };

            await service.AddAsync(book);
            var books = await service.GetBooksAsync();
        }
    }
}
