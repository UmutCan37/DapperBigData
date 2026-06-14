using DapperBigData.Dtos.PaymentMethodDtos;

namespace DapperBigData.Services.Abstract
{
    public interface IPaymentMethodService
    {
        Task CreateAsync(CreatePaymentMethodDto dto);
        Task UpdateAsync(UpdatePaymentMethodDto dto);
        Task DeleteAsync(int id);
        Task<List<ResultPaymentMethodDto>> GetAllAsync(int page, int pageSize);
        Task<int> GetCountAsync();
        Task<GetByIdPaymentMethodDto> GetByIdAsync(int id);
    }

}
