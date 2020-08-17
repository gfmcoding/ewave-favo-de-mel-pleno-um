using System;
using System.Text.Json;
using System.Threading.Tasks;
using FavoDeMel.Application.ManageService.Application.Order;
using FavoDeMel.Application.ManageService.Application.Order.PlaceNewOrder;
using Microsoft.AspNetCore.Mvc;
using STAN.Client;

namespace FavoDeMel.WebApi.Resources.Order
{
    [Route("api/order-tabs")]
    public class PlaceNewOrderController : ControllerBase
    {
        private readonly IStanConnection _stanConnection;

        public PlaceNewOrderController(IStanConnection stanConnection)
        {
            _stanConnection = stanConnection;
        }

        /// <summary>
        /// Abre um novo pedido em uma comanda.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/order-tabs/744585199437783040/orders
        ///     {
        ///        "name": "Pedido",
        ///        "description": "Descrição",
        ///        "order_tab_id": 744585199437783040
        ///     }
        ///
        /// </remarks>
        /// <param name="order tab id" example="744585199437783040">O id da comanda</param>
        /// <response code="202">O pedido para abrir um novo pedido foi aceito.</response>
        /// <response code="500">Erro interno</response>
        [HttpPost("{orderTabId:long}/orders")]
        [ApiExplorerSettings(GroupName = "Orders")]
        public async ValueTask<IActionResult> Handler([FromRoute] long orderTabId, [FromBody] PlaceNewOrderRequest request)
        {
            if (string.IsNullOrEmpty(request.Name)) throw new ArgumentNullException("nome", "O nome do pedido deve ser informado.");
            if (string.IsNullOrEmpty(request.Description)) throw new ArgumentNullException("descrição", "A descrição deve ser informada.");

            var orderRequest = new OrderRequest {Type = OrderRequestTypes.PlaceNewOrder, Request = request};
            var requestSerialized = JsonSerializer.SerializeToUtf8Bytes(orderRequest);

            await _stanConnection.PublishAsync("order_handler", requestSerialized);

            return Accepted();
        }
    }
}