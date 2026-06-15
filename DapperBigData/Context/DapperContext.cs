using Microsoft.Data.SqlClient;
using System.Data;
using static Org.BouncyCastle.Math.EC.ECCurve;

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
    public SqlConnection CreateConnection() =>
    new SqlConnection(_configuration.GetConnectionString("connectionKey") + ";Command Timeout=120");


    }
}
