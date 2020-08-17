using FavoDeMel.Infrastructure.Common.Exception;

namespace FavoDeMel.Application.ManageService.Application.Order.ToDoneOrder
{
    public class ToDoneOrderException : CustomException
    {
        public ToDoneOrderException(string message) : base(message)
        {
        }
    }
}