using System.Collections.Generic;
using System.Linq;
using FavoDeMel.Application.ManageService.Core;

namespace FavoDeMel.Application.ManageService.Infrastructure.Repositories
{
    public class OrderRepositoryInMemory : IOrderRepository
    {
        //TODO: It may be necessary to be ReadOnly, where the updates are new instances.
        private SortedList<int, Order> _orders;

        public OrderRepositoryInMemory() => _orders = new SortedList<int, Order>();

        public void Add(Order order) => _orders.Add(order.Index, order);

        public SortedList<int, Order> GetAll() => _orders;
        
        public Order GetById(long id)
        {
            var keyValuePairs = _orders.Single(o => o.Value.Id == id);
            return keyValuePairs.Value;
        }

        public Order GetByPosition(int position)
        {
            var keyValuePairs = _orders.Single(o => o.Key == position);
            return keyValuePairs.Value;
        }

        public SortedList<int, Order> GetOrderTabOrders(long orderTabId)
        {
            var enumerable = _orders.Where(o => o.Value.OrderTabId == orderTabId);
            var dictionary = enumerable.ToDictionary(k => k.Key, v => v.Value);
            return new SortedList<int, Order>(dictionary);
        }

        public int Count() => _orders.Count;

        public void RemoveById(long id)
        {
            var order = _orders.SingleOrDefault(o => o.Value.OrderTabId == id);
            _orders.Remove(order.Key);
        }
    }
}