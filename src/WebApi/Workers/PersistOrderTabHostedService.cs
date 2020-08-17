using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FavoDeMel.Application.ManageService.Application;
using FavoDeMel.Application.ManageService.Application.OrderTab.CloseOrderTab;
using FavoDeMel.Application.ManageService.Application.OrderTab.OpenOrderTab;
using FavoDeMel.Application.ManageService.Core;
using FavoDeMel.Application.ManageService.Infrastructure.Repositories;
using FavoDeMel.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using STAN.Client;

namespace FavoDeMel.WebApi.Workers
{
    public class PersistOrderTabHostedService : BackgroundService
    {
        public IServiceProvider Services { get; }
        
        public PersistOrderTabHostedService(IServiceProvider services)
        {
            Services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = Services.CreateScope();
            var stanConnectionFactory = scope.ServiceProvider.GetRequiredService<StanConnectionFactory>();
            var stanConnection = stanConnectionFactory.CreateConnection("test-cluster", "PersistOrderTabHostedService");
            
            var connectionString = scope.ServiceProvider.GetRequiredService<ConnectionString>();
            
            stanConnection.Subscribe("persist_order_tab", async (sender, args) =>
            {
                var decodedOrder = Encoding.UTF8.GetString(args.Message.Data);
                var persistentAction = JsonSerializer.Deserialize<PersistentAction>(decodedOrder);
                var jsonElement = (JsonElement)persistentAction.Value;
                
                //Internally is managed per a connection pool
                await using var sqlConnection = new SqlConnection(connectionString);
                var orderTabRepository = new OrderTabRepository(sqlConnection.GetQueryFactory());
                
                switch (persistentAction.Type)
                {
                    case PersistentActionTypes.Insert:
                        var id = jsonElement.GetProperty("Id").GetInt64();
                        var tableNumber = jsonElement.GetProperty("TableNumber").GetInt32();
                        var orderTab = new OrderTab(id, tableNumber);
                        
                        await orderTabRepository.AddAsync(orderTab);
                        break;
                    case PersistentActionTypes.Delete:
                        await orderTabRepository.RemoveByIdAsync(jsonElement.GetInt64());
                        break;
                }
            });

            while (!stoppingToken.IsCancellationRequested) { await Task.Delay(1000, stoppingToken); }
        }
    }
}