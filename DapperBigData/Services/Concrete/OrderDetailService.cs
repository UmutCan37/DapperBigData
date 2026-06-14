using Dapper;
using DapperBigData.Context;
using DapperBigData.Dtos.OrderDetailDtos;
using DapperBigData.Services.Abstract;

namespace DapperBigData.Services.Concrete
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly DapperContext _context;

        public OrderDetailService(DapperContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CreateOrderDetailDto dto)
        {
            string query = "INSERT INTO OrderDetails (OrderId, ProductId, Quantity, UnitPrice) VALUES(@OrderId, @ProductId, @Quantity, @UnitPrice)";
            var parameters = new DynamicParameters();
            parameters.Add("@OrderId", dto.OrderId);
            parameters.Add("@ProductId", dto.ProductId);
            parameters.Add("@Quantity", dto.Quantity);
            parameters.Add("@UnitPrice", dto.UnitPrice);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task DeleteAsync(int id)
        {
            string query = "DELETE FROM OrderDetails WHERE OrderDetailId=@OrderDetailId";
            var parameters = new DynamicParameters();
            parameters.Add("@OrderDetailId", id);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<List<ResultOrderDetailDto>> GetAllAsync(int page, int pageSize)
        {
            string query = @"SELECT od.OrderDetailId, od.OrderId, od.ProductId,
                        od.Quantity, od.UnitPrice,
                        p.ProductName, p.Sku, c.CategoryName
                        FROM OrderDetails od
                        JOIN Products p ON od.ProductId = p.ProductId
                        JOIN Categories c ON p.CategoryId = c.CategoryId
                        ORDER BY od.OrderDetailId DESC
                        OFFSET (@Page - 1) * @PageSize ROWS
                        FETCH NEXT @PageSize ROWS ONLY";
            var parameters = new DynamicParameters();
            parameters.Add("@Page", page);
            parameters.Add("@PageSize", pageSize);
            var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<ResultOrderDetailDto>(query, parameters);
            return result.ToList();
        }

        public async Task<int> GetCountAsync()
        {
            string query = "SELECT COUNT(*) FROM OrderDetails";
            var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query);
        }

        public async Task<GetByIdOrderDetailDto> GetByIdAsync(int id)
        {
            string query = @"SELECT od.OrderDetailId, od.OrderId, od.ProductId,
                        od.Quantity, od.UnitPrice,
                        p.ProductName, p.Sku, c.CategoryName, c.ColorCode AS CategoryColor
                        FROM OrderDetails od
                        JOIN Products p ON od.ProductId = p.ProductId
                        JOIN Categories c ON p.CategoryId = c.CategoryId
                        WHERE od.OrderDetailId = @OrderDetailId";
            var parameters = new DynamicParameters();
            parameters.Add("@OrderDetailId", id);
            var connection = _context.CreateConnection();
            return await connection.QueryFirstAsync<GetByIdOrderDetailDto>(query, parameters);
        }

        public async Task<List<ResultOrderDetailDto>> GetByOrderIdAsync(int orderId)
        {
            string query = @"SELECT od.OrderDetailId, od.OrderId, od.ProductId,
                        od.Quantity, od.UnitPrice,
                        p.ProductName, p.Sku, c.CategoryName
                        FROM OrderDetails od
                        JOIN Products p ON od.ProductId = p.ProductId
                        JOIN Categories c ON p.CategoryId = c.CategoryId
                        WHERE od.OrderId = @OrderId";
            var parameters = new DynamicParameters();
            parameters.Add("@OrderId", orderId);
            var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<ResultOrderDetailDto>(query, parameters);
            return result.ToList();
        }

        public async Task UpdateAsync(UpdateOrderDetailDto dto)
        {
            string query = "UPDATE OrderDetails SET OrderId=@OrderId, ProductId=@ProductId, Quantity=@Quantity, UnitPrice=@UnitPrice WHERE OrderDetailId=@OrderDetailId";
            var parameters = new DynamicParameters();
            parameters.Add("@OrderDetailId", dto.OrderDetailId);
            parameters.Add("@OrderId", dto.OrderId);
            parameters.Add("@ProductId", dto.ProductId);
            parameters.Add("@Quantity", dto.Quantity);
            parameters.Add("@UnitPrice", dto.UnitPrice);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }
    }
}
