using System.Collections.Generic;
using System.Threading.Tasks;

namespace FavoDeMel.Application.ManageService.Core
{
    public interface IOrderRepository
    {
        ValueTask AddAsync(Order order);
        SortedList<int, Order> GetAll();
        ValueTask<SortedList<int, Order>> GetAllAsync();
        ValueTask<Order> GetByIdAsync(long id);
        ValueTask<Order> GetByPositionAsync(int position);
        ValueTask<SortedList<int, Order>> GetOrderByOrderTabIdAsync(long orderTabId);
        ValueTask<int> CountAsync();
        ValueTask RemoveByIdAsync(long id);
        ValueTask UpdateAsync(Order order);
    }
}