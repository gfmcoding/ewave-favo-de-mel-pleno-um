using FavoDeMel.Application.ManageService.Core;
using FavoDeMel.Application.ManageService.Infrastructure.Repositories;
using Xunit;

namespace FavoDeMel.Application.ManageService.UnitTests.Infrastructure
{
    public class OrderTabRepositoryInMemoryTests
    {
        [Fact]
        public void Add_A_New_OrderTab()
        {
            //Arrange
            var repository = new OrderTabRepositoryInMemory();

            const long id = 1;
            var orderTab = new OrderTab(id, 1);
            
            //Action
            repository.Add(orderTab);
            
            //Assert
            var expectedOrderTabs = repository.GetAll();
            Assert.NotNull(expectedOrderTabs);
            Assert.Single(expectedOrderTabs);
        }
        
        [Fact]
        public void Get_A_OrderTab_By_Id()
        {
            //Arrange
            var repository = new OrderTabRepositoryInMemory();

            const long id = 1;
            var orderTab = new OrderTab(id, 1);
            repository.Add(orderTab);
            
            //Action
            var expectedOrderTab = repository.GetById(id);
            
            //Assert
            Assert.NotNull(expectedOrderTab);
            Assert.Equal(expectedOrderTab, orderTab);
        }
        
        [Fact]
        public void Get_All_OrderTab()
        {
            //Arrange
            var repository = new OrderTabRepositoryInMemory();

            for (var i = 0; i < 3; i++)
            {
                var orderTab = new OrderTab(i, i);
                repository.Add(orderTab);
            }
            
            //Action
            var expectedOrderTabs = repository.GetAll();
            
            //Assert
            Assert.NotNull(expectedOrderTabs);
            Assert.Equal(3, expectedOrderTabs.Count);
        }
        
        [Fact]
        public void Remove_A_OrderTab_By_Id()
        {
            //Arrange
            var repository = new OrderTabRepositoryInMemory();

            for (var i = 0; i < 3; i++)
            {
                var orderTab = new OrderTab(i, i);
                repository.Add(orderTab);
            }
            
            //Action
            repository.RemoveById(2);
            
            //Assert
            var expectedOrderTabs = repository.GetAll();
            Assert.Equal(2, expectedOrderTabs.Count);
        }
    }
}