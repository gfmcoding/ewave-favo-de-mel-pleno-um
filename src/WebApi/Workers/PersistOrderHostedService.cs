using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FavoDeMel.Application.ManageService.Application;
using FavoDeMel.Application.ManageService.Core;
using FavoDeMel.Application.ManageService.Infrastructure.Repositories;
using FavoDeMel.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using STAN.Client;

namespace FavoDeMel.WebApi.Workers
{
    public class PersistOrderHostedService : BackgroundService
    {
        public IServiceProvider Services { get; }

        public PersistOrderHostedService(IServiceProvider services)
        {
            Services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = Services.CreateScope();
            var stanConnectionFactory = scope.ServiceProvider.GetRequiredService<StanConnectionFactory>();
            var stanConnection = stanConnectionFactory.CreateConnection("test-cluster", "PersistOrderHostedService");
            
            var connectionString = scope.ServiceProvider.GetRequiredService<ConnectionString>();
            
            stanConnection.Subscribe("persist_order", async (sender, args) =>
            {
                var decodedOrder = Encoding.UTF8.GetString(args.Message.Data);
                var persistentAction = JsonSerializer.Deserialize<PersistentAction>(decodedOrder);
                var jsonElement = (JsonElement)persistentAction.Value;
                
                //Internally is managed per a connection pool
                await using var sqlConnection = new SqlConnection(connectionString);
                var orderRepository = new OrderRepository(sqlConnection.GetQueryFactory());

                switch (persistentAction.Type)
                {
                    case  PersistentActionTypes.Insert:
                        var id = jsonElement.GetProperty("Id").GetInt64();
                        var index = jsonElement.GetProperty("Index").GetInt32();
                        var name = jsonElement.GetProperty("Name").GetString();
                        var description = jsonElement.GetProperty("Description").GetString();
                        var orderTabId = jsonElement.GetProperty("OrderTabId").GetInt64();
                        var status = jsonElement.GetProperty("Status").GetInt32();
                        
                        var order = new Order(id, index, name, description, orderTabId, (OrderStatus)status);
                        await orderRepository.AddAsync(order);
                        break;
                    case PersistentActionTypes.Update:
                        var updateId = jsonElement.GetProperty("Id").GetInt64();
                        var updateIndex = jsonElement.GetProperty("Index").GetInt32();
                        var updateName = jsonElement.GetProperty("Name").GetString();
                        var updatedDescription = jsonElement.GetProperty("Description").GetString();
                        var updatedOrderTabId = jsonElement.GetProperty("OrderTabId").GetInt64();
                        var updateStatus = jsonElement.GetProperty("Status").GetInt32();
                        
                        var updateOrder = new Order(updateId, updateIndex, updateName, updatedDescription, updatedOrderTabId, (OrderStatus)updateStatus);
                        await orderRepository.UpdateAsync(updateOrder);
                        break;
                    case PersistentActionTypes.Delete:
                        await orderRepository.RemoveByIdAsync(jsonElement.GetInt64());
                        break;
                }
            });
            
            while (!stoppingToken.IsCancellationRequested) { await Task.Delay(1000, stoppingToken); }
        }
    }
}