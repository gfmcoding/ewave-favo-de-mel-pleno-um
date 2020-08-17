using System.Collections.Generic;
using System.Linq;

namespace FavoDeMel.Application.ManageService.Core
{
    public class OrderTab
    {
        public long Id { get; private set; }
        public int TableNumber { get; private set; }
        public bool IsClosed { get; private set; }
        public List<Order> Orders { get; private set; }

        public OrderTab(long id, int tableNumber)
        {
            Id = id;
            TableNumber = tableNumber;
            Orders = new List<Order>();
        }
        
        public OrderTab(long id, int tableNumber, List<Order> orders)
        {
            Id = id;
            TableNumber = tableNumber;
            Orders = orders ?? new List<Order>();
        }

        public void AddOrder(Order order)
        {
            order.OrderTabId = Id;
            Orders.Add(order);
        } 

        public Order? CancelOrder(long orderId)
        {
            var order = Orders.Find(o => o.Id == orderId);
            if (order == null) return null;
            if (!order.ToCancel()) return null;

            Orders.Remove(order);
            return order;
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