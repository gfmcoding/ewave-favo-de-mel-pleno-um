using System.Text.Json;
using System.Threading.Tasks;
using FavoDeMel.Application.ManageService.Application.Order;
using FavoDeMel.Application.ManageService.Application.Order.ToOrderInDelivery;
using Microsoft.AspNetCore.Mvc;
using STAN.Client;

namespace FavoDeMel.WebApi.Resources.Order
{
    [Route("api/orders")]
    public class ToOrderInDeliveryController : ControllerBase
    {
        private readonly IStanConnection _stanConnection;

        public ToOrderInDeliveryController(IStanConnection stanConnection)
        {
            _stanConnection = stanConnection;
        }

        /// <summary>
        /// Colocar um pedido como entregue.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/orders/12312312444123123/to-delivery
        ///     {
        ///        "id": 744578323308388352
        ///     }
        ///
        /// </remarks>
        /// <param name="id" example="744578323308388352">O id do pedido</param>
        /// <response code="202">O pedido para colocar um pedido como entregue foi aceito.</response>
        /// <response code="500">Erro interno</response>
        [HttpPut("{id:long}/to-delivery")]
        [ApiExplorerSettings(GroupName = "Orders")]
        public async ValueTask<IActionResult> Handler([FromBody] ToOrderInDeliveryRequest request)
        {
            var orderRequest = new OrderRequest {Type = OrderRequestTypes.ToOrderInDelivery, Request = request};
            var requestSerialized = JsonSerializer.SerializeToUtf8Bytes(orderRequest);

            await _stanConnection.PublishAsync("order_handler", requestSerialized);

            return Accepted();
        }
    }
}