using DapperBigData.Dtos.CategoryDtos;

namespace DapperBigData.Services.Abstract
{
    public interface ICategoryService
    {
        Task CreateAsync(CreateCategoryDto dto);
        Task UpdateAsync(UpdateCategoryDto dto);
        Task DeleteAsync(int id);
        Task<List<ResultCategoryDto>> GetAllAsync(int page, int pageSize);
        Task<int> GetCountAsync();
        Task<GetByIdCategoryDto> GetByIdAsync(int id);

        Task<List<ResultCategoryDto>> GetTopCategoriesAsync();
    }
}
