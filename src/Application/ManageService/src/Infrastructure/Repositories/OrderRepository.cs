using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using FavoDeMel.Application.ManageService.Core;
using SqlKata.Execution;

namespace FavoDeMel.Application.ManageService.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly QueryFactory _queryFactory;

        public OrderRepository(QueryFactory queryFactory)
        {
            _queryFactory = queryFactory;
        }

        public async ValueTask AddAsync(Order order)
        {
            await _queryFactory.Query("order").InsertAsync(
                new
                {
                    id = order.Id,
                    position = order.Index,
                    name = order.Name,
                    description = order.Description,
                    status = order.Status,
                    order_tab_id = order.OrderTabId
                }
            );
        }

        public SortedList<int, Order> GetAll()
        {
            var results = _queryFactory
                .Query("order")
                .Select("id", "position", "name", "description", "order_tab_id", "status")
                .Get<dynamic>();

            var orders = new SortedList<int, Order>();
            foreach (var result in results)
            {
                var order = new Order((long)result.id, (int)result.position, (string)result.name, (string)result.description, (long)result.order_tab_id, (OrderStatus)result.status);
                orders.Add(order.Index, order);
            }

            return orders;
        }

        public async ValueTask<SortedList<int, Order>> GetAllAsync()
        {
            var results = await _queryFactory
                .Query("order")
                .Select("id", "position", "name", "description", "order_tab_id", "status")
                .GetAsync<dynamic>();

            var orders = new SortedList<int, Order>();
            foreach (var result in results)
            {
                var order = new Order((long)result.id, (int)result.position, (string)result.name, (string)result.description, (long)result.order_tab_id, (OrderStatus)result.status);
                orders.Add(order.Index, order);
            }

            return orders;
        }

        public async ValueTask<Order> GetByIdAsync(long id)
        {
            var result = await _queryFactory
                .Query("order")
                .Select("id", "position", "name", "description", "order_tab_id", "status")
                .Where("id", "=", id)
                .FirstOrDefaultAsync<dynamic>();
            
            return new Order((long)result.id, (int)result.position, (string)result.name, (string)result.description, (long)result.order_tab_id, (OrderStatus)result.status);
        }

        public async ValueTask<Order> GetByPositionAsync(int position)
        {
            var result = await _queryFactory
                .Query("order")
                .Select("id", "position", "name", "description", "order_tab_id", "status")
                .Where("position", "=", position)
                .FirstOrDefaultAsync<dynamic>();
            
            return new Order((long)result.id, (int)result.position, (string)result.name, (string)result.description, (long)result.order_tab_id, (OrderStatus)result.status);
        }

        public async ValueTask<SortedList<int, Order>> GetOrderByOrderTabIdAsync(long orderTabId)
        {
            var results = await _queryFactory
                .Query("order")
                .Select("id", "position", "name", "description", "order_tab_id", "status")
                .Where("order_tab_id", "=", orderTabId)
                .GetAsync<dynamic>();

            var orders = new SortedList<int, Order>();
            foreach (var result in results)
            {
                var order = new Order((long)result.id, (int)result.position, (string)result.name, (string)result.description, (long)result.order_tab_id, (OrderStatus)result.status);
                orders.Add(order.Index, order);
            }

            return orders;
        }

        public async ValueTask<int> CountAsync()
        {
            return await _queryFactory
                .Query("order")
                .SelectRaw("COUNT(id)")
                .FirstOrDefaultAsync<int>();
        }

        public async ValueTask RemoveByIdAsync(long id)
        {
            await _queryFactory
                .Query("order")
                .Where("id", "=", id)
                .DeleteAsync();
        }

        public async ValueTask UpdateAsync(Order order)
        {
            await _queryFactory
                .Query("order")
                .Where("id", "=", order.Id)
                .UpdateAsync(new
                {
                    id = order.Id,
                    position = order.Index,
                    name = order.Name,
                    description = order.Description,
                    status = order.Status,
                    order_tab_id = order.OrderTabId
                });
        }
    }
}