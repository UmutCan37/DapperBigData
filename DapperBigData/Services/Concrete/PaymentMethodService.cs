using Dapper;
using DapperBigData.Context;
using DapperBigData.Dtos.PaymentMethodDtos;
using DapperBigData.Services.Abstract;

namespace DapperBigData.Services.Concrete
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly DapperContext _context;
        public PaymentMethodService(DapperContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CreatePaymentMethodDto dto)
        {
            string query = "INSERT INTO PaymentMethods (MethodName, IconClass) VALUES(@MethodName, @IconClass)";
            var parameters = new DynamicParameters();
            parameters.Add("@MethodName", dto.MethodName);
            parameters.Add("@IconClass", dto.IconClass);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task UpdateAsync(UpdatePaymentMethodDto dto)
        {
            string query = "UPDATE PaymentMethods SET MethodName=@MethodName, IconClass=@IconClass WHERE PaymentMethodId=@PaymentMethodId";
            var parameters = new DynamicParameters();
            parameters.Add("@PaymentMethodId", dto.PaymentMethodId);
            parameters.Add("@MethodName", dto.MethodName);
            parameters.Add("@IconClass", dto.IconClass);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task DeleteAsync(int id)
        {
            string query = "DELETE FROM PaymentMethods WHERE PaymentMethodId=@PaymentMethodId";
            var parameters = new DynamicParameters();
            parameters.Add("@PaymentMethodId", id);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<List<ResultPaymentMethodDto>> GetAllAsync(int page, int pageSize)
        {
            string query = @"SELECT pm.PaymentMethodId, pm.MethodName, pm.IconClass,
                        COUNT(o.OrderId) AS TotalOrders,
                        ISNULL(SUM(o.TotalAmount), 0) AS TotalRevenue
                        FROM PaymentMethods pm
                        LEFT JOIN Orders o ON pm.PaymentMethodId = o.PaymentMethodId
                        GROUP BY pm.PaymentMethodId, pm.MethodName, pm.IconClass
                        ORDER BY pm.PaymentMethodId ASC
                        OFFSET (@Page - 1) * @PageSize ROWS
                        FETCH NEXT @PageSize ROWS ONLY";
            var parameters = new DynamicParameters();
            parameters.Add("@Page", page);
            parameters.Add("@PageSize", pageSize);
            var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<ResultPaymentMethodDto>(query, parameters);
            return result.ToList();
        }

        public async Task<int> GetCountAsync()
        {
            string query = "SELECT COUNT(*) FROM PaymentMethods";
            var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query);
        }

        public async Task<GetByIdPaymentMethodDto> GetByIdAsync(int id)
        {
            string query = @"SELECT pm.PaymentMethodId, pm.MethodName, pm.IconClass,
                        COUNT(o.OrderId) AS TotalOrders,
                        ISNULL(SUM(o.TotalAmount), 0) AS TotalRevenue,
                        CAST(COUNT(o.OrderId) * 100.0 / NULLIF((SELECT COUNT(*) FROM Orders), 0) AS DECIMAL(5,2)) AS RevenueShare
                        FROM PaymentMethods pm
                        LEFT JOIN Orders o ON pm.PaymentMethodId = o.PaymentMethodId
                        WHERE pm.PaymentMethodId = @PaymentMethodId
                        GROUP BY pm.PaymentMethodId, pm.MethodName, pm.IconClass";
            var parameters = new DynamicParameters();
            parameters.Add("@PaymentMethodId", id);
            var connection = _context.CreateConnection();
            return await connection.QueryFirstAsync<GetByIdPaymentMethodDto>(query, parameters);
        }
    }
}