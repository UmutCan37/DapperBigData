using DapperBigData.Dtos.CityDtos;

namespace DapperBigData.Services.Abstract
{
    public interface ICityService
    {
        Task CreateAsync(CreateCityDto dto);
        Task UpdateAsync(UpdateCityDto dto);
        Task DeleteAsync(int id);
        Task<List<ResultCityDto>> GetAllAsync(int page, int pageSize);
        Task<int> GetCountAsync();
        Task<GetByIdCityDto> GetByIdAsync(int id);

        Task<List<ResultCityDto>> GetTopCitiesAsync();
    }
}
