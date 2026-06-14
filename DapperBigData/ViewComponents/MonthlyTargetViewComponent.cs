using DapperBigData.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace DapperBigData.ViewComponents
{
    public class MonthlyTargetViewComponent : ViewComponent
    {
        private readonly IMonthlyTargetService _monthlyTargetService;

        public MonthlyTargetViewComponent(IMonthlyTargetService monthlyTargetService)
        {
            _monthlyTargetService = monthlyTargetService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var value = await _monthlyTargetService.GetCurrentMonthTargetAsync();
            return View(value);
        }
    }
}
