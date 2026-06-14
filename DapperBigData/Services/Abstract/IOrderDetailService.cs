using DapperBigData.Dtos.OrderDetailDtos;

namespace DapperBigData.Services.Abstract
{
    public interface IOrderDetailService
    {
        Task CreateAsync(CreateOrderDetailDto dto);
        Task UpdateAsync(UpdateOrderDetailDto dto);
        Task DeleteAsync(int id);
        Task<List<ResultOrderDetailDto>> GetAllAsync(int page, int pageSize);
        Task<int> GetCountAsync();
        Task<GetByIdOrderDetailDto> GetByIdAsync(int id);
        Task<List<ResultOrderDetailDto>> GetByOrderIdAsync(int orderId);
    }
}
