using DapperBigData.Dtos.ProductDtos;

namespace DapperBigData.Services.Abstract
{
    public interface IProductService
    {
        Task CreateAsync(CreateProductDto dto);
        Task UpdateAsync(UpdateProductDto dto);
        Task DeleteAsync(int id);
        Task<List<ResultProductDto>> GetAllAsync(int page, int pageSize);
        Task<int> GetCountAsync();
        Task<GetByIdProductDto> GetByIdAsync(int id);

        Task<List<ResultProductDto>> GetTopProductsAsync();
    }
}
