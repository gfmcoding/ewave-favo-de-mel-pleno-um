using System.Text.Json;
using FavoDeMel.Application.ManageService.Queries.Order.GetOrderByOrderTabId;
using Microsoft.AspNetCore.Mvc;

namespace FavoDeMel.WebApi.Resources.Order
{
    [Route("api/order-tabs")]
    public class GetOrderByOrderTabIdController : ControllerBase
    {
        private readonly GetOrderByOrderTabIdQuery _getOrderByOrderTabIdQuery;

        public GetOrderByOrderTabIdController(GetOrderByOrderTabIdQuery getOrderByOrderTabIdQuery)
        {
            _getOrderByOrderTabIdQuery = getOrderByOrderTabIdQuery;
        }

        /// <summary>
        /// Obtem os pedidos de uma comanda em especifico.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/orders-tabs/123123123123/orders
        /// </remarks>
        /// <param name="orders tab id" example="744582059380482048">O id da comanda.</param>
        /// <response code="200">Retorna os pedidos encontrados daquela comanda.</response>
        /// <response code="404">Nenhum pedido com dessa comanda foi encontrado.</response>
        /// <response code="500">Erro interno</response>
        [HttpGet("{OrderTabId:long}/orders")]
        [ApiExplorerSettings(GroupName = "Orders")]
        public IActionResult Handle([FromRoute] long orderTabId)
        {
            var order = _getOrderByOrderTabIdQuery.Exec(orderTabId);
            return Ok(JsonSerializer.Serialize(order));
        }
    }
}