using System.Collections.Generic;

namespace FavoDeMel.Application.ManageService.Core
{
    public interface IOrderTabRepository
    {
        void Add(OrderTab orderTab);
        OrderTab GetById(long id);
        HashSet<OrderTab> GetAll();
        void RemoveById(long id);
    }
}