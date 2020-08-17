using System;
using FavoDeMel.Infrastructure.Common.Exception;

namespace FavoDeMel.Application.ManageService.Application.OrderTab.OpenOrderTab
{
    public class InvalidTableNumberException : CustomException
    {
        public int TableNumber { get; private set; }
        public InvalidTableNumberException(int tableNumber, string message) : base(message)
        {
            TableNumber = tableNumber;
        }
    }
}