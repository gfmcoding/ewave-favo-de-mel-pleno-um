using System.Collections.Generic;
using System.Linq;
using FavoDeMel.Application.ManageService.Core;

namespace FavoDeMel.Application.ManageService.Infrastructure.Repositories
{
    public class OrderTabRepositoryInMemory : IOrderTabRepositoryInMemory
    {
        private HashSet<OrderTab> _orderTabs;

        public OrderTabRepositoryInMemory() => _orderTabs = new HashSet<OrderTab>();
        public OrderTabRepositoryInMemory(HashSet<OrderTab> orderTabs) => _orderTabs = orderTabs;

        public void Add(OrderTab orderTab) => _orderTabs.Add(orderTab);

        public OrderTab GetById(long id) => _orderTabs.SingleOrDefault(o => o.Id == id);

        public HashSet<OrderTab> GetAll() => _orderTabs;

        public void RemoveById(long id) => _orderTabs.RemoveWhere(o => o.Id == id);
        public bool ExistByTableNumber(int tableNumber) => _orderTabs.Any(o => o.TableNumber == tableNumber);
    }
}