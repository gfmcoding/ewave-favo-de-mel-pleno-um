using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FavoDeMel.Application.ManageService.Core;
using NATS.Client;
using STAN.Client;

namespace FavoDeMel.Application.ManageService.Application.Order.PlaceNewOrder
{
    public class PlaceNewOrderHandler
    {
        private readonly OrdersManager _ordersManager;
        private readonly IOrderTabRepositoryInMemory _orderTabRepositoryInMemory;
        private readonly IStanConnection _stanConnection;
        private readonly IConnection _connection;

        public PlaceNewOrderHandler(OrdersManager ordersManager, IOrderTabRepositoryInMemory orderTabRepositoryInMemory, IStanConnection stanConnection, IConnection connection)
        {
            _orderTabRepositoryInMemory = orderTabRepositoryInMemory;
            _stanConnection = stanConnection;
            _connection = connection;
            _ordersManager = ordersManager;
        }

        public async ValueTask ExecAsync(PlaceNewOrderRequest request)
        {
            var newOrder = _ordersManager.NewOrder(request.Name, request.Description);

            var orderTab = _orderTabRepositoryInMemory.GetById(request.OrderTabId);
            orderTab.AddOrder(newOrder);

            var persistentAction = new PersistentAction {Type = PersistentActionTypes.Insert, Value = newOrder};
            var persistentActionSerialized = JsonSerializer.SerializeToUtf8Bytes(persistentAction);
            await _stanConnection.PublishAsync("persist_order", persistentActionSerialized);
            
            var notification = new NewOrderPlacedNotification {Order = newOrder};
            var notificationSerialized = JsonSerializer.Serialize(notification);
            var notificationEncoded = Encoding.UTF8.GetBytes(notificationSerialized);
            _connection.Publish("order_notification", notificationEncoded);
        }
    }
}