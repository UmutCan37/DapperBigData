using DapperBigData.Dtos.PaymentMethodDtos;
using DapperBigData.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace DapperBigData.Controllers
{
    public class PaymentMethodController : Controller
    {
        private readonly IPaymentMethodService _paymentMethodService;

        public PaymentMethodController(IPaymentMethodService paymentMethodService)
        {
            _paymentMethodService = paymentMethodService;
        }

        public async Task<IActionResult> PaymentMethodList(int page = 1)
        {
            int pageSize = 20;
            var values = await _paymentMethodService.GetAllAsync(page, pageSize);
            int total = await _paymentMethodService.GetCountAsync();
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = total;
            ViewBag.Action = "PaymentMethodList";
            return View(values);
        }

        public IActionResult CreatePaymentMethod()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentMethod(CreatePaymentMethodDto dto)
        {
            await _paymentMethodService.CreateAsync(dto);
            return RedirectToAction("PaymentMethodList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdatePaymentMethod(int id)
        {
            var value = await _paymentMethodService.GetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePaymentMethod(UpdatePaymentMethodDto dto)
        {
            await _paymentMethodService.UpdateAsync(dto);
            return RedirectToAction("PaymentMethodList");
        }

        public async Task<IActionResult> DeletePaymentMethod(int id)
        {
            await _paymentMethodService.DeleteAsync(id);
            return RedirectToAction("PaymentMethodList");
        }

        public async Task<IActionResult> PaymentMethodDetail(int id)
        {
            var value = await _paymentMethodService.GetByIdAsync(id);
            return View(value);
        }
    }
}
