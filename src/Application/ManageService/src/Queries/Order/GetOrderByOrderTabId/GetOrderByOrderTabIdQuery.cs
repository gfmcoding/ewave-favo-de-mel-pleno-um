using System.Collections.Generic;
using System.Collections.ObjectModel;
using FavoDeMel.Application.ManageService.Core;

namespace FavoDeMel.Application.ManageService.Queries.Order.GetOrderByOrderTabId
{
    public class GetOrderByOrderTabIdQuery
    {
        private readonly IOrderRepositoryInMemory _repositoryInMemory;

        public GetOrderByOrderTabIdQuery(IOrderRepositoryInMemory repositoryInMemory)
        {
            _repositoryInMemory = repositoryInMemory;
        }

        public IEnumerable<Result> Exec(long orderTabId)
        {
            var orders = _repositoryInMemory.GetOrderByOrderTabId(orderTabId);
            
            var results = new Collection<Result>();
            foreach (var order in orders)
            {
                var result = new Result
                {
                    Id = order.Value.Id,
                    Position = order.Key,
                    Name = order.Value.Name,
                    Description = order.Value.Description,
                    Status = (int) order.Value.Status,
                    OrderTabId = order.Value.OrderTabId
                };
                results.Add(result);
            }

            return results;
        }
    }
}