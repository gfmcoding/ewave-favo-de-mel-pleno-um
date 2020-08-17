using FavoDeMel.Application.ManageService.Core;

namespace FavoDeMel.Application.ManageService.Queries.Order.GetByPosition
{
    public class GetByPositionQuery
    {
        private readonly IOrderRepositoryInMemory _repositoryInMemory;

        public GetByPositionQuery(IOrderRepositoryInMemory repositoryInMemory)
        {
            _repositoryInMemory = repositoryInMemory;
        }

        public Result Exec(int position)
        {
            var order = _repositoryInMemory.GetByPosition(position);
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