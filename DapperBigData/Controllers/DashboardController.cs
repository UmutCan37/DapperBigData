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
            var categoriesTask = _categoryService.GetTopCategoriesAsync();
            var totalRevenueTask = _orderService.GetTotalRevenueAsync();
            var totalOrdersTask = _orderService.GetCountAsync();
            var totalCustomersTask = _customerService.GetCountAsync();
            var totalProductsTask = _productService.GetCountAsync();
            var paymentTask = _paymentMethodService.GetAllAsync(1, 100);
            var revenue2025Task = _orderService.GetMonthlyRevenueAsync(2025);
            var revenue2026Task = _orderService.GetMonthlyRevenueAsync(2026);

            await Task.WhenAll(
                categoriesTask, totalRevenueTask, totalOrdersTask,
                totalCustomersTask, totalProductsTask, paymentTask,
                revenue2025Task, revenue2026Task);

            var categories = categoriesTask.Result;
            ViewBag.CategoryLabels = JsonSerializer.Serialize(categories.Select(x => x.CategoryName).ToList());
            ViewBag.CategoryData = JsonSerializer.Serialize(categories.Select(x => (double)x.TotalRevenue).ToList());

            ViewBag.TotalRevenue = totalRevenueTask.Result;
            ViewBag.TotalOrders = totalOrdersTask.Result;
            ViewBag.TotalCustomers = totalCustomersTask.Result;
            ViewBag.TotalProducts = totalProductsTask.Result;

            var payments = paymentTask.Result;
            ViewBag.PaymentLabels = JsonSerializer.Serialize(payments.Select(x => x.MethodName).ToList());
            ViewBag.PaymentData = JsonSerializer.Serialize(payments.Select(x => x.TotalOrders).ToList());

            
            var rev2025 = new decimal[12];
            var rev2026 = new decimal[12];

            foreach (var item in revenue2025Task.Result)
                rev2025[item.MonthNumber - 1] = item.Revenue / 1000;

            foreach (var item in revenue2026Task.Result)
                rev2026[item.MonthNumber - 1] = item.Revenue / 1000;

            ViewBag.Revenue2025 = JsonSerializer.Serialize(rev2025);
            ViewBag.Revenue2026 = JsonSerializer.Serialize(rev2026);

            return View();
        }
    }
    }

