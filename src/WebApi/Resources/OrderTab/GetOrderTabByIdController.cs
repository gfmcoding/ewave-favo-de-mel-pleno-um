using System.Text.Json;
using FavoDeMel.Application.ManageService.Queries.OrderTab.GetOrderTabById;
using Microsoft.AspNetCore.Mvc;

namespace FavoDeMel.WebApi.Resources.OrderTab
{
    [Route("api/order-tabs")]
    public class GetOrderTabByIdController : ControllerBase
    {
        private readonly GetOrderTabByIdQuery _getOrderTabByIdQuery;

        public GetOrderTabByIdController(GetOrderTabByIdQuery getOrderTabByIdQuery)
        {
            _getOrderTabByIdQuery = getOrderTabByIdQuery;
        }

        /// <summary>
        /// Obtem uma comanda em especifico.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/order-tabss/123123123123
        /// </remarks>
        /// <param name="id" example="123123123123">O id da comanda</param>
        /// <response code="200">Retorna a comanda encontrada.</response>
        /// <response code="404">Nenhuma comanda com esse id foi encontrada.</response>
        /// <response code="500">Erro interno</response>
        [HttpGet("{id:long}")]
        [ApiExplorerSettings(GroupName = "Order Tabs")]
        public IActionResult Handle([FromRoute] long id)
        {
            var orderTab = _getOrderTabByIdQuery.Exec(id);
            if (orderTab is null) return NotFound();

            return Ok(JsonSerializer.Serialize(orderTab));
        }
    }
}