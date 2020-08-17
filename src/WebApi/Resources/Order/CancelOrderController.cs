using System.ComponentModel;
using System.Text.Json;
using System.Threading.Tasks;
using FavoDeMel.Application.ManageService.Application.Order;
using FavoDeMel.Application.ManageService.Application.Order.CancelOrder;
using Microsoft.AspNetCore.Mvc;
using STAN.Client;

namespace FavoDeMel.WebApi.Resources.Order
{
    [Route("api/orders")]
    public class CancelOrderController : ControllerBase
    {
        private readonly IStanConnection _stanConnection;

        public CancelOrderController(IStanConnection stanConnection)
        {
            _stanConnection = stanConnection;
        }
        
        /// <summary>
        /// Cancela um pedido.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/orders/744582059380482048
        ///     {
        ///        "order_tab_id": 744581978342334464,
        ///        "order_id": 744582059380482048
        ///     }
        ///
        /// </remarks>
        /// <param name="id" example="744582059380482048">O id do pedido</param>
        /// <param name="cancel order request"></param>
        /// <response code="202">O pedido para cancelar um pedido foi aceito.</response>
        /// <response code="500">Erro interno</response> 
        [HttpDelete("{id:long}")]
        [ApiExplorerSettings(GroupName = "Orders")]
        public async ValueTask<IActionResult> Handler([FromBody] CancelOrderRequest request)
        {
            var orderRequest = new OrderRequest {Type = OrderRequestTypes.CancelOrder, Request = request};
            var requestSerialized = JsonSerializer.SerializeToUtf8Bytes(orderRequest);

            await _stanConnection.PublishAsync("order_handler", requestSerialized);

            return Accepted();
        }
    }
}