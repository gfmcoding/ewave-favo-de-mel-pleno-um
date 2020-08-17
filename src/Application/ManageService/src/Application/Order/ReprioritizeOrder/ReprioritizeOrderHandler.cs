using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FavoDeMel.Application.ManageService.Core;
using NATS.Client;
using STAN.Client;

namespace FavoDeMel.Application.ManageService.Application.Order.ReprioritizeOrder
{
    public class ReprioritizeOrderHandler
    {
        private readonly OrdersManager _ordersManager;
        private readonly IOrderRepositoryInMemory _orderRepositoryInMemory;
        private readonly IStanConnection _stanConnection;
        private readonly IConnection _connection;

        public ReprioritizeOrderHandler(OrdersManager ordersManager, IStanConnection stanConnection, IConnection connection, IOrderRepositoryInMemory orderRepositoryInMemory)
        {
            _ordersManager = ordersManager;
            _stanConnection = stanConnection;
            _connection = connection;
            _orderRepositoryInMemory = orderRepositoryInMemory;
        }

        public async ValueTask ExecAsync(ReprioritizeOrderRequest request)
        {
            var result = _ordersManager.ReprioritizeOrder(request.Position, request.NewPosition);
            if (!result) throw new ReprioritizeOrderException("A repriorização dos pedidos falhou.");
            
            var positionOrder = _orderRepositoryInMemory.GetByPosition(request.Position);

            var persistentActionPosition = new PersistentAction
                {Type = PersistentActionTypes.Update, Value = positionOrder};
            var persistentActionSerializedPosition = JsonSerializer.SerializeToUtf8Bytes(persistentActionPosition);
            await _stanConnection.PublishAsync("persist_order", persistentActionSerializedPosition);
            
            var newPositionOrder = _orderRepositoryInMemory.GetByPosition(request.NewPosition);
            
            var persistentActionNewPosition = new PersistentAction {Type = PersistentActionTypes.Update, Value = newPositionOrder};
            var persistentActionSerializedNewPosition = JsonSerializer.SerializeToUtf8Bytes(persistentActionNewPosition);
            await _stanConnection.PublishAsync("persist_order", persistentActionSerializedNewPosition);

            var notification = new OrderReprioritizedNotification
                {Position = request.Position, NewPosition = request.NewPosition};
            var notificationSerialized = JsonSerializer.SerializeToUtf8Bytes(notification);
            _connection.Publish("order_notification", notificationSerialized);
        }
    }
}