using System.Linq;
using FavoDeMel.Application.ManageService.Core;
using FavoDeMel.Application.ManageService.Infrastructure.Repositories;
using Xunit;

namespace FavoDeMel.Application.ManageService.UnitTests.Infrastructure
{
    public class OrderRepositoryInMemoryTests
    {
        [Fact]
        public void Add_A_New_Order()
        {
            //Arrange
            var orderRepository = new OrderRepositoryInMemory();
            var order = new Order(1, 1, "Teste", "Testando", 1, OrderStatus.AwaitingPreparation);
            
            //Action
            orderRepository.Add(order);
            
            //Assert
            var expectedOrder = orderRepository.GetById(order.Id);
            Assert.NotNull(expectedOrder);
            Assert.Equal(expectedOrder, order);
            Assert.Equal(1, orderRepository.Count());
        }

        [Fact]
        public void Get_All_Orders()
        {
            //Arrange
            var orderRepository = new OrderRepositoryInMemory();
            
            for (var i = 0; i < 3; i++)
            {
                var order = new Order(i, i, "Teste", "Testando", 1, OrderStatus.AwaitingPreparation);
                orderRepository.Add(order);
            }

            //Action
            var orders = orderRepository.GetAll();

            //Assert
            Assert.NotNull(orders);
            Assert.Equal(3, orderRepository.Count());
        }
        
        [Fact]
        public void Get_A_Order_By_Id()
        {
            //Arrange
            var orderRepository = new OrderRepositoryInMemory();
            var order = new Order(1, 1, "Teste", "Testando", 1, OrderStatus.AwaitingPreparation);
            orderRepository.Add(order);
            
            //Action
            var expectedOrder = orderRepository.GetById(order.Id);
            
            //Assert
            Assert.NotNull(expectedOrder);
            Assert.Equal(expectedOrder, order);
        }
        
        [Fact]
        public void Get_A_Order_By_Position()
        {
            //Arrange
            var orderRepository = new OrderRepositoryInMemory();
            var order = new Order(1, 1, "Teste", "Testando", 1, OrderStatus.AwaitingPreparation);
            orderRepository.Add(order);
            
            //Action
            var expectedOrder = orderRepository.GetByPosition(order.Index);
            
            //Assert
            Assert.NotNull(expectedOrder);
            Assert.Equal(expectedOrder, order);
        }
        
        [Fact]
        public void Get_A_Order_By_OrderTab_Id()
        {
            //Arrange
            var orderRepository = new OrderRepositoryInMemory();
            const long orderTabId = 1;
            
            for (var i = 0; i < 3; i++)
            {
                var order = new Order(i, i, "Teste", "Testando", orderTabId, OrderStatus.AwaitingPreparation);
                orderRepository.Add(order);
            }
            
            //Action
            var orderTabOrders = orderRepository.GetOrderTabOrders(1);
            
            //Assert
            Assert.NotNull(orderTabOrders);
            
            var result = orderTabOrders.All(o => o.Value.OrderTabId == orderTabId);
            Assert.True(result);
        }

        [Fact]
        public void Count_All_Orders_Stored()
        {
            //Arrange
            var orderRepository = new OrderRepositoryInMemory();
            const long orderTabId = 1;
            
            for (var i = 0; i < 3; i++)
            {
                var order = new Order(i, i, "Teste", "Testando", orderTabId, OrderStatus.AwaitingPreparation);
                orderRepository.Add(order);
            }
            
            //Action
            var count = orderRepository.Count();
            
            //Assert
            Assert.Equal(3, count);
        }

        [Fact]
        public void Remove_A_Order_By_Id()
        {
            //Arrange
            var orderRepository = new OrderRepositoryInMemory();
            var order = new Order(1, 1, "Teste", "Testando", 1, OrderStatus.AwaitingPreparation);
            
            orderRepository.Add(order);
            
            //Action
            orderRepository.RemoveById(order.Id);
            
            //Assert
            Assert.Equal(0, orderRepository.Count());
        }
    }
}