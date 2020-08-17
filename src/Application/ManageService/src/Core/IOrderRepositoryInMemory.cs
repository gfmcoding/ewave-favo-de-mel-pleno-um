using System.Collections.Generic;

namespace FavoDeMel.Application.ManageService.Core
{
    public interface IOrderRepositoryInMemory
    {
        void Add(Order order);
        SortedList<int, Order> GetAll();
        Order GetById(long id);
        Order GetByPosition(int position);
        SortedList<int, Order> GetOrderByOrderTabId(long orderTabId);
        int Count();
        void RemoveById(long id);
    }
}