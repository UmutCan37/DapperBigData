using Dapper;
using DapperBigData.Context;
using DapperBigData.Dtos.CategoryDtos;
using DapperBigData.Dtos.CustomerDtos;
using DapperBigData.Services.Abstract;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Numerics;

namespace DapperBigData.Services.Concrete
{
    public class CustomerService : ICustomerService
    {
        private readonly DapperContext _context;

        public CustomerService(DapperContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CreateCustomerDto dto)
        {
            string query= "INSERT INTO Customers (CityId, FirstName, LastName, Email, Phone, CreatedAt, IsActive) VALUES(@CityId, @FirstName, @LastName, @Email, @Phone, @CreatedAt, @IsActive)";
            var parameters=new DynamicParameters();
            parameters.Add("@CityId", dto.CityId);
            parameters.Add("@FirstName", dto.FirstName);
            parameters.Add("@LastName", dto.LastName);
            parameters.Add("@Email", dto.Email);
            parameters.Add("@Phone", dto.Phone);
            parameters.Add("@CreatedAt", DateTime.UtcNow);
            parameters.Add("@IsActive", true);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task DeleteAsync(int id)
        {
            string query = "DELETE FROM Customers WHERE CustomerId = @CustomerId";
            var parameters = new DynamicParameters();
            parameters.Add("@CustomerId", id);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<List<ResultCustomerDto>> GetAllAsync(int page, int pageSize)
        {
            string query = @"SELECT c.CustomerId, c.CityId, c.FirstName, c.LastName, c.Email,
                        c.Phone, c.CreatedAt, c.IsActive, ci.CityName
                        FROM Customers c
                        JOIN Cities ci ON c.CityId = ci.CityId
                        ORDER BY c.CustomerId DESC
                        OFFSET (@Page - 1) * @PageSize ROWS
                        FETCH NEXT @PageSize ROWS ONLY";
            var parameters = new DynamicParameters();
            parameters.Add("@Page", page);
            parameters.Add("@PageSize", pageSize);
            var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<ResultCustomerDto>(query, parameters);
            return result.ToList();
        }

        public async Task<int> GetCountAsync()
        {
            string query = "SELECT COUNT(*) FROM Customers";
            var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query);
        }

        public async Task<GetByIdCustomerDto> GetByIdAsync(int id)
        {
            string query = @"SELECT c.CustomerId, c.CityId, c.FirstName, c.LastName, c.Email,
                        c.Phone, c.CreatedAt, c.IsActive, ci.CityName, ci.PlateCode,
                        COUNT(o.OrderId) AS TotalOrders,
                        ISNULL(SUM(o.TotalAmount), 0) AS TotalSpent
                        FROM Customers c
                        JOIN Cities ci ON c.CityId = ci.CityId
                        LEFT JOIN Orders o ON c.CustomerId = o.CustomerId
                        WHERE c.CustomerId = @CustomerId
                        GROUP BY c.CustomerId, c.CityId, c.FirstName, c.LastName, c.Email,
                        c.Phone, c.CreatedAt, c.IsActive, ci.CityName, ci.PlateCode";
            var parameters = new DynamicParameters();
            parameters.Add("CustomerId", id);
            var connection = _context.CreateConnection();
            var value= await connection.QueryFirstAsync<GetByIdCustomerDto>(query, parameters);
            return value;
        }

        public async Task UpdateAsync(UpdateCustomerDto dto)
        {
            string query = "UPDATE Customers SET CityId=@CityId, FirstName=@FirstName, LastName=@LastName, Email=@Email, Phone=@Phone, IsActive=@IsActive WHERE CustomerId=@CustomerId";
            var parameters = new DynamicParameters();
            parameters.Add("CustomerId", dto.CustomerId);
            parameters.Add("CityId", dto.CityId);
            parameters.Add("FirstName", dto.FirstName);
            parameters.Add("LastName", dto.LastName);
            parameters.Add("Email", dto.Email);
            parameters.Add("Phone", dto.Phone);
            parameters.Add("IsActive", dto.IsActive);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }
    }
}
