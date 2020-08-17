using FavoDeMel.Infrastructure.Common.Exception;

namespace FavoDeMel.Application.ManageService.Application.Order.ToOrderInDelivery
{
    public class ToOrderInDeliveryException : CustomException
    {
        public ToOrderInDeliveryException(string message) : base(message)
        {
        }
    }
}