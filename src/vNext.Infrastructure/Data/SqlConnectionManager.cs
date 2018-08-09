using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using vNext.Core.Interfaces;

namespace vNext.Infrastructure.Data
{
    public class SqlConnectionManager : ISqlConnectionManager
    {
        private readonly IConfiguration _configuration;
        public SqlConnectionManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration["Data:DefaultConnection:ConnectionString"]);
        }
    }
}
