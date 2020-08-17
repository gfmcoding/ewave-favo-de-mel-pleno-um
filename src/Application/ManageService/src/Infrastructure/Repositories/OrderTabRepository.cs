using System.Collections.Generic;
using System.Threading.Tasks;
using FavoDeMel.Application.ManageService.Core;
using SqlKata.Execution;

namespace FavoDeMel.Application.ManageService.Infrastructure.Repositories
{
    public class OrderTabRepository : IOrderTabRepository
    {
        private readonly QueryFactory _queryFactory;

        public OrderTabRepository(QueryFactory queryFactory)
        {
            _queryFactory = queryFactory;
        }

        public async ValueTask AddAsync(OrderTab orderTab)
        {
            await _queryFactory.Query("order_tab").InsertAsync(
                new
                {
                    id = orderTab.Id,
                    table_number = orderTab.TableNumber,
                    is_closed = orderTab.IsClosed
                }
            );
        }

        public async ValueTask UpdateAsync(OrderTab orderTab)
        {
            await _queryFactory
                .Query("order_tab")
                .Where("id", "=", orderTab.Id)
                .UpdateAsync(new
                {
                    id = orderTab.Id,
                    table_number = orderTab.TableNumber,
                    is_closed = orderTab.IsClosed
                });
        }

        public async ValueTask<OrderTab> GetByIdAsync(long id)
        {
            var result = await _queryFactory
                .Query("order_tab")
                .Join("order", "order.order_tab_id", "order_tab.id")
                .Select("id", "table_number", "is_closed")
                .FirstOrDefaultAsync<dynamic>();

            return new OrderTab((long)result.id, (int)result.table_number, new List<Order>(result.order));
        }

        public HashSet<OrderTab> GetAll()
        {
            var results = _queryFactory
                .Query("order_tab")
                .Join("order", "order.order_tab_id", "order_tab.id")
                .Select("id", "table_number", "is_closed")
                .Get<dynamic>();

            var orderTabs = new HashSet<OrderTab>();
            foreach (var result in results)
            {
                var orderTab = new OrderTab((long)result.id, (int)result.table_number, new List<Order>(result.order));
                orderTabs.Add(orderTab);
            }

            return orderTabs;
        }

        public async ValueTask<HashSet<OrderTab>> GetAllAsync()
        {
            var results = await _queryFactory
                .Query("order_tab")
                .Join("order", "order.order_tab_id", "order_tab.id")
                .Select("id", "table_number", "is_closed")
                .GetAsync<dynamic>();

            var orderTabs = new HashSet<OrderTab>();
            foreach (var result in results)
            {
                var orderTab = new OrderTab((long)result.id, (int)result.table_number, new List<Order>(result.order));
                orderTabs.Add(orderTab);
            }

            return orderTabs;
        }

        public async ValueTask RemoveByIdAsync(long id)
        {
            await _queryFactory
                .Query("order_tab")
                .Where("id", "=", id)
                .DeleteAsync();
        }

        public async ValueTask<bool> ExistByTableNumberAsync(int tableNumber)
        {
            var result = await _queryFactory
                .Query("order_tab")
                .SelectRaw("COUNT(id)")
                .Where("table_number", "=", tableNumber)
                .FirstOrDefaultAsync<int>();

            return result == 1;
        }
    }
}