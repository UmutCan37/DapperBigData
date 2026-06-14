using DapperBigData.Dtos.MonthlyTargetDtos;
using DapperBigData.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace DapperBigData.Controllers
{
    public class MonthlyTargetController : Controller
    {
        private readonly IMonthlyTargetService _monthlyTargetService;

        public MonthlyTargetController(IMonthlyTargetService monthlyTargetService)
        {
            _monthlyTargetService = monthlyTargetService;
        }

        public async Task<IActionResult> MonthlyTargetList(int page = 1)
        {
            int pageSize = 20;
            var values = await _monthlyTargetService.GetAllAsync(page, pageSize);
            int total = await _monthlyTargetService.GetCountAsync();
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = total;
            ViewBag.Action = "MonthlyTargetList";
            return View(values);
        }

        public IActionResult CreateMonthlyTarget()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMonthlyTarget(CreateMonthlyTargetDto dto)
        {
            await _monthlyTargetService.CreateAsync(dto);
            return RedirectToAction("MonthlyTargetList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateMonthlyTarget(int year, int month)
        {
            var value = await _monthlyTargetService.GetByIdAsync(year, month);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMonthlyTarget(UpdateMonthlyTargetDto dto)
        {
            await _monthlyTargetService.UpdateAsync(dto);
            return RedirectToAction("MonthlyTargetList");
        }

        public async Task<IActionResult> DeleteMonthlyTarget(int year, int month)
        {
            await _monthlyTargetService.DeleteAsync(year, month);
            return RedirectToAction("MonthlyTargetList");
        }

        public async Task<IActionResult> MonthlyTargetDetail(int year, int month)
        {
            var value = await _monthlyTargetService.GetByIdAsync(year, month);
            return View(value);
        }
    }
}
