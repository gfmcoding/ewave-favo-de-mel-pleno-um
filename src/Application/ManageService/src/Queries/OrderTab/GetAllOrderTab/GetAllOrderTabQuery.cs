using System.Collections.Generic;
using System.Collections.ObjectModel;
using FavoDeMel.Application.ManageService.Core;

namespace FavoDeMel.Application.ManageService.Queries.OrderTab.GetAllOrderTab
{
    public class GetAllOrderTabQuery
    {
        private readonly IOrderTabRepositoryInMemory _repositoryInMemory;

        public GetAllOrderTabQuery(IOrderTabRepositoryInMemory repositoryInMemory)
        {
            _repositoryInMemory = repositoryInMemory;
        }

        public IEnumerable<Result> Exec()
        {
            var orderTabs = _repositoryInMemory.GetAll();

            var results = new Collection<Result>();
            foreach (var orderTab in orderTabs)
            {
                var result = new Result
                {
                    Id = orderTab.Id,
                    TableNumber = orderTab.TableNumber,
                    IsClosed = orderTab.IsClosed,
                    Orders = orderTab.Orders
                };
                results.Add(result);
            }

            return results;
        }
    }
}