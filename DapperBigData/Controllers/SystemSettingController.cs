using DapperBigData.Dtos.SystemSettingDtos;
using DapperBigData.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace DapperBigData.Controllers
{
    public class SystemSettingController : Controller
    {
        private readonly ISystemSettingService _systemSettingService;

        public SystemSettingController(ISystemSettingService systemSettingService)
        {
            _systemSettingService = systemSettingService;
        }

        public async Task<IActionResult> SystemSettingList(int page = 1)
        {
            int pageSize = 20;
            var values = await _systemSettingService.GetAllAsync(page, pageSize);
            int total = await _systemSettingService.GetCountAsync();
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = total;
            ViewBag.Action = "SystemSettingList";
            return View(values);
        }

        public IActionResult CreateSystemSetting()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateSystemSetting(CreateSystemSettingDto dto)
        {
            await _systemSettingService.CreateAsync(dto);
            return RedirectToAction("SystemSettingList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateSystemSetting(int id)
        {
            var value = await _systemSettingService.GetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSystemSetting(UpdateSystemSettingDto dto)
        {
            await _systemSettingService.UpdateAsync(dto);
            return RedirectToAction("SystemSettingList");
        }

        public async Task<IActionResult> DeleteSystemSetting(int id)
        {
            await _systemSettingService.DeleteAsync(id);
            return RedirectToAction("SystemSettingList");
        }

        public async Task<IActionResult> SystemSettingDetail(int id)
        {
            var value = await _systemSettingService.GetByIdAsync(id);
            return View(value);
        }
    }
}
