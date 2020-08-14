using System.Collections.Generic;
using FavoDeMel.Application.ManageService.Core;
using FavoDeMel.Infrastructure.Common.IdGenerator;
using Moq;
using Xunit;

namespace FavoDeMel.Application.ManageService.UnitTests.Core
{
    public class OrdersManagerTests
    {
        private IIdGenerator _idGenerator;
        
        public OrdersManagerTests()
        {
            var idGeneratorMock = new Mock<IIdGenerator>();
            idGeneratorMock
                .Setup(m => m.Generate())
                .Returns(10);
            _idGenerator = idGeneratorMock.Object;
        }

        [Fact]
        public void Create_A_New_OrdersManager()
        {
            //Arrange
            var orderRepositoryMock = new Mock<IOrderRepository>();
            orderRepositoryMock
                .Setup(m => m.Count())
                .Returns(0);
            orderRepositoryMock
                .Setup(m => m.GetAll())
                .Returns(new SortedList<int, Order>());
            
            //Action
            var ordersManager = new OrdersManager(_idGenerator, orderRepositoryMock.Object);
            
            //Assert
            Assert.NotNull(ordersManager);
            Assert.NotNull(ordersManager.GetOrders());
            Assert.Equal(0, ordersManager.Count);
        }

        [Fact]
        public void New_Order()
        {
            //Arrange
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var returnOrder = new Order(1, 1, "Teste", "Testando", 1, OrderStatus.AwaitingPreparation);
            orderRepositoryMock
                .Setup(m => m.Add(returnOrder));
            orderRepositoryMock
                .Setup(m => m.Count())
                .Returns(1);
            orderRepositoryMock
                .Setup(m => m.GetAll())
                .Returns(new SortedList<int, Order>
                {
                    {1, returnOrder}
                });
            
            var ordersManager = new OrdersManager(_idGenerator, orderRepositoryMock.Object);
            
            //Action
            var order = ordersManager.NewOrder("Teste", "Testando");
            
            //Assert
            Assert.NotNull(order);
            Assert.True(ordersManager.Count == 1);
            Assert.True(ordersManager.GetOrders().Count == 1);
        }

        [Fact]
        public void Get_Orders()
        {
            //Arrange
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var returnOrder = new Order(1, 1, "Teste", "Testando", 1, OrderStatus.AwaitingPreparation);
            orderRepositoryMock
                .Setup(m => m.Add(returnOrder));
            orderRepositoryMock
                .Setup(m => m.Count())
                .Returns(3);
            orderRepositoryMock
                .Setup(m => m.GetAll())
                .Returns(new SortedList<int, Order>
                {
                    {1, returnOrder},
                    {2, returnOrder},
                    {3, returnOrder}
                });
            
            var ordersManager = new OrdersManager(_idGenerator, orderRepositoryMock.Object);
            _ = ordersManager.NewOrder("Teste", "Testando");
            _ = ordersManager.NewOrder("Teste", "Testando");
            _ = ordersManager.NewOrder("Teste", "Testando");
            
            //Action
            var orders = ordersManager.GetOrders();

            //Assert
            Assert.NotNull(orders);
            Assert.Equal(3, ordersManager.Count);
            Assert.Equal(3, ordersManager.GetOrders().Count);
        }

        [Fact]
        public void Deprecate_A_Order()
        {
            //Arrange
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var returnOrderOne = new Order(1, 1, "Teste1", "Testando1", 1, OrderStatus.AwaitingPreparation);
            var returnOrderTwo = new Order(1, 2, "Teste2", "Testando2", 1, OrderStatus.AwaitingPreparation);
            var returnOrderTree = new Order(1, 3, "Teste3", "Testando3", 1, OrderStatus.AwaitingPreparation);
            orderRepositoryMock
                .Setup(m => m.Add(returnOrderOne));
            orderRepositoryMock
                .Setup(m => m.Count())
                .Returns(3);
            orderRepositoryMock
                .Setup(m => m.GetAll())
                .Returns(new SortedList<int, Order>
                {
                    {1, returnOrderOne},
                    {2, returnOrderTwo},
                    {3, returnOrderTree}
                });
            
            var ordersManager = new OrdersManager(_idGenerator, orderRepositoryMock.Object);
            _ = ordersManager.NewOrder("Teste1", "Testando1");
            _ = ordersManager.NewOrder("Teste2", "Testando2");
            _ = ordersManager.NewOrder("Teste3", "Testando3");
            
            const int actualPosition = 1;
            const int newPosition = 3;
            
            //Action
            var result = ordersManager.ReprioritizeOrder(actualPosition, newPosition);
            
            //Assert
            Assert.True(result);

            var actualOrders = ordersManager.GetOrders();
            Assert.Equal(1, actualOrders[actualPosition].Index);
            Assert.Equal("Teste3", actualOrders[actualPosition].Name);
            
            Assert.Equal(3, actualOrders[newPosition].Index);
            Assert.Equal("Teste1", actualOrders[newPosition].Name);
        }
        
