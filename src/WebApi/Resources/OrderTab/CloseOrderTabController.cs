using System.Threading.Tasks;
using FavoDeMel.Application.ManageService.Application.OrderTab.CloseOrderTab;
using Microsoft.AspNetCore.Mvc;

namespace FavoDeMel.WebApi.Resources.OrderTab
{
    [Route("api/order-tabs")]
    public class CloseOrderTabController : ControllerBase
    {
        private readonly CloseOrderTabUseCase _useCase;

        public CloseOrderTabController(CloseOrderTabUseCase useCase)
        {
            _useCase = useCase;
        }

        /// <summary>
        /// Fecha uma comanda.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/order-tabs/744582059380482048
        ///     {
        ///        "Id": 744582059380482048
        ///     }
        ///
        /// </remarks>
        /// <param name="id" example="744582059380482048">O id da comanda</param>
        /// <response code="200">A comanda foi fechada.</response>
        /// <response code="400">Parametro invalido.</response>
        /// <response code="500">Erro interno</response>
        [HttpDelete("{id:long}")]
        [ApiExplorerSettings(GroupName = "Order Tabs")]
        public async ValueTask<IActionResult> Handler([FromRoute] long id, [FromBody] CloseOrderTabRequest request)
        {
            await _useCase.ExecAsync(request);

            return Ok();
        }
    }
}