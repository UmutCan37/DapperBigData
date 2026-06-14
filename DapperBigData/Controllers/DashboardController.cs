using DapperBigData.Services.Abstract;
using DapperBigData.Services.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DapperBigData.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        private readonly IPaymentMethodService _paymentMethodService;

        public DashboardController(
            ICategoryService categoryService,
            IOrderService orderService,
            ICustomerService customerService,
            IProductService productService,
            IPaymentMethodService paymentMethodService)
        {
            _categoryService = categoryService;
            _orderService = orderService;
            _customerService = customerService;
            _productService = productService;
            _paymentMethodService = paymentMethodService;
        }

        public async Task<IActionResult> Index()
        {
            // KPI
            ViewBag.TotalRevenue = await _orderService.GetTotalRevenueAsync();
            ViewBag.TotalOrders = await _orderService.GetCountAsync();
            ViewBag.TotalCustomers = await _customerService.GetCountAsync();

            // Category Donut Chart
            var categories = await _categoryService.GetTopCategoriesAsync();
            ViewBag.CategoryLabels = JsonSerializer.Serialize(categories.Select(x => x.CategoryName).ToList());
            ViewBag.CategoryData = JsonSerializer.Serialize(categories.Select(x => (double)x.TotalRevenue).ToList());

            // Payment Chart
            var payments = await _paymentMethodService.GetAllAsync(1, 100);
            ViewBag.PaymentLabels = JsonSerializer.Serialize(payments.Select(x => x.MethodName).ToList());
            ViewBag.PaymentData = JsonSerializer.Serialize(payments.Select(x => x.TotalOrders).ToList());

            return View();
        }
    }
}
