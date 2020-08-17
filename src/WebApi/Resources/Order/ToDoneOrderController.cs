using System.Text.Json;
using System.Threading.Tasks;
using FavoDeMel.Application.ManageService.Application.Order;
using FavoDeMel.Application.ManageService.Application.Order.ToDoneOrder;
using Microsoft.AspNetCore.Mvc;
using STAN.Client;

namespace FavoDeMel.WebApi.Resources.Order
{
    [Route("api/orders")]
    public class ToDoneOrderController : ControllerBase
    {
        private readonly IStanConnection _stanConnection;

        public ToDoneOrderController(IStanConnection stanConnection)
        {
            _stanConnection = stanConnection;
        }

        /// <summary>
        /// Colocar um pedido como feito.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/orders/12312312444123123/to-done
        ///     {
        ///        "id": 744578323308388352
        ///     }
        ///
        /// </remarks>
        /// <param name="id" example="744578323308388352">O id do pedido</param>
        /// <response code="202">O pedido para colocar um pedido como feito foi aceito.</response>
        /// <response code="500">Erro interno</response>
        [HttpPut("{id:long}/to-done")]
        [ApiExplorerSettings(GroupName = "Orders")]
        public async ValueTask<IActionResult> Handler([FromBody] ToDoneOrderRequest request)
        {
            var orderRequest = new OrderRequest {Type = OrderRequestTypes.ToOrderInDone, Request = request};
            var requestSerialized = JsonSerializer.SerializeToUtf8Bytes(orderRequest);

            await _stanConnection.PublishAsync("order_handler", requestSerialized);

            return Accepted();
        }
    }
}