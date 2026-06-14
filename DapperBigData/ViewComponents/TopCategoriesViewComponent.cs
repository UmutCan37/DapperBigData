using DapperBigData.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace DapperBigData.ViewComponents
{
    public class TopCategoriesViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public TopCategoriesViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _categoryService.GetTopCategoriesAsync();
            return View(values);
        }
    }
}
