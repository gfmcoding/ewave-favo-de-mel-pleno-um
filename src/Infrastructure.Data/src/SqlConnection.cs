using System;
using System.Threading.Tasks;
using Npgsql;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace FavoDeMel.Infrastructure.Data
{
    public class SqlConnection : IAsyncDisposable
    {
        private readonly NpgsqlConnection _connection;
        private readonly PostgresCompiler _compiler;

        public SqlConnection(ConnectionString connectionString)
        {
            _connection = new NpgsqlConnection(connectionString.Value);
            _compiler = new PostgresCompiler();
        }

        public async ValueTask OpenAsync() => await _connection.OpenAsync();

        public async ValueTask CloseAsync() => await _connection.CloseAsync();

        public QueryFactory GetQueryFactory() => new QueryFactory(_connection, _compiler);
        
        public async ValueTask DisposeAsync() => await _connection.DisposeAsync();
    }
}