using DapperBigData.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace DapperBigData.ViewComponents
{
    public class TopProductsViewComponent : ViewComponent
    {
        private readonly IProductService _productService;

        public TopProductsViewComponent(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var values = await _productService.GetTopProductsAsync();
            return View(values);
        }
    }
}
