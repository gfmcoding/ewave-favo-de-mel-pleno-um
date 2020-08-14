using FavoDeMel.Application.ManageService.Core;
using Xunit;

namespace FavoDeMel.Application.ManageService.UnitTests.Core
{
    public class OrderTests
    {
        [Fact]
        public void Canceling_A_Order_AwaitingPreparation()
        {
            //Arrange
            const int id = 1;
            const int index = 1;
            const string name = "Teste";
            const string description = "Testando order";
            const OrderStatus status = OrderStatus.AwaitingPreparation;
            
            var order = new Order(id, index, name, description, 1, status);
            
            //Action
            var result = order.ToCancel();

            //Assert
            Assert.True(result);
            Assert.Equal(OrderStatus.Canceled, order.Status);
        }

        [Fact]
        public void Put_InPreparation_A_Order_In_AwaitingPreparation()
        {
            //Arrange
            const int id = 1;
            const int index = 1;
            const string name = "Teste";
            const string description = "Testando order";
            const OrderStatus status = OrderStatus.AwaitingPreparation;
            
            var order = new Order(id, index, name, description, 1, status);
            
            //Action
            var result = order.ToInPreparation();

            //Assert
            Assert.True(result);
            Assert.Equal(OrderStatus.InPreparation, order.Status);
        }
        
        [Fact]
        public void Finish_An_Order_In_Preparation()
        {
            //Arrange
            const int id = 1;
            const int index = 1;
            const string name = "Teste";
            const string description = "Testando order";
            const OrderStatus status = OrderStatus.InPreparation;
            
            var order = new Order(id, index, name, description, 1, status);
            
            //Action
            var result = order.ToDone();

            //Assert
            Assert.True(result);
            Assert.Equal(OrderStatus.Done, order.Status);
        }
        
        [Fact]
        public void Delivery_An_Order_Done()
        {
            //Arrange
            const int id = 1;
            const int index = 1;
            const string name = "Teste";
            const string description = "Testando order";
            const OrderStatus status = OrderStatus.Done;
            
            var order = new Order(id, index, name, description, 1, status);
            
            //Action
            var result = order.ToDelivery();

            //Assert
            Assert.True(result);
            Assert.Equal(OrderStatus.Delivered, order.Status);
        }
        
        [Theory]
        [InlineData(OrderStatus.Canceled)]
        [InlineData(OrderStatus.Delivered)]
        [InlineData(OrderStatus.Done)]
        [InlineData(OrderStatus.InPreparation)]
        public void Try_Canceling_A_Order_Dont_Cancelable(OrderStatus status)
        {
            //Arrange
            const int id = 1;
            const int index = 1;
            const string name = "Teste";
            const string description = "Testando order";
            
            var order = new Order(id, index, name, description, 1, status);
            
            //Action
            var result = order.ToCancel();

            //Assert
            Assert.False(result);
            Assert.Equal(status, order.Status);
        }
        
        [Theory]
        [InlineData(OrderStatus.Canceled)]
        [InlineData(OrderStatus.Delivered)]
        [InlineData(OrderStatus.Done)]
        [InlineData(OrderStatus.InPreparation)]
        public void Try_To_InPreparation_A_Order_Dont_Preparable(OrderStatus status)
        {
            //Arrange
            const int id = 1;
            const int index = 1;
            const string name = "Teste";
            const string description = "Testando order";
            
            var order = new Order(id, index, name, description, 1, status);
            
            //Action
            var result = order.ToInPreparation();

            //Assert
            Assert.False(result);
            Assert.Equal(status, order.Status);
        }
        
        [Theory]
        [InlineData(OrderStatus.Canceled)]
        [InlineData(OrderStatus.Delivered)]
        [InlineData(OrderStatus.Done)]
        [InlineData(OrderStatus.AwaitingPreparation)]
        public void Try_Finish_An_Order_In_Not_Preparing(OrderStatus status)
        {
            //Arrange
            const int id = 1;
            const int index = 1;
            const string name = "Teste";
            const string description = "Testando order";
            
            var order = new Order(id, index, name, description, 1, status);
            
            //Action
            var result = order.ToDone();

            //Assert
            Assert.False(result);
            Assert.Equal(status, order.Status);
        }
        
        [Theory]
        [InlineData(OrderStatus.Canceled)]
        [InlineData(OrderStatus.Delivered)]
        [InlineData(OrderStatus.InPreparation)]
        [InlineData(OrderStatus.AwaitingPreparation)]
        public void Try_Delivery_An_Order_Dont_Finish(OrderStatus status)
        {
            //Arrange
            const int id = 1;
            const int index = 1;
            const string name = "Teste";
            const string description = "Testando order";
            
            var order = new Order(id, index, name, description, 1, status);
            
            //Action
            var result = order.ToDelivery();

            //Assert
            Assert.False(result);
            Assert.Equal(status, order.Status);
        }
    }
}