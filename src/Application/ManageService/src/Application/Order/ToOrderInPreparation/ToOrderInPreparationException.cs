using FavoDeMel.Infrastructure.Common.Exception;

namespace FavoDeMel.Application.ManageService.Application.Order.ToOrderInPreparation
{
    public class ToOrderInPreparationException : CustomException
    {
        public ToOrderInPreparationException(string message) : base(message)
        {
        }
    }
}