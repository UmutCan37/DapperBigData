using DapperBigData.Dtos.CustomerDtos;

namespace DapperBigData.Services.Abstract
{
    public interface ICustomerService
    {
        Task CreateAsync(CreateCustomerDto dto);
        Task UpdateAsync(UpdateCustomerDto dto);
        Task DeleteAsync(int id);
        Task<List<ResultCustomerDto>> GetAllAsync(int page, int pageSize);
        Task<int> GetCountAsync();
        Task<GetByIdCustomerDto> GetByIdAsync(int id);
    }
}
