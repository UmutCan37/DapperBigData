using Dapper;
using DapperBigData.Context;
using DapperBigData.Dtos.CityDtos;
using DapperBigData.Services.Abstract;

namespace DapperBigData.Services.Concrete
{
    public class CityService : ICityService
    {
        private readonly DapperContext _context;
        public CityService(DapperContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CreateCityDto dto)
        {
            string query = "INSERT INTO Cities (CityName, PlateCode) VALUES(@CityName, @PlateCode)";
            var parameters = new DynamicParameters();
            parameters.Add("@CityName", dto.CityName);
            parameters.Add("@PlateCode", dto.PlateCode);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task UpdateAsync(UpdateCityDto dto)
        {
            string query = "UPDATE Cities SET CityName=@CityName, PlateCode=@PlateCode WHERE CityId=@CityId";
            var parameters = new DynamicParameters();
            parameters.Add("@CityId", dto.CityId);
            parameters.Add("@CityName", dto.CityName);
            parameters.Add("@PlateCode", dto.PlateCode);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task DeleteAsync(int id)
        {
            string query = "DELETE FROM Cities WHERE CityId=@CityId";
            var parameters = new DynamicParameters();
            parameters.Add("@CityId", id);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<List<ResultCityDto>> GetAllAsync(int page, int pageSize)
        {
            string query = @"SELECT ci.CityId, ci.CityName, ci.PlateCode,
                        COUNT(o.OrderId) AS TotalOrders,
                        ISNULL(SUM(o.TotalAmount), 0) AS TotalRevenue
                        FROM Cities ci
                        LEFT JOIN Customers c ON ci.CityId = c.CityId
                        LEFT JOIN Orders o ON c.CustomerId = o.CustomerId
                        GROUP BY ci.CityId, ci.CityName, ci.PlateCode
                        ORDER BY ci.CityId ASC
                        OFFSET (@Page - 1) * @PageSize ROWS
                        FETCH NEXT @PageSize ROWS ONLY";
            var parameters = new DynamicParameters();
            parameters.Add("@Page", page);
            parameters.Add("@PageSize", pageSize);
            var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<ResultCityDto>(query, parameters);
            return result.ToList();
        }

        public async Task<int> GetCountAsync()
        {
            string query = "SELECT COUNT(*) FROM Cities";
            var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query);
        }

        public async Task<GetByIdCityDto> GetByIdAsync(int id)
        {
            string query = @"SELECT ci.CityId, ci.CityName, ci.PlateCode,
                        COUNT(DISTINCT o.OrderId) AS TotalOrders,
                        ISNULL(SUM(o.TotalAmount), 0) AS TotalRevenue,
                        COUNT(DISTINCT c.CustomerId) AS TotalCustomers
                        FROM Cities ci
                        LEFT JOIN Customers c ON ci.CityId = c.CityId
                        LEFT JOIN Orders o ON c.CustomerId = o.CustomerId
                        WHERE ci.CityId = @CityId
                        GROUP BY ci.CityId, ci.CityName, ci.PlateCode";
            var parameters = new DynamicParameters();
            parameters.Add("@CityId", id);
            var connection = _context.CreateConnection();
            return await connection.QueryFirstAsync<GetByIdCityDto>(query, parameters);
        }

        public async Task<List<ResultCityDto>> GetTopCitiesAsync()
        {
            string query = @"SELECT TOP(8)
                    ci.CityId, ci.CityName, ci.PlateCode,
                    COUNT(o.OrderId) AS TotalOrders,
                    ISNULL(SUM(o.TotalAmount), 0) AS TotalRevenue,
                    CAST(ISNULL(SUM(o.TotalAmount), 0) * 100.0 / NULLIF((SELECT SUM(TotalAmount) FROM Orders), 0) AS DECIMAL(5,2)) AS RevenueShare
                    FROM Cities ci
                    LEFT JOIN Customers c ON ci.CityId = c.CityId
                    LEFT JOIN Orders o ON c.CustomerId = o.CustomerId
                    GROUP BY ci.CityId, ci.CityName, ci.PlateCode
                    ORDER BY TotalRevenue DESC";
            var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<ResultCityDto>(query);
            return result.ToList();
        }
    }
}
