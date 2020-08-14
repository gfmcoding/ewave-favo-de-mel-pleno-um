using System.Collections.Generic;

namespace FavoDeMel.Application.ManageService.Core
{
    public class Order
    {
        public long Id { get; private set; }
        public int Index { get; protected internal set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public OrderStatus Status { get; private set; }
        public long OrderTabId { get; protected internal set; }

        protected internal Order(long id, int index, string name, string description)
        {
            Id = id;
            Index = index;
            Name = name;
            Description = description;
            Status = OrderStatus.AwaitingPreparation;
        }
        
        public Order(long id, int index, string name, string description, long orderTabId, OrderStatus status)
        {
            Id = id;
            Index = index;
            Name = name;
            Description = description;
            Status = status;
            OrderTabId = orderTabId;
        }

        private bool UpdateStatus(OrderStatus expected, OrderStatus newStatus)
        {
            if (Status != expected) return false;

            Status = newStatus;
            return true;
        }

        public bool ToCancel() => UpdateStatus(OrderStatus.AwaitingPreparation, OrderStatus.Canceled);
        public bool ToInPreparation() => UpdateStatus(OrderStatus.AwaitingPreparation, OrderStatus.InPreparation);
        public bool ToDone() => UpdateStatus(OrderStatus.InPreparation, OrderStatus.Done);
        public bool ToDelivery() => UpdateStatus(OrderStatus.Done, OrderStatus.Delivered);
    }
}