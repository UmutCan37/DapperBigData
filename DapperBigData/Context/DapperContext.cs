using Microsoft.Data.SqlClient;
using System.Data;

namespace DapperBigData.Context
{
    public class DapperContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DapperContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("connectionKey");
        }
        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);


    }
}
