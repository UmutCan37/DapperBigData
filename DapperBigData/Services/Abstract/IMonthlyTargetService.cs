using DapperBigData.Dtos.MonthlyTargetDtos;

namespace DapperBigData.Services.Abstract
{
    public interface IMonthlyTargetService
    {
        Task CreateAsync(CreateMonthlyTargetDto dto);
        Task UpdateAsync(UpdateMonthlyTargetDto dto);
        Task DeleteAsync(int year, int month);
        Task<List<ResultMonthlyTargetDto>> GetAllAsync(int page, int pageSize);
        Task<int> GetCountAsync();
        Task<GetByIdMonthlyTargetDto> GetByIdAsync(int year, int month);

        Task<GetByIdMonthlyTargetDto> GetCurrentMonthTargetAsync();
    }
}
