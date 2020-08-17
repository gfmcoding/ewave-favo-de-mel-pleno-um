using FavoDeMel.Infrastructure.Common.Exception;

namespace FavoDeMel.Application.ManageService.Application.Order.ReprioritizeOrder
{
    public class ReprioritizeOrderException : CustomException
    {
        public ReprioritizeOrderException(string message) : base(message)
        {
        }
    }
}