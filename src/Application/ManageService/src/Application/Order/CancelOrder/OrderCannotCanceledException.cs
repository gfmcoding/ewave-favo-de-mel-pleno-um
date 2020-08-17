using System;
using FavoDeMel.Infrastructure.Common.Exception;

namespace FavoDeMel.Application.ManageService.Application.Order.CancelOrder
{
    public class OrderCannotCanceledException : CustomException
    {
        public long Id { get; private set; }
        public OrderCannotCanceledException(long id, string message) : base(message)
        {
            Id = id;
        }
    }
}