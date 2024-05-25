using Npgsql;
namespace awsapi.Services
{
    public interface IDbService
    {
        Task<NpgsqlConnection> GetConnectionAsync();
    }

    public class DbService : IDbService
    {
        private readonly IConfiguration _config;
        public DbService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<NpgsqlConnection> GetConnectionAsync()
        {
            var connection = new NpgsqlConnection(Environment.GetEnvironmentVariable("DB_CONNECTION2"));
            await connection.OpenAsync();
            return connection;
        }
    }
}