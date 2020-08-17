using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FavoDeMel.Application.ManageService.Core;
using NATS.Client;
using STAN.Client;

namespace FavoDeMel.Application.ManageService.Application.Order.ToDoneOrder
{
    public class ToDoneOrderHandler
    {
        private readonly OrdersManager _ordersManager;
        private readonly IOrderRepositoryInMemory _repositoryInMemory;
        private readonly IStanConnection _stanConnection;
        private readonly IConnection _connection;

        public ToDoneOrderHandler(OrdersManager ordersManager, IConnection connection, IStanConnection stanConnection, IOrderRepositoryInMemory repositoryInMemory)
        {
            _ordersManager = ordersManager;
            _connection = connection;
            _stanConnection = stanConnection;
            _repositoryInMemory = repositoryInMemory;
        }

        public async ValueTask ExecAsync(ToDoneOrderRequest request)
        {
            var order = _repositoryInMemory.GetById(request.Id);
            var result = order.ToDone();
            if (!result) throw new ToDoneOrderException("O pedido ainda não esta em preparo para ser finalizado.");
            
            var persistentAction = new PersistentAction {Type = PersistentActionTypes.Update, Value = order};
            var persistentActionSerialized = JsonSerializer.SerializeToUtf8Bytes(persistentAction);
            await _stanConnection.PublishAsync("persist_order", persistentActionSerialized);

            var notification = new CompletedOrderNotification {Order = order};
            var notificationSerialized = JsonSerializer.SerializeToUtf8Bytes(notification);
            _connection.Publish("order_notification", notificationSerialized);
        }
    }
}