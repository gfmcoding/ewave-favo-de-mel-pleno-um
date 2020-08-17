using System.Collections.Generic;

namespace FavoDeMel.Application.ManageService.Core
{
    public interface IOrderTabRepositoryInMemory
    {
        void Add(OrderTab orderTab);
        OrderTab GetById(long id);
        HashSet<OrderTab> GetAll();
        void RemoveById(long id);
        bool ExistByTableNumber(int tableNumber);
    }
}