using DapperBigData.Dtos.OrderDtos;

namespace DapperBigData.Services.Abstract
{
    public interface IOrderService
    {
        Task CreateAsync(CreateOrderDto dto);
        Task UpdateAsync(UpdateOrderDto dto);
        Task DeleteAsync(int id);
        Task<List<ResultOrderDto>> GetAllAsync(int page, int pageSize);
        Task<int> GetCountAsync();
        Task<GetByIdOrderDto> GetByIdAsync(int id);

        Task<List<ResultOrderDto>> GetRecentOrdersAsync();

        Task<decimal> GetTotalRevenueAsync();
    }
}
