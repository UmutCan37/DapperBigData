using Dapper;
using DapperBigData.Context;
using DapperBigData.Dtos.ProductDtos;
using DapperBigData.Services.Abstract;

namespace DapperBigData.Services.Concrete
{
    public class ProductService : IProductService
    {
        private readonly DapperContext _context;

        public ProductService(DapperContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CreateProductDto dto)
        {
            string query = "INSERT INTO Products (CategoryId, ProductName, Sku, UnitPrice, StockQuantity, CreatedAt, IsActive) VALUES(@CategoryId, @ProductName, @Sku, @UnitPrice, @StockQuantity, @CreatedAt, @IsActive)";
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryId", dto.CategoryId);
            parameters.Add("@ProductName", dto.ProductName);
            parameters.Add("@Sku", dto.Sku);
            parameters.Add("@UnitPrice", dto.UnitPrice);
            parameters.Add("@StockQuantity", dto.StockQuantity);
            parameters.Add("@CreatedAt", DateTime.UtcNow);
            parameters.Add("@IsActive", true);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task DeleteAsync(int id)
        {
            string query = "DELETE FROM Products WHERE ProductId=@ProductId";
            var parameters = new DynamicParameters();
            parameters.Add("@ProductId", id);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<List<ResultProductDto>> GetAllAsync(int page, int pageSize)
        {
            string query = @"SELECT p.ProductId, p.CategoryId, p.ProductName, p.Sku, p.UnitPrice,
                        p.StockQuantity, p.CreatedAt, p.IsActive,
                        c.CategoryName, c.ColorCode AS CategoryColor
                        FROM Products p
                        JOIN Categories c ON p.CategoryId = c.CategoryId
                        ORDER BY p.ProductId DESC
                        OFFSET (@Page - 1) * @PageSize ROWS
                        FETCH NEXT @PageSize ROWS ONLY";
            var parameters = new DynamicParameters();
            parameters.Add("@Page", page);
            parameters.Add("@PageSize", pageSize);
            var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<ResultProductDto>(query, parameters);
            return result.ToList();
        }

        public async Task<int> GetCountAsync()
        {
            string query = "SELECT COUNT(*) FROM Products";
            var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query);
        }

        public async Task<GetByIdProductDto> GetByIdAsync(int id)
        {
            string query = @"SELECT p.ProductId, p.CategoryId, p.ProductName, p.Sku, p.UnitPrice,
                        p.StockQuantity, p.CreatedAt, p.IsActive,
                        c.CategoryName, c.ColorCode AS CategoryColor,
                        ISNULL(SUM(od.Quantity), 0) AS TotalSold,
                        ISNULL(SUM(od.Quantity * od.UnitPrice), 0) AS TotalRevenue
                        FROM Products p
                        JOIN Categories c ON p.CategoryId = c.CategoryId
                        LEFT JOIN OrderDetails od ON p.ProductId = od.ProductId
                        WHERE p.ProductId = @ProductId
                        GROUP BY p.ProductId, p.CategoryId, p.ProductName, p.Sku, p.UnitPrice,
                        p.StockQuantity, p.CreatedAt, p.IsActive, c.CategoryName, c.ColorCode";
            var parameters = new DynamicParameters();
            parameters.Add("@ProductId", id);
            var connection = _context.CreateConnection();
            var value= await connection.QueryFirstOrDefaultAsync<GetByIdProductDto>(query, parameters);
            return value;
        }

        public async Task UpdateAsync(UpdateProductDto dto)
        {
            string query = "UPDATE Products SET CategoryId=@CategoryId, ProductName=@ProductName, Sku=@Sku, UnitPrice=@UnitPrice, StockQuantity=@StockQuantity, IsActive=@IsActive WHERE ProductId=@ProductId";
            var parameters = new DynamicParameters();
            parameters.Add("@ProductId", dto.ProductId);
            parameters.Add("@CategoryId", dto.CategoryId);
            parameters.Add("@ProductName", dto.ProductName);
            parameters.Add("@Sku", dto.Sku);
            parameters.Add("@UnitPrice", dto.UnitPrice);
            parameters.Add("@StockQuantity", dto.StockQuantity);
            parameters.Add("@IsActive", dto.IsActive);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<List<ResultProductDto>> GetTopProductsAsync()
        {
            string query = @"SELECT TOP(5)
                    p.ProductId, p.CategoryId, p.ProductName, p.Sku, p.UnitPrice,
                    p.StockQuantity, p.CreatedAt, p.IsActive,
                    c.CategoryName, c.ColorCode AS CategoryColor,
                    ISNULL(SUM(od.Quantity), 0) AS TotalSold,
                    ISNULL(SUM(od.Quantity * od.UnitPrice), 0) AS TotalRevenue
                    FROM Products p
                    JOIN Categories c ON p.CategoryId = c.CategoryId
                    LEFT JOIN OrderDetails od ON p.ProductId = od.ProductId
                    GROUP BY p.ProductId, p.CategoryId, p.ProductName, p.Sku, p.UnitPrice,
                    p.StockQuantity, p.CreatedAt, p.IsActive, c.CategoryName, c.ColorCode
                    ORDER BY TotalRevenue DESC";
            var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<ResultProductDto>(query);
            return result.ToList();
        }
    }
}
