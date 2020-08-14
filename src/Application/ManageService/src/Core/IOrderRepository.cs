using System.Collections.Generic;

namespace FavoDeMel.Application.ManageService.Core
{
    public interface IOrderRepository
    {
        void Add(Order order);
        SortedList<int, Order> GetAll();
        Order GetById(long id);
        Order GetByPosition(int position);
        SortedList<int, Order> GetOrderTabOrders(long orderTabId);
        int Count();
        void RemoveById(long id);
    }
}