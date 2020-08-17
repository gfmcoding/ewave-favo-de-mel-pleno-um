using System.Threading.Tasks;
using FavoDeMel.Application.ManageService.Application.OrderTab.OpenOrderTab;
using Microsoft.AspNetCore.Mvc;

namespace FavoDeMel.WebApi.Resources.OrderTab
{
    [Route("api/order-tabs")]
    public class OpenOrderTabController : ControllerBase
    {
        private readonly OpenOrderTabUseCase _useCase;

        public OpenOrderTabController(OpenOrderTabUseCase useCase)
        {
            _useCase = useCase;
        }

        /// <summary>
        /// Abre uma comanda.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/order-tabs
        ///     {
        ///        "table_number": 777
        ///     }
        ///
        /// </remarks>
        /// <param name="table number" example="744582059380482048">Numero da mesa correspondente daquela comanda</param>
        /// <response code="201">A comanda foi aberta.</response>
        /// <response code="400">Parametro invalido.</response>
        /// <response code="500">Erro interno</response>
        [HttpPost]
        [ApiExplorerSettings(GroupName = "Order Tabs")]
        public async ValueTask<IActionResult> Handler([FromBody] OpeningOrderTabRequest request)
        {
            var id = await _useCase.ExecAsync(request);

            return Created($"api/OrderTabs/{id}", id);
        }
    }
}