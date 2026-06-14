using DapperBigData.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace DapperBigData.ViewComponents
{
    public class PaymentMethodsViewComponent : ViewComponent
    {
        private readonly IPaymentMethodService _paymentMethodService;

        public PaymentMethodsViewComponent(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _paymentMethodService.GetAllAsync(1, 100);
            return View(values);
        }
    }
}
