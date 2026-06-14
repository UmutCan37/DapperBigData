using Dapper;
using DapperBigData.Context;
using DapperBigData.Dtos.SystemSettingDtos;
using DapperBigData.Services.Abstract;

namespace DapperBigData.Services.Concrete
{
    public class SystemSettingService : ISystemSettingService
    {
        private readonly DapperContext _context;
        public SystemSettingService(DapperContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CreateSystemSettingDto dto)
        {
            string query = "INSERT INTO SystemSettings (SettingKey, SettingValue, IsToggle, Description) VALUES(@SettingKey, @SettingValue, @IsToggle, @Description)";
            var parameters = new DynamicParameters();
            parameters.Add("@SettingKey", dto.SettingKey);
            parameters.Add("@SettingValue", dto.SettingValue);
            parameters.Add("@IsToggle", dto.IsToggle);
            parameters.Add("@Description", dto.Description);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task UpdateAsync(UpdateSystemSettingDto dto)
        {
            string query = "UPDATE SystemSettings SET SettingKey=@SettingKey, SettingValue=@SettingValue, IsToggle=@IsToggle, Description=@Description WHERE SettingId=@SettingId";
            var parameters = new DynamicParameters();
            parameters.Add("@SettingId", dto.SettingId);
            parameters.Add("@SettingKey", dto.SettingKey);
            parameters.Add("@SettingValue", dto.SettingValue);
            parameters.Add("@IsToggle", dto.IsToggle);
            parameters.Add("@Description", dto.Description);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task DeleteAsync(int id)
        {
            string query = "DELETE FROM SystemSettings WHERE SettingId=@SettingId";
            var parameters = new DynamicParameters();
            parameters.Add("@SettingId", id);
            var connection = _context.CreateConnection();
            await connection.ExecuteAsync(query, parameters);
        }

        public async Task<List<ResultSystemSettingDto>> GetAllAsync(int page, int pageSize)
        {
            string query = @"SELECT SettingId, SettingKey, SettingValue, IsToggle, Description
                        FROM SystemSettings
                        ORDER BY SettingId ASC
                        OFFSET (@Page - 1) * @PageSize ROWS
                        FETCH NEXT @PageSize ROWS ONLY";
            var parameters = new DynamicParameters();
            parameters.Add("@Page", page);
            parameters.Add("@PageSize", pageSize);
            var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<ResultSystemSettingDto>(query, parameters);
            return result.ToList();
        }

        public async Task<int> GetCountAsync()
        {
            string query = "SELECT COUNT(*) FROM SystemSettings";
            var connection = _context.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(query);
        }

        public async Task<GetByIdSystemSettingDto> GetByIdAsync(int id)
        {
            string query = "SELECT SettingId, SettingKey, SettingValue, IsToggle, Description FROM SystemSettings WHERE SettingId=@SettingId";
            var parameters = new DynamicParameters();
            parameters.Add("@SettingId", id);
            var connection = _context.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<GetByIdSystemSettingDto>(query, parameters);
        }

        public async Task<GetByIdSystemSettingDto> GetByKeyAsync(string key)
        {
            string query = "SELECT SettingId, SettingKey, SettingValue, IsToggle, Description FROM SystemSettings WHERE SettingKey=@SettingKey";
            var parameters = new DynamicParameters();
            parameters.Add("@SettingKey", key);
            var connection = _context.CreateConnection();
            return await connection.QueryFirstAsync<GetByIdSystemSettingDto>(query, parameters);
        }
    }
}
