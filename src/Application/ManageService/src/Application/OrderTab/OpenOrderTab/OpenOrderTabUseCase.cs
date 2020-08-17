using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FavoDeMel.Application.ManageService.Core;
using FavoDeMel.Infrastructure.Common.IdGenerator;
using NATS.Client;
using STAN.Client;

namespace FavoDeMel.Application.ManageService.Application.OrderTab.OpenOrderTab
{
    public class OpenOrderTabUseCase
    {
        private readonly IIdGenerator _idGenerator;
        private readonly IOrderTabRepositoryInMemory _repositoryInMemory;
        private readonly IStanConnection _stanConnection;
        private readonly IConnection _connection;

        public OpenOrderTabUseCase(IOrderTabRepositoryInMemory repositoryInMemory, IIdGenerator idGenerator, IStanConnection stanConnection, IConnection connection)
        {
            _repositoryInMemory = repositoryInMemory;
            _idGenerator = idGenerator;
            _stanConnection = stanConnection;
            _connection = connection;
        }

        public async ValueTask<long> ExecAsync(OpeningOrderTabRequest request)
        {
            if(request.TableNumber <= 0) throw new InvalidTableNumberException(request.TableNumber, $"A mesa do numero informado já tem um comanda em aberto.");
            if(_repositoryInMemory.ExistByTableNumber(request.TableNumber)) throw new InvalidTableNumberException(request.TableNumber, $"O numero da mesa é invalido: {request.TableNumber}");
            
            var id = _idGenerator.Generate();
            var orderTab = new Core.OrderTab(id, request.TableNumber);
            
            _repositoryInMemory.Add(orderTab);

            var persistentAction = new PersistentAction {Type = PersistentActionTypes.Insert, Value = orderTab};
            var persistentActionSerialized = JsonSerializer.SerializeToUtf8Bytes(persistentAction);
            await _stanConnection.PublishAsync("persist_order_tab", persistentActionSerialized);

            var notification = new NewOrderTabOpenedNotificaiton {OrderTab = orderTab};
            var notificationSerialized = JsonSerializer.SerializeToUtf8Bytes(notification);
            _connection.Publish("order_tab_notification", notificationSerialized);

            return id;
        }
    }
}