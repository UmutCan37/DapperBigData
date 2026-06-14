using DapperBigData.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace DapperBigData.ViewComponents
{
    public class KpiCardsViewComponent : ViewComponent
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;

        public KpiCardsViewComponent(IOrderService orderService, ICustomerService customerService, IProductService productService)
        {
            _orderService = orderService;
            _customerService = customerService;
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.TotalOrders = await _orderService.GetCountAsync();
            ViewBag.TotalCustomers = await _customerService.GetCountAsync();
            ViewBag.TotalProducts = await _productService.GetCountAsync();
            return View();
        }
    }
}
