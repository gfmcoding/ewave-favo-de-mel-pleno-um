using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FavoDeMel.Application.ManageService.Core;
using NATS.Client;
using STAN.Client;

namespace FavoDeMel.Application.ManageService.Application.OrderTab.CloseOrderTab
{
    public class CloseOrderTabUseCase
    {
        private readonly IOrderTabRepositoryInMemory _repositoryInMemory;
        private readonly IStanConnection _stanConnection;
        private readonly IConnection _connection;

        public CloseOrderTabUseCase(IOrderTabRepositoryInMemory repositoryInMemory, IStanConnection stanConnection, IConnection connection)
        {
            _repositoryInMemory = repositoryInMemory;
            _stanConnection = stanConnection;
            _connection = connection;
        }

        public async ValueTask ExecAsync(CloseOrderTabRequest request)
        {
            var orderTab = _repositoryInMemory.GetById(request.Id);
            if(orderTab == null) throw new CloseOrderTabException($"Não existe uma comanda com o id informado: {request.Id}");

            var result = orderTab.TryClose();
            if (!result) throw new OrderTabDoesNotClosableException(request.Id, $"A comanda informada {request.Id} não pode ser fechada, contem pedidos em aberto.");
            
            _repositoryInMemory.RemoveById(request.Id);

            var persistentAction = new PersistentAction {Type = PersistentActionTypes.Delete, Value = request.Id};
            var persistentActionSerialized = JsonSerializer.SerializeToUtf8Bytes(persistentAction);
            await _stanConnection.PublishAsync("persist_order_tab", persistentActionSerialized);

            var notification = new OrderTabClosedNotification {OrderTab = orderTab};
            var notificationSerialized = JsonSerializer.Serialize(notification);
            var notificationEncoded = Encoding.UTF8.GetBytes(notificationSerialized);
            _connection.Publish("order_tab_notification", notificationEncoded);
        }
    }
}