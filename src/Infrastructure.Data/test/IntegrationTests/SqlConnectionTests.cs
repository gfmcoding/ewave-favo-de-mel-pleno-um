using System;
using System.Threading.Tasks;
using Dapper;
using Xunit;

namespace FavoDeMel.Infrastructure.Data.IntegrationTests
{
    public class SqlConnectionTests
    {
        private readonly string _connectionString;
        public SqlConnectionTests()
        {
            DotNetEnv.Env.Load("./../../../../../../.env");
            _connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? "";
            
        }
        
        [Fact]
        public async Task Open_A_SqlConnection()
        {
            //Arrange
            var connectionString = new ConnectionString(_connectionString);
            await using var connection = new SqlConnection(connectionString);
            
            //Action
            //Assert
            await connection.OpenAsync();
        }
        
        [Fact]
        public async ValueTask Close_A_SqlConnection()
        {
            //Arrange
            var connectionString = new ConnectionString(_connectionString);
            await using var connection = new SqlConnection(connectionString);
            
            //Action
            //Assert
            await connection.CloseAsync();
            
        }
        
        [Fact]
        public async Task Get_A_QueryFactory()
        {
            //Arrange
            var connectionString = new ConnectionString(_connectionString);
            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            
            //Action
            var queryFactory = connection.GetQueryFactory();
            
            //Assert
            Assert.NotNull(queryFactory);
        }

        [Fact]
        public async Task Execute_A_Query_In_Database()
        {
            //Arrange
            var connectionString = new ConnectionString(_connectionString);
            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();

            var queryFactory = connection.GetQueryFactory();

            //Action
            var result = await queryFactory.Connection.QuerySingleAsync<string>("SELECT version();");

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}