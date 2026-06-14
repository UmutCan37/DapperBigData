using Dapper;
using DapperBigData.Context;
using DapperBigData.Dtos.OrderDetailDtos;
using DapperBigData.Dtos.OrderDtos;
using DapperBigData.Services.Abstract;

namespace DapperBigData.Services.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly DapperContext _context;

        public OrderService(DapperContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CreateOrderDto dto)
        {
            string query = "INSERT INTO Orders (CustomerId, PaymentMethodId, OrderNumber, OrderDate, TotalAmount, OrderStatus) VALUES(@CustomerId, @PaymentMethodId, @OrderNumber, @OrderDate, @TotalAmount, @OrderStatus)";
            var parameters = new DynamicParameters();
            parameters.Add("@CustomerId", dto.CustomerId);
            parameters.Add("@PaymentMethodId", dto.PaymentMethodId);
            parameters.Add("@OrderNumber", dto.OrderNumber);
            parameters.Add("@OrderDate", dto.OrderDate);
            parameters.Add("@TotalAmount", dto.TotalAmount);
            parameters.Add("@OrderStatus", dto.OrderStatus);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task DeleteAsync(int id)
        {
            string query = "DELETE FROM Orders WHERE OrderId=@OrderId";
            var parameters = new DynamicParameters();
            parameters.Add("@OrderId", id);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<List<ResultOrderDto>> GetAllAsync(int page, int pageSize)
        {
            string query = @"SELECT o.OrderId, o.CustomerId, o.PaymentMethodId, o.OrderNumber,
                        o.OrderDate, o.TotalAmount, o.OrderStatus,
                        c.FirstName + ' ' + c.LastName AS CustomerFullName,
                        pm.MethodName AS PaymentMethod,
                        pm.IconClass AS PaymentIconClass,
                        ci.CityName
                        FROM Orders o
                        JOIN Customers c ON o.CustomerId = c.CustomerId
                        JOIN PaymentMethods pm ON o.PaymentMethodId = pm.PaymentMethodId
                        JOIN Cities ci ON c.CityId = ci.CityId
                        ORDER BY o.OrderId DESC
                        OFFSET (@Page - 1) * @PageSize ROWS
                        FETCH NEXT @PageSize ROWS ONLY";
            var parameters = new DynamicParameters();
            parameters.Add("@Page", page);
            parameters.Add("@PageSize", pageSize);
            var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<ResultOrderDto>(query, parameters);
            return result.ToList();
        }

        public async Task<int> GetCountAsync()
        {
            string query = "SELECT COUNT(*) FROM Orders";
            var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query);
        }

        public async Task<GetByIdOrderDto> GetByIdAsync(int id)
        {
            string query = @"SELECT o.OrderId, o.CustomerId, o.PaymentMethodId, o.OrderNumber,
                    o.OrderDate, o.TotalAmount, o.OrderStatus,
                    c.FirstName + ' ' + c.LastName AS CustomerFullName,
                    c.Email AS CustomerEmail, c.Phone AS CustomerPhone,
                    pm.MethodName AS PaymentMethod,
                    pm.IconClass AS PaymentIconClass,
                    ci.CityName
                    FROM Orders o
                    JOIN Customers c ON o.CustomerId = c.CustomerId
                    JOIN PaymentMethods pm ON o.PaymentMethodId = pm.PaymentMethodId
                    JOIN Cities ci ON c.CityId = ci.CityId
                    WHERE o.OrderId = @OrderId";
            var parameters = new DynamicParameters();
            parameters.Add("@OrderId", id);
            var connection = _context.CreateConnection();
            var value = await connection.QueryFirstAsync<GetByIdOrderDto>(query, parameters);

            string detailQuery = @"SELECT od.OrderDetailId, od.OrderId, od.ProductId,
                          od.Quantity, od.UnitPrice,
                          p.ProductName, p.Sku, c.CategoryName
                          FROM OrderDetails od
                          JOIN Products p ON od.ProductId = p.ProductId
                          JOIN Categories c ON p.CategoryId = c.CategoryId
                          WHERE od.OrderId = @OrderId";
            var detailParameters = new DynamicParameters();
            detailParameters.Add("@OrderId", id);
            var details = await connection.QueryAsync<ResultOrderDetailDto>(detailQuery, detailParameters);
            value.OrderDetails = details.ToList();

            return value;
        }

        public async Task UpdateAsync(UpdateOrderDto dto)
        {
            string query = "UPDATE Orders SET CustomerId=@CustomerId, PaymentMethodId=@PaymentMethodId, OrderNumber=@OrderNumber, OrderDate=@OrderDate, TotalAmount=@TotalAmount, OrderStatus=@OrderStatus WHERE OrderId=@OrderId";
            var parameters = new DynamicParameters();
            parameters.Add("@OrderId", dto.OrderId);
            parameters.Add("@CustomerId", dto.CustomerId);
            parameters.Add("@PaymentMethodId", dto.PaymentMethodId);
            parameters.Add("@OrderNumber", dto.OrderNumber);
            parameters.Add("@OrderDate", dto.OrderDate);
            parameters.Add("@TotalAmount", dto.TotalAmount);
            parameters.Add("@OrderStatus", dto.OrderStatus);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<List<ResultOrderDto>> GetRecentOrdersAsync()
        {
            string query = @"SELECT TOP(10)
                    o.OrderId, o.CustomerId, o.PaymentMethodId, o.OrderNumber,
                    o.OrderDate, o.TotalAmount, o.OrderStatus,
                    c.FirstName + ' ' + c.LastName AS CustomerFullName,
                    pm.MethodName AS PaymentMethod,
                    pm.IconClass AS PaymentIconClass,
                    ci.CityName
                    FROM Orders o
                    JOIN Customers c ON o.CustomerId = c.CustomerId
                    JOIN PaymentMethods pm ON o.PaymentMethodId = pm.PaymentMethodId
                    JOIN Cities ci ON c.CityId = ci.CityId
                    ORDER BY o.OrderDate DESC";
            var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<ResultOrderDto>(query);
            return result.ToList();
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            string query = "SELECT ISNULL(SUM(TotalAmount), 0) FROM Orders";
            var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<decimal>(query);
        }
    }
}
