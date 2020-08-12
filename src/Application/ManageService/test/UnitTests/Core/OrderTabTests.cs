using System.Collections.Generic;
using FavoDeMel.Application.ManageService.Core;
using FavoDeMel.Infrastructure.Common.IdGenerator;
using Moq;
using Xunit;

namespace FavoDeMel.Application.ManageService.UnitTests.Core
{
    public class OrderTabTests
    {
        private IIdGenerator _idGenerator;
        
        public OrderTabTests()
        {
            var idGeneratorMock = new Mock<IIdGenerator>();
            idGeneratorMock
                .Setup(m => m.Generate())
                .Returns(10);
            _idGenerator = idGeneratorMock.Object;
        }
        
        [Fact]
        public void Open_A_New_OrderTab()
        {
            //Arrange
            const long id = 1;
            const int tableNumber = 1;

            //Action
            var orderTab = new OrderTab(id, tableNumber, _idGenerator);
            
            //Assert
            Assert.NotNull(orderTab);
            Assert.Equal(id, orderTab.Id);
            Assert.Equal(tableNumber, orderTab.TableNumber);
        }
        
        [Fact]
        public void Reopen_A_OrderTab()
        {
            //Arrange
            const long id = 1;
            const int tableNumber = 1;
            var orders = new List<Order>
            {
                new Order(1, 1, "Teste", "Testando", OrderStatus.InPreparation)
            };

            //Action
            var orderTab = new OrderTab(id, tableNumber, orders, _idGenerator);
            
            //Assert
            Assert.NotNull(orderTab);
            Assert.Equal(id, orderTab.Id);
            Assert.Equal(tableNumber, orderTab.TableNumber);
            Assert.NotNull(orderTab.Orders);
        }

        [Fact]
        public void Add_A_New_Order()
        {
            //Arrange
            const long id = 1;
            const int tableNumber = 1;
            var orderTab = new OrderTab(id, tableNumber, _idGenerator);

            //Action
            orderTab.AddOrder(1, "Teste", "Testando");
            
            //Assert
            Assert.Single(orderTab.Orders);
        }
        
        [Fact]
        public void Cancel_A_Order()
        {
            //Arrange
            const long id = 1;
            const int tableNumber = 1;
            
            var orders = new List<Order>
            {
                new Order(1, 1, "Test", "Testando", OrderStatus.AwaitingPreparation)
            };
            var orderTab = new OrderTab(id, tableNumber, orders, _idGenerator);

            //Action
            var result = orderTab.CancelOrder(1);

            //Assert
            Assert.Empty(orderTab.Orders);
        }
        
        [Fact]
        public void Try_Cancel_A_Order_Dont_Nonexistent()
        {
            //Arrange
            const long id = 1;
            const int tableNumber = 1;
            
            var orders = new List<Order>
            {
                new Order(1, 1, "Test", "Testando", OrderStatus.InPreparation)
            };
            var orderTab = new OrderTab(id, tableNumber, orders, _idGenerator);

            //Action
            var result = orderTab.CancelOrder(1);

            //Assert
            Assert.Single(orderTab.Orders);
        }
        
        [Fact]
        public void Try_Close_A_OrderTab()
        {
            //Arrange
            const long id = 1;
            const int tableNumber = 1;
            
            var orders = new List<Order>
            {
                new Order(1, 1, "Test", "Testando", OrderStatus.Canceled)
            };
            var orderTab = new OrderTab(id, tableNumber, orders, _idGenerator);
            
            //Action
            var result = orderTab.TryClose();
            
            //Assert
            Assert.True(result);
            Assert.True(orderTab.IsClosed);
        }
        
        [Fact]
        public void Try_Close_A_OrderTab_When_Dont_Contains_Orders()
        {
            //Arrange
            const long id = 1;
            const int tableNumber = 1;

            var orders = new List<Order>();
            var orderTab = new OrderTab(id, tableNumber, orders, _idGenerator);
            
            //Action
            var result = orderTab.TryClose();
            
            //Assert
            Assert.True(result);
            Assert.True(orderTab.IsClosed);
        }
        
        [Fact]
        public void Try_Close_A_OrderTab_When_Some_Order_Is_Progress()
        {
            //Arrange
            const long id = 1;
            const int tableNumber = 1;

            var orders = new List<Order>
            {
                new Order(1, 1, "Test", "Testando", OrderStatus.InPreparation)
            };
            var orderTab = new OrderTab(id, tableNumber, orders, _idGenerator);
            
            //Action
            var result = orderTab.TryClose();
            
            //Assert
            Assert.False(result);
            Assert.False(orderTab.IsClosed);
        }
    }
}