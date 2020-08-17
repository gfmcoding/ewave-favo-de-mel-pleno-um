using System.Collections.Generic;
using System.Threading.Tasks;

namespace FavoDeMel.Application.ManageService.Core
{
    public interface IOrderTabRepository
    {
        ValueTask AddAsync(OrderTab orderTab);
        ValueTask UpdateAsync(OrderTab orderTab);
        ValueTask<OrderTab> GetByIdAsync(long id);
        HashSet<OrderTab> GetAll();
        ValueTask<HashSet<OrderTab>> GetAllAsync();
        ValueTask RemoveByIdAsync(long id);
        ValueTask<bool> ExistByTableNumberAsync(int tableNumber);
    }
}