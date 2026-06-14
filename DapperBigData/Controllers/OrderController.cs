using DapperBigData.Dtos.OrderDtos;
using DapperBigData.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace DapperBigData.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IPaymentMethodService _paymentMethodService;

        public OrderController(IOrderService orderService, ICustomerService customerService, IPaymentMethodService paymentMethodService)
        {
            _orderService = orderService;
            _customerService = customerService;
            _paymentMethodService = paymentMethodService;
        }

        public async Task<IActionResult> OrderList(int page = 1)
        {
            int pageSize = 20;
            var values = await _orderService.GetAllAsync(page, pageSize);
            int total = await _orderService.GetCountAsync();
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = total;
            ViewBag.Action = "OrderList";
            return View(values);
        }

        public async Task<IActionResult> CreateOrder()
        {
            ViewBag.Customers = await _customerService.GetAllAsync(1, 100);
            ViewBag.PaymentMethods = await _paymentMethodService.GetAllAsync(1, 100);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
        {
            await _orderService.CreateAsync(dto);
            return RedirectToAction("OrderList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateOrder(int id)
        {
            ViewBag.Customers = await _customerService.GetAllAsync(1, 100);
            ViewBag.PaymentMethods = await _paymentMethodService.GetAllAsync(1, 100);
            var value = await _orderService.GetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(UpdateOrderDto dto)
        {
            await _orderService.UpdateAsync(dto);
            return RedirectToAction("OrderList");
        }

        public async Task<IActionResult> DeleteOrder(int id)
        {
            await _orderService.DeleteAsync(id);
            return RedirectToAction("OrderList");
        }

        public async Task<IActionResult> OrderDetail(int id)
        {
            var value = await _orderService.GetByIdAsync(id);
            return View(value);
        }
    }
}
