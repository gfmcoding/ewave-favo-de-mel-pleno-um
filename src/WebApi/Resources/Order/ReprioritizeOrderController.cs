using System;
using System.Text.Json;
using System.Threading.Tasks;
using FavoDeMel.Application.ManageService.Application.Order;
using FavoDeMel.Application.ManageService.Application.Order.ReprioritizeOrder;
using Microsoft.AspNetCore.Mvc;
using STAN.Client;

namespace FavoDeMel.WebApi.Resources.Order
{
    [Route("api/orders")]
    public class ReprioritizeOrderController : ControllerBase
    {
        private readonly IStanConnection _stanConnection;

        public ReprioritizeOrderController(IStanConnection stanConnection)
        {
            _stanConnection = stanConnection;
        }

        /// <summary>
        /// Reprioriza um pedido.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/orders/reprioritize
        ///     {
        ///        "position": 4,
        ///        "new_position": 1
        ///     }
        ///
        /// </remarks>
        /// <response code="202">O pedido para repriorizar um pedido foi aceito.</response>
        /// <response code="500">Erro interno</response>
        [HttpPut("reprioritize")]
        [ApiExplorerSettings(GroupName = "Orders")]
        public async ValueTask<IActionResult> Handler([FromBody] ReprioritizeOrderRequest request)
        {
            if(request.Position <= 0) throw new ArgumentException("A posição tem que ser um valor positivo.", "posição");
            if(request.NewPosition <= 0) throw new ArgumentException("A nova posição tem que ser um valor positivo.", "nova posição");
            
            var orderRequest = new OrderRequest {Type = OrderRequestTypes.ReprioritizeOrder, Request = request};
            var requestSerialized = JsonSerializer.SerializeToUtf8Bytes(orderRequest);

            await _stanConnection.PublishAsync("order_handler", requestSerialized);

            return Accepted();
        }
    }
}