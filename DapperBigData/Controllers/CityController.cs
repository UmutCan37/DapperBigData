using DapperBigData.Dtos.CityDtos;
using DapperBigData.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace DapperBigData.Controllers
{
    public class CityController : Controller
    {
        private readonly ICityService _cityService;

        public CityController(ICityService cityService)
        {
            _cityService = cityService;
        }

        public async Task<IActionResult> CityList(int page = 1)
        {
            int pageSize = 20;
            var values = await _cityService.GetAllAsync(page, pageSize);
            int total = await _cityService.GetCountAsync();
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = total;
            ViewBag.Action = "CityList";
            return View(values);
        }

        public IActionResult CreateCity()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCity(CreateCityDto dto)
        {
            await _cityService.CreateAsync(dto);
            return RedirectToAction("CityList");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCity(int id)
        {
            var value = await _cityService.GetByIdAsync(id);
            return View(value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCity(UpdateCityDto dto)
        {
            await _cityService.UpdateAsync(dto);
            return RedirectToAction("CityList");
        }

        public async Task<IActionResult> DeleteCity(int id)
        {
            await _cityService.DeleteAsync(id);
            return RedirectToAction("CityList");
        }

        public async Task<IActionResult> CityDetail(int id)
        {
            var value = await _cityService.GetByIdAsync(id);
            return View(value);
        }
    }
}