        [Fact]
        public void Priorize_A_Order()
        {
            //Arrange
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var returnOrderOne = new Order(1, 3, "Teste3", "Testando3", 1, OrderStatus.AwaitingPreparation);
            var returnOrderTwo = new Order(1,2, "Teste2", "Testando2", 1, OrderStatus.AwaitingPreparation);
            var returnOrderTree = new Order(1, 1, "Teste1", "Testando1", 1, OrderStatus.AwaitingPreparation);
            orderRepositoryMock
                .Setup(m => m.Add(returnOrderOne));
            orderRepositoryMock
                .Setup(m => m.Count())
                .Returns(3);
            orderRepositoryMock
                .Setup(m => m.GetAll())
                .Returns(new SortedList<int, Order>
                {
                    {1, returnOrderOne},
                    {2, returnOrderTwo},
                    {3, returnOrderTree}
                });
            
            var ordersManager = new OrdersManager(_idGenerator, orderRepositoryMock.Object);
            _ = ordersManager.NewOrder("Teste3", "Testando3");
            _ = ordersManager.NewOrder("Teste2", "Testando2");
            _ = ordersManager.NewOrder("Teste1", "Testando1");
            
            const int actualPosition = 1;
            const int newPosition = 3;
            
            //Action
            var result = ordersManager.ReprioritizeOrder(actualPosition, newPosition);
            
            //Assert
            Assert.True(result);

            var actualOrders = ordersManager.GetOrders();
            Assert.Equal(1, actualOrders[actualPosition].Index);
            Assert.Equal("Teste1", actualOrders[actualPosition].Name);

            Assert.Equal(3, actualOrders[newPosition].Index);
            Assert.Equal("Teste3", actualOrders[newPosition].Name);
        }

        [Fact]
        public void Remove_A_Order_By_Position()
        {
            //Arrange
            var orderRepositoryMock = new Mock<IOrderRepository>();
            var returnOrderOne = new Order(1, 1, "Teste1", "Testando1", 1, OrderStatus.AwaitingPreparation);
            var returnOrderTwo = new Order(2, 2, "Teste2", "Testando2", 1, OrderStatus.AwaitingPreparation);
            var returnOrderTree = new Order(3, 3, "Teste3", "Testando3", 1, OrderStatus.AwaitingPreparation);
            var returnOrderFour = new Order(4, 4, "Teste4", "Testando4", 1, OrderStatus.AwaitingPreparation);
            orderRepositoryMock
                .Setup(m => m.Add(returnOrderOne));
            orderRepositoryMock
                .Setup(m => m.GetAll())
                .Returns(new SortedList<int, Order>
                {
                    {1, returnOrderOne},
                    {2, returnOrderTwo},
                    {3, returnOrderTree},
                    {4, returnOrderFour}
                });

            const int position = 2;
            
            var ordersManager = new OrdersManager(_idGenerator, orderRepositoryMock.Object);
            _ = ordersManager.NewOrder("Teste1", "Testando1");
            _ = ordersManager.NewOrder("Teste2", "Testando2");
            _ = ordersManager.NewOrder("Teste3", "Testando3");
            
            //Action
            ordersManager.Remove(position);
            
            //Assert
            var orders = ordersManager.GetOrders();
            Assert.Equal(3, orders.Count);
            
            Assert.Equal(2, orders[position].Index);
            Assert.Equal("Teste3", orders[position].Name);
            
            Assert.Equal(3, orders[position + 1].Index);
            Assert.Equal("Teste4", orders[position + 1].Name);
        }
    }
}