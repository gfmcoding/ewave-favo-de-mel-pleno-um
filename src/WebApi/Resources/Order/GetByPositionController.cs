using System.Text.Json;
using FavoDeMel.Application.ManageService.Queries.Order.GetByPosition;
using Microsoft.AspNetCore.Mvc;

namespace FavoDeMel.WebApi.Resources.Order
{
    [Route("api/orders")]
    public class GetByPositionController : ControllerBase
    {
        private readonly GetByPositionQuery _getByPositionQuery;

        public GetByPositionController(GetByPositionQuery getByPositionQuery)
        {
            _getByPositionQuery = getByPositionQuery;
        }

        /// <summary>
        /// Obtem um pedido em especifico pela sua posição.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/orders/positions/123123123123
        /// </remarks>
        /// <param name="posição" example="744582059380482048">A posição do pedido.</param>
        /// <response code="200">Retorna o pedido encontrado.</response>
        /// <response code="404">Nenhum pedido com esse id foi encontrado.</response>
        /// <response code="500">Erro interno</response>
        [HttpGet("positions/{position:int}")]
        [ApiExplorerSettings(GroupName = "Orders")]
        public IActionResult Handler([FromRoute] int position)
        {
            var order = _getByPositionQuery.Exec(position);
            if (order is null) return NotFound();
            
            var orderSerialized = JsonSerializer.Serialize(order);
            return Ok(orderSerialized);
        }
    }
}