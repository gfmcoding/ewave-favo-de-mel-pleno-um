using FavoDeMel.Infrastructure.Common.Exception;

namespace FavoDeMel.Application.ManageService.Application.OrderTab.CloseOrderTab
{
    public class CloseOrderTabException : CustomException
    {
        public CloseOrderTabException(string message) : base(message)
        {
        }
    }
}