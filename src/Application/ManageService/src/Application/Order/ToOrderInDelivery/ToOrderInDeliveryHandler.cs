using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FavoDeMel.Application.ManageService.Core;
using NATS.Client;
using STAN.Client;

namespace FavoDeMel.Application.ManageService.Application.Order.ToOrderInDelivery
{
    public class ToOrderInDeliveryHandler
    {
        private readonly IOrderRepositoryInMemory _repositoryInMemory;
        private readonly IStanConnection _stanConnection;
        private readonly IConnection _connection;

        public ToOrderInDeliveryHandler(IOrderRepositoryInMemory repositoryInMemory, IStanConnection stanConnection, IConnection connection)
            {
            _repositoryInMemory = repositoryInMemory;
            _stanConnection = stanConnection;
            _connection = connection;
        }

        public async ValueTask ExecAsync(ToOrderInDeliveryRequest request)
        {
            var order = _repositoryInMemory.GetById(request.Id);
            var result = order.ToDelivery();
            if (!result) throw new ToOrderInDeliveryException("A entrega do pedido falhou, ele ainda não esta pronto.");
            
            var persistentAction = new PersistentAction {Type = PersistentActionTypes.Update, Value = order};
            var persistentActionSerialized = JsonSerializer.SerializeToUtf8Bytes(persistentAction);
            await _stanConnection.PublishAsync("persist_order", persistentActionSerialized);

            var notification = new OrderDeliveredNotification {Order = order};
            var notificationSerialized = JsonSerializer.SerializeToUtf8Bytes(notification);
            _connection.Publish("order_notification", notificationSerialized);
        }
    }
}