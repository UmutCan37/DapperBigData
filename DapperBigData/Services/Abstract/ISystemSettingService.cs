using DapperBigData.Dtos.SystemSettingDtos;

namespace DapperBigData.Services.Abstract
{
    public interface ISystemSettingService
    {
        Task CreateAsync(CreateSystemSettingDto dto);
        Task UpdateAsync(UpdateSystemSettingDto dto);
        Task DeleteAsync(int id);
        Task<List<ResultSystemSettingDto>> GetAllAsync(int page, int pageSize);
        Task<int> GetCountAsync();
        Task<GetByIdSystemSettingDto> GetByIdAsync(int id);
        Task<GetByIdSystemSettingDto> GetByKeyAsync(string key);
    }
}
