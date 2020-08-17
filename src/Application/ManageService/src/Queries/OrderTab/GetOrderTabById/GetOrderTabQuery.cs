using FavoDeMel.Application.ManageService.Core;

namespace FavoDeMel.Application.ManageService.Queries.OrderTab.GetOrderTabById
{
    public class GetOrderTabByIdQuery
    {
        private readonly IOrderTabRepositoryInMemory _repositoryInMemory;

        public GetOrderTabByIdQuery(IOrderTabRepositoryInMemory repositoryInMemory)
        {
            _repositoryInMemory = repositoryInMemory;
        }

        public Result Exec(long id)
        {
            var orderTab = _repositoryInMemory.GetById(id);
            if (orderTab == null) return null;
            
            return new Result
            {
                Id = orderTab.Id,
                TableNumber = orderTab.TableNumber,
                IsClosed = orderTab.IsClosed,
                Orders = orderTab.Orders
            };
        }
    }
}