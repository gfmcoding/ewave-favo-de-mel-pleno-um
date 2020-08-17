using FavoDeMel.Application.ManageService.Core;

namespace FavoDeMel.Application.ManageService.Queries.Order.GetById
{
    public class GetByIdQuery
    {
        private readonly IOrderRepositoryInMemory _repositoryInMemory;

        public GetByIdQuery(IOrderRepositoryInMemory repositoryInMemory)
        {
            _repositoryInMemory = repositoryInMemory;
        }

        public Result Exec(long id)
        {
            var order = _repositoryInMemory.GetById(id);
            if (order is null) return null;
            
            return  new Result
            {
                Id = order.Id,
                Position = order.Index,
                Name = order.Name,
                Description = order.Description,
                Status = (int) order.Status,
                OrderTabId = order.OrderTabId
            };
        }
    }
}