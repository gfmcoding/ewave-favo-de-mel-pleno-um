using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FavoDeMel.Application.ManageService.Core;
using NATS.Client;
using STAN.Client;

namespace FavoDeMel.Application.ManageService.Application.Order.CancelOrder
{
    public class CancelOrderHandler
    {
        private readonly OrdersManager _ordersManager;
        private readonly IOrderTabRepositoryInMemory _orderTabRepositoryInMemory;
        private readonly IStanConnection _stanConnection;
        private readonly IConnection _connection;

        public CancelOrderHandler(OrdersManager ordersManager, IStanConnection stanConnection, IConnection connection, IOrderTabRepositoryInMemory orderTabRepositoryInMemory)
        {
            _ordersManager = ordersManager;
            _stanConnection = stanConnection;
            _connection = connection;
            _orderTabRepositoryInMemory = orderTabRepositoryInMemory;
        }

        public async ValueTask ExecAsync(CancelOrderRequest request)
        { 
            var orderTab = _orderTabRepositoryInMemory.GetById(request.OrderTabId);
            if(orderTab == null) throw new NullReferenceException($"Don't exist an order tab to id informed: {request.OrderTabId}");

            var removedOrder = orderTab.CancelOrder(request.OrderId);
            if (removedOrder == null) throw new OrderCannotCanceledException(request.OrderId, $"The order {request.OrderId} cannot be canceled");
            
            _ordersManager.Remove(removedOrder.Index);

            var persistentAction = new PersistentAction {Type = PersistentActionTypes.Delete, Value = request.OrderId};
            var persistentActionSerialized = JsonSerializer.SerializeToUtf8Bytes(persistentAction);
            await _stanConnection.PublishAsync("persist_order", persistentActionSerialized);

            var notification = new OrderCanceledNotification {Id = request.OrderId};
            var notificationSerialized = JsonSerializer.SerializeToUtf8Bytes(notification);
            _connection.Publish("order_notification", notificationSerialized);
        }
    }
}