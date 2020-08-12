using System.Collections.Generic;
using System.Linq;
using FavoDeMel.Infrastructure.Common.IdGenerator;

namespace FavoDeMel.Application.ManageService.Core
{
    public class OrderTab
    {
        public long Id { get; private set; }
        public int TableNumber { get; private set; }
        public bool IsClosed { get; private set; }
        public List<Order> Orders { get; private set; }

        private readonly IIdGenerator _idGenerator;
        
        public OrderTab(long id, int tableNumber, IIdGenerator idGenerator)
        {
            Id = id;
            TableNumber = tableNumber;
            Orders = new List<Order>();
            _idGenerator = idGenerator;
        }
        
        public OrderTab(long id, int tableNumber, List<Order> orders, IIdGenerator idGenerator)
        {
            Id = id;
            TableNumber = tableNumber;
            Orders = orders ?? new List<Order>();
            _idGenerator = idGenerator;
        }

        public void AddOrder(int index, string name, string description)
        {
            var orderId = _idGenerator.Generate();
            var order = new Order(orderId, index, name, description);
            Orders.Add(order);
        }

        public bool CancelOrder(long orderId)
        {
            var order = Orders.Find(o => o.Id == orderId);
            if (!order.ToCancel()) return false;

            Orders.Remove(order);
            return true;
        }

        public bool TryClose()
        {
            var isClosable =  Orders.Count == 0 || Orders.All(order =>
                       order.Status == OrderStatus.Delivered || order.Status == OrderStatus.Canceled);
            if (!isClosable) return false;

            IsClosed = true;
            return true;
        }
    }
}