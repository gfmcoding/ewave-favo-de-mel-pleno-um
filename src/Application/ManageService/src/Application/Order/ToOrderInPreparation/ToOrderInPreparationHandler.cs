using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FavoDeMel.Application.ManageService.Core;
using NATS.Client;
using STAN.Client;

namespace FavoDeMel.Application.ManageService.Application.Order.ToOrderInPreparation
{
    public class ToOrderInPreparationHandler
    {
        private readonly IOrderRepositoryInMemory _repositoryInMemory;
        private readonly IStanConnection _stanConnection;
        private readonly IConnection _connection;

        public ToOrderInPreparationHandler(IOrderRepositoryInMemory repositoryInMemory, IStanConnection stanConnection, IConnection connection)
        {
            _repositoryInMemory = repositoryInMemory;
            _stanConnection = stanConnection;
            _connection = connection;
        }

        public async ValueTask ExecAsync(ToOrderInPreparationRequest request)
        {
            var order = _repositoryInMemory.GetById(request.Id);
            var result = order.ToInPreparation();
            if (!result) throw new ToOrderInPreparationException("Colocar o pedido em preparação falhou, ele já esta em andamento ou finalizado.");
            
            var persistentAction = new PersistentAction {Type = PersistentActionTypes.Update, Value = order};
            var persistentActionSerialized = JsonSerializer.SerializeToUtf8Bytes(persistentAction);
            await _stanConnection.PublishAsync("persist_order", persistentActionSerialized);

            var notification = new OrderInPreparationNotification {Order = order};
            var notificationSerialized = JsonSerializer.SerializeToUtf8Bytes(notification);
            _connection.Publish("order_notification", notificationSerialized);
        }
    }
}