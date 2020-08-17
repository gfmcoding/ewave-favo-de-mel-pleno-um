using System.Text.Json;
using FavoDeMel.Application.ManageService.Queries.Order.GetById;
using Microsoft.AspNetCore.Mvc;

namespace FavoDeMel.WebApi.Resources.Order
{
    [Route("api/orders")]
    public class GetByIdController : ControllerBase
    {
        private readonly GetByIdQuery _getByIdQuery;

        public GetByIdController(GetByIdQuery getByIdQuery)
        {
            _getByIdQuery = getByIdQuery;
        }

        /// <summary>
        /// Obtem um pedido em especifico.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/orders/123123123123
        /// </remarks>
        /// <param name="id" example="744582059380482048">O id do pedido</param>
        /// <response code="200">Retorna o pedido encontrado.</response>
        /// <response code="404">Nenhum pedido com esse id foi encontrado.</response>
        /// <response code="500">Erro interno</response> 
        [HttpGet("{id:long}")]
        [ApiExplorerSettings(GroupName = "Orders")]
        public IActionResult Handler([FromRoute] long id)
        {
            var order = _getByIdQuery.Exec(id);
            
            if (order is null) return NotFound();
            return Ok(JsonSerializer.Serialize(order));
        }
    }
}