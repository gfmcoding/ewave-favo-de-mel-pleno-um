using System.Text.Json;
using FavoDeMel.Application.ManageService.Queries.Order.GetAllOrders;
using Microsoft.AspNetCore.Mvc;

namespace FavoDeMel.WebApi.Resources.Order
{
    [Route("api/orders")]
    public class GetAllOrdersController : ControllerBase
    {
        private readonly GetAllOrdersQuery _getAllOrdersQuery;

        public GetAllOrdersController(GetAllOrdersQuery getAllOrdersQuery)
        {
            _getAllOrdersQuery = getAllOrdersQuery;
        }

        /// <summary>
        /// Obtem todos pedidos em aberto.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/orders
        /// </remarks>
        /// <response code="200">Retorna os pedidos encontrados.</response>
        /// <response code="500">Erro interno</response> 
        [HttpGet]
        [ApiExplorerSettings(GroupName = "Orders")]
        public IActionResult Handler()
        {
            var orders = _getAllOrdersQuery.Exec();
            var ordersSerialized = JsonSerializer.Serialize(orders);

            return Ok(ordersSerialized);
        }
    }
}