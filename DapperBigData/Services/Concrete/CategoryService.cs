using Dapper;
using DapperBigData.Context;
using DapperBigData.Dtos.CategoryDtos;
using DapperBigData.Services.Abstract;

namespace DapperBigData.Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly DapperContext _context;

        public CategoryService(DapperContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CreateCategoryDto dto)
        {
            string query = "INSERT INTO Categories (CategoryName, IconClass, ColorCode) VALUES(@CategoryName, @IconClass, @ColorCode)";
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryName", dto.CategoryName);
            parameters.Add("@IconClass", dto.IconClass);
            parameters.Add("@ColorCode", dto.ColorCode);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task DeleteAsync(int id)
        {
            string query = "DELETE FROM Categories WHERE CategoryId=@CategoryId";
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryId", id);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<List<ResultCategoryDto>> GetAllAsync(int page, int pageSize)
        {
            string query = @"SELECT c.CategoryId, c.CategoryName, c.IconClass, c.ColorCode,
                        COUNT(p.ProductId) AS ProductCount
                        FROM Categories c
                        LEFT JOIN Products p ON c.CategoryId = p.CategoryId
                        GROUP BY c.CategoryId, c.CategoryName, c.IconClass, c.ColorCode
                        ORDER BY c.CategoryId DESC
                        OFFSET (@Page - 1) * @PageSize ROWS
                        FETCH NEXT @PageSize ROWS ONLY";
            var parameters = new DynamicParameters();
            parameters.Add("@Page", page);
            parameters.Add("@PageSize", pageSize);
            var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<ResultCategoryDto>(query, parameters);
            return result.ToList();
        }

        public async Task<int> GetCountAsync()
        {
            string query = "SELECT COUNT(*) FROM Categories";
            var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query);
        }

        public async Task<GetByIdCategoryDto> GetByIdAsync(int id)
        {
            string query = @"SELECT c.CategoryId, c.CategoryName, c.IconClass, c.ColorCode,
                        COUNT(p.ProductId) AS ProductCount,
                        SUM(CASE WHEN p.IsActive = 1 THEN 1 ELSE 0 END) AS ActiveProductCount,
                        ISNULL(SUM(od.Quantity * od.UnitPrice), 0) AS TotalRevenue
                        FROM Categories c
                        LEFT JOIN Products p ON c.CategoryId = p.CategoryId
                        LEFT JOIN OrderDetails od ON p.ProductId = od.ProductId
                        WHERE c.CategoryId = @CategoryId
                        GROUP BY c.CategoryId, c.CategoryName, c.IconClass, c.ColorCode";
            var parameters = new DynamicParameters();
            parameters.Add("@CategoryId", id);
            var connection = _context.CreateConnection();
            var value= await connection.QueryFirstAsync<GetByIdCategoryDto>(query, parameters);
            return value;
        }

        public Task UpdateAsync(UpdateCategoryDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ResultCategoryDto>> GetTopCategoriesAsync()
        {
            string query = @"SELECT TOP(5)
                    c.CategoryId, c.CategoryName, c.IconClass, c.ColorCode,
                    ISNULL(SUM(od.Quantity), 0) AS TotalSold,
                    ISNULL(SUM(od.Quantity * od.UnitPrice), 0) AS TotalRevenue
                    FROM Categories c
                    LEFT JOIN Products p ON c.CategoryId = p.CategoryId
                    LEFT JOIN OrderDetails od ON p.ProductId = od.ProductId
                    GROUP BY c.CategoryId, c.CategoryName, c.IconClass, c.ColorCode
                    ORDER BY TotalRevenue DESC";
            var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<ResultCategoryDto>(query);
            return result.ToList();
        }
    }
}
