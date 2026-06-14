using DapperBigData.Dtos.CustomerDtos;
using DapperBigData.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace DapperBigData.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ICityService _cityService;

        public CustomerController(ICustomerService customerService, ICityService cityService)
        {
            _customerService = customerService;
            _cityService = cityService;
        }

        public async Task<IActionResult> CustomerList(int page = 1)
        {
            int pageSize = 20;
            var values = await _customerService.GetAllAsync(page, pageSize);
            int total = await _customerService.GetCountAsync();
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = total;
            ViewBag.Action = "CustomerList";
            return View(values);
        }

        public async Task<IActionResult> CreateCustomer()
        {
            ViewBag.Cities = await _cityService.GetAllAsync(1, 100);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDto dto)
        {
            await _customerService.CreateAsync(dto);
            return RedirectToAction("CustomerList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCustomer(int id)
        {
            ViewBag.Cities = await _cityService.GetAllAsync(1, 100);
            var value = await _customerService.GetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCustomer(UpdateCustomerDto dto)
        {
            await _customerService.UpdateAsync(dto);
            return RedirectToAction("CustomerList");
        }

        public async Task<IActionResult> DeleteCustomer(int id)
        {
            await _customerService.DeleteAsync(id);
            return RedirectToAction("CustomerList");
        }

        public async Task<IActionResult> CustomerDetail(int id)
        {
            var value = await _customerService.GetByIdAsync(id);
            return View(value);
        }
    }
}
