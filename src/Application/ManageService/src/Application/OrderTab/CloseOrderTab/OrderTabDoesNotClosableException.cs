using System;
using FavoDeMel.Infrastructure.Common.Exception;

namespace FavoDeMel.Application.ManageService.Application.OrderTab.CloseOrderTab
{
    public class OrderTabDoesNotClosableException : CustomException
    {
        public long OrderTabId { get; private set; } 
        
        public OrderTabDoesNotClosableException(long orderTabId, string message) : base(message)
        {
            OrderTabId = orderTabId;
        }
    }
}