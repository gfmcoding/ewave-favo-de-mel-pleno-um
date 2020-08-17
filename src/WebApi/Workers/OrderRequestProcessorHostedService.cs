using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FavoDeMel.Application.ManageService.Application.Order;
using FavoDeMel.Application.ManageService.Application.Order.CancelOrder;
using FavoDeMel.Application.ManageService.Application.Order.PlaceNewOrder;
using FavoDeMel.Application.ManageService.Application.Order.ReprioritizeOrder;
using FavoDeMel.Application.ManageService.Application.Order.ToDoneOrder;
using FavoDeMel.Application.ManageService.Application.Order.ToOrderInDelivery;
using FavoDeMel.Application.ManageService.Application.Order.ToOrderInPreparation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using STAN.Client;

namespace FavoDeMel.WebApi.Workers
{
    public class OrderRequestProcessorHostedService : BackgroundService
    {
        public IServiceProvider Services { get; }

        public OrderRequestProcessorHostedService(IServiceProvider services)
        {
            Services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = Services.CreateScope();
            var stanConnectionFactory = scope.ServiceProvider.GetRequiredService<StanConnectionFactory>();
            var stanConnection = stanConnectionFactory.CreateConnection("test-cluster", "OrderRequestProcessorHostedService");
            var cancelOrderHandler = scope.ServiceProvider.GetRequiredService<CancelOrderHandler>();
            var placeNewOrderHandler = scope.ServiceProvider.GetRequiredService<PlaceNewOrderHandler>();
            var reprioritizeOrderHandler = scope.ServiceProvider.GetRequiredService<ReprioritizeOrderHandler>();
            var toDoneOrderHandler = scope.ServiceProvider.GetRequiredService<ToDoneOrderHandler>();
            var toOrderInDeliveryHandler = scope.ServiceProvider.GetRequiredService<ToOrderInDeliveryHandler>();
            var toOrderInPreparationHandler = scope.ServiceProvider.GetRequiredService<ToOrderInPreparationHandler>();
            
            stanConnection.Subscribe("order_handler", async (sender, args) =>
            {
                var decodedOrder = Encoding.UTF8.GetString(args.Message.Data);
                var request = JsonSerializer.Deserialize<OrderRequest>(decodedOrder);
                var jsonElement = (JsonElement)request.Request;

                switch (request.Type)
                {
                    case OrderRequestTypes.CancelOrder:
                        var cancelRequest = JsonSerializer.Deserialize<CancelOrderRequest>(jsonElement.GetRawText());
                        await cancelOrderHandler.ExecAsync(cancelRequest);
                        break;
                    case OrderRequestTypes.PlaceNewOrder:
                        var placeNewOrderRequest = JsonSerializer.Deserialize<PlaceNewOrderRequest>(jsonElement.GetRawText());
                        await placeNewOrderHandler.ExecAsync(placeNewOrderRequest);
                        break;
                    case OrderRequestTypes.ReprioritizeOrder:
                        var reprioritizeOrderRequest = JsonSerializer.Deserialize<ReprioritizeOrderRequest>(jsonElement.GetRawText());
                        await reprioritizeOrderHandler.ExecAsync(reprioritizeOrderRequest);
                        break;
                    case OrderRequestTypes.ToOrderInDone:
                        var toDoneOrderRequest = JsonSerializer.Deserialize<ToDoneOrderRequest>(jsonElement.GetRawText());
                        await toDoneOrderHandler.ExecAsync(toDoneOrderRequest);
                        break;
                    case OrderRequestTypes.ToOrderInDelivery:
                        var toOrderInDeliveryRequest = JsonSerializer.Deserialize<ToOrderInDeliveryRequest>(jsonElement.GetRawText());
                        await toOrderInDeliveryHandler.ExecAsync(toOrderInDeliveryRequest);
                        break;
                    case OrderRequestTypes.ToOrderInPreparation:
                        var toOrderInPreparationRequest = JsonSerializer.Deserialize<ToOrderInPreparationRequest>(jsonElement.GetRawText());
                        await toOrderInPreparationHandler.ExecAsync(toOrderInPreparationRequest);
                        break;
                }
            });
            
            while (!stoppingToken.IsCancellationRequested) { await Task.Delay(1000, stoppingToken); }
        }
    }
}