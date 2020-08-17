using System.Text.Json;
using FavoDeMel.Application.ManageService.Queries.OrderTab.GetAllOrderTab;
using Microsoft.AspNetCore.Mvc;

namespace FavoDeMel.WebApi.Resources.OrderTab
{
    [Route("api/order-tabs")]
    public class GetAllOrderTabController : ControllerBase
    {
        private readonly GetAllOrderTabQuery _getAllOrderTabQuery;

        public GetAllOrderTabController(GetAllOrderTabQuery getAllOrderTabQuery)
        {
            _getAllOrderTabQuery = getAllOrderTabQuery;
        }

        /// <summary>
        /// Obtem todas comandas em aberto.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/order-tabs
        /// </remarks>
        /// <response code="200">Retorna as comandas encontradas.</response>
        /// <response code="500">Erro interno</response> 
        [HttpGet]
        [ApiExplorerSettings(GroupName = "Order Tabs")]
        public IActionResult Handler()
        {
            var orderTabs = _getAllOrderTabQuery.Exec();
            var orderTabsSerialized = JsonSerializer.Serialize(orderTabs);

            return Ok(orderTabsSerialized);
        }
    }
}