using Dapper;
using DapperBigData.Context;
using DapperBigData.Dtos.MonthlyTargetDtos;
using DapperBigData.Services.Abstract;

namespace DapperBigData.Services.Concrete
{
    public class MonthlyTargetService : IMonthlyTargetService
    {
        private readonly DapperContext _context;
        public MonthlyTargetService(DapperContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CreateMonthlyTargetDto dto)
        {
            string query = "INSERT INTO MonthlyTargets (TargetYear, TargetMonth, RevenueTarget, OrderTarget) VALUES(@TargetYear, @TargetMonth, @RevenueTarget, @OrderTarget)";
            var parameters = new DynamicParameters();
            parameters.Add("@TargetYear", dto.TargetYear);
            parameters.Add("@TargetMonth", dto.TargetMonth);
            parameters.Add("@RevenueTarget", dto.RevenueTarget);
            parameters.Add("@OrderTarget", dto.OrderTarget);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task UpdateAsync(UpdateMonthlyTargetDto dto)
        {
            string query = "UPDATE MonthlyTargets SET RevenueTarget=@RevenueTarget, OrderTarget=@OrderTarget WHERE TargetYear=@TargetYear AND TargetMonth=@TargetMonth";
            var parameters = new DynamicParameters();
            parameters.Add("@TargetYear", dto.TargetYear);
            parameters.Add("@TargetMonth", dto.TargetMonth);
            parameters.Add("@RevenueTarget", dto.RevenueTarget);
            parameters.Add("@OrderTarget", dto.OrderTarget);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task DeleteAsync(int year, int month)
        {
            string query = "DELETE FROM MonthlyTargets WHERE TargetYear=@TargetYear AND TargetMonth=@TargetMonth";
            var parameters = new DynamicParameters();
            parameters.Add("@TargetYear", year);
            parameters.Add("@TargetMonth", month);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<List<ResultMonthlyTargetDto>> GetAllAsync(int page, int pageSize)
        {
            string query = @"SELECT TargetYear, TargetMonth, RevenueTarget, OrderTarget
                        FROM MonthlyTargets
                        ORDER BY TargetYear DESC, TargetMonth DESC
                        OFFSET (@Page - 1) * @PageSize ROWS
                        FETCH NEXT @PageSize ROWS ONLY";
            var parameters = new DynamicParameters();
            parameters.Add("@Page", page);
            parameters.Add("@PageSize", pageSize);
            var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<ResultMonthlyTargetDto>(query, parameters);
            return result.ToList();
        }

        public async Task<int> GetCountAsync()
        {
            string query = "SELECT COUNT(*) FROM MonthlyTargets";
            var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query);
        }

        public async Task<GetByIdMonthlyTargetDto> GetByIdAsync(int year, int month)
        {
            string query = @"SELECT mt.TargetYear, mt.TargetMonth, mt.RevenueTarget, mt.OrderTarget,
                        ISNULL(SUM(o.TotalAmount), 0) AS ActualRevenue,
                        COUNT(o.OrderId) AS ActualOrders,
                        CAST(ISNULL(SUM(o.TotalAmount), 0) * 100.0 / NULLIF(mt.RevenueTarget, 0) AS DECIMAL(5,2)) AS RevenueAchievementPct,
                        CAST(COUNT(o.OrderId) * 100.0 / NULLIF(mt.OrderTarget, 0) AS DECIMAL(5,2)) AS OrderAchievementPct
                        FROM MonthlyTargets mt
                        LEFT JOIN Orders o ON YEAR(o.OrderDate) = mt.TargetYear AND MONTH(o.OrderDate) = mt.TargetMonth
                        WHERE mt.TargetYear = @TargetYear AND mt.TargetMonth = @TargetMonth
                        GROUP BY mt.TargetYear, mt.TargetMonth, mt.RevenueTarget, mt.OrderTarget";
            var parameters = new DynamicParameters();
            parameters.Add("@TargetYear", year);
            parameters.Add("@TargetMonth", month);
            var connection = _context.CreateConnection();
            return await connection.QueryFirstAsync<GetByIdMonthlyTargetDto>(query, parameters);
        }

        public async Task<GetByIdMonthlyTargetDto> GetCurrentMonthTargetAsync()
        {
            string query = @"SELECT mt.TargetYear, mt.TargetMonth, mt.RevenueTarget, mt.OrderTarget,
                    ISNULL(SUM(o.TotalAmount), 0) AS ActualRevenue,
                    COUNT(o.OrderId) AS ActualOrders,
                    CAST(ISNULL(SUM(o.TotalAmount), 0) * 100.0 / NULLIF(mt.RevenueTarget, 0) AS DECIMAL(5,2)) AS RevenueAchievementPct,
                    CAST(COUNT(o.OrderId) * 100.0 / NULLIF(mt.OrderTarget, 0) AS DECIMAL(5,2)) AS OrderAchievementPct
                    FROM MonthlyTargets mt
                    LEFT JOIN Orders o ON YEAR(o.OrderDate) = mt.TargetYear AND MONTH(o.OrderDate) = mt.TargetMonth
                    WHERE mt.TargetYear = YEAR(GETDATE()) AND mt.TargetMonth = MONTH(GETDATE())
                    GROUP BY mt.TargetYear, mt.TargetMonth, mt.RevenueTarget, mt.OrderTarget";
            var connection = _context.CreateConnection();
            var value = await connection.QueryFirstAsync<GetByIdMonthlyTargetDto>(query);
            return value;
        }
    }
}
