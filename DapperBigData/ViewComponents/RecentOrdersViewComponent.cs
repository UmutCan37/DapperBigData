using DapperBigData.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace DapperBigData.ViewComponents
{
    public class RecentOrdersViewComponent : ViewComponent
    {
        private readonly IOrderService _orderService;

        public RecentOrdersViewComponent(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _orderService.GetRecentOrdersAsync();
            return View(values);
        }
    }
}
