using System.Collections.Generic;
using FavoDeMel.Infrastructure.Common.IdGenerator;

namespace FavoDeMel.Application.ManageService.Core
{
    public class OrdersManager
    { 
        public int Count => OrdersRepositoryInMemory.Count();
        //private SortedList<int, Order> Orders { get; set; }
        private IOrderRepositoryInMemory OrdersRepositoryInMemory { get; set; }
        
        private readonly IIdGenerator _idGenerator;
        
        public OrdersManager(IIdGenerator idGenerator, IOrderRepositoryInMemory ordersRepositoryInMemory)
        {
            OrdersRepositoryInMemory = ordersRepositoryInMemory;
            _idGenerator = idGenerator;
        }

        public Order NewOrder(string name, string description)
        {
            var nextId = _idGenerator.Generate();
            var nextIndex = (OrdersRepositoryInMemory.Count() + 1);
            var order = new Order(nextId,  nextIndex, name, description, 1, OrderStatus.AwaitingPreparation);
            
            OrdersRepositoryInMemory.Add(order);
            return order;
        }

        public IReadOnlyDictionary<int, Order> GetOrders() => OrdersRepositoryInMemory.GetAll();

        public bool ReprioritizeOrder(int position, int newPosition)
        {
            var orders = OrdersRepositoryInMemory.GetAll();
            
            var containsPosition = orders.TryGetValue(position, out var orderPosition);
            var containsNewPosition = orders.TryGetValue(newPosition, out var orderNewPosition);
            if (!(containsPosition && containsNewPosition)) return false;
            
            
            orders.Remove(newPosition);
            orderPosition.Index = newPosition;
            orders.Add(newPosition, orderPosition);
            
            orders.Remove(position);
            orderNewPosition.Index = position;
            orders.Add(position, orderNewPosition);

            return true;
        }

        //TODO: Surely there can be a better solution...
        public void Remove(int position)
        {
            const int numberOne = 1;
            var orders = OrdersRepositoryInMemory.GetAll();
            
            orders.Remove(position);
            for (var i = position; (i - numberOne) < orders.Count; i++)
            {
                var order = orders[i + numberOne];
                orders.Remove(i + numberOne);
                order.Index = (order.Index - numberOne);
                orders.Add(order.Index, order);
            }
        }
    }
}