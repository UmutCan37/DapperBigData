using DapperBigData.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace DapperBigData.ViewComponents
{
    public class TopCitiesViewComponent : ViewComponent
    {
        private readonly ICityService _cityService;

        public TopCitiesViewComponent(ICityService cityService)
        {
            _cityService = cityService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _cityService.GetTopCitiesAsync();
            return View(values);
        }
    }
}
