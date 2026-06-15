using DapperBigData.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace DapperBigData.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        public async Task<IActionResult> OrderExcel()
        {
            var data = await _reportService.GetOrderReportExcelAsync();
            return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Siparis_Raporu_{DateTime.Now:yyyyMMdd}.xlsx");
        }

        public async Task<IActionResult> OrderPdf()
        {
            var data = await _reportService.GetOrderReportPdfAsync();
            return File(data, "application/pdf", $"Siparis_Raporu_{DateTime.Now:yyyyMMdd}.pdf");
        }

        public async Task<IActionResult> CustomerExcel()
        {
            var data = await _reportService.GetCustomerReportExcelAsync();
            return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Musteri_Raporu_{DateTime.Now:yyyyMMdd}.xlsx");
        }

        public async Task<IActionResult> CustomerPdf()
        {
            var data = await _reportService.GetCustomerReportPdfAsync();
            return File(data, "application/pdf", $"Musteri_Raporu_{DateTime.Now:yyyyMMdd}.pdf");
        }

        public async Task<IActionResult> ProductExcel()
        {
            var data = await _reportService.GetProductReportExcelAsync();
            return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Urun_Raporu_{DateTime.Now:yyyyMMdd}.xlsx");
        }

        public async Task<IActionResult> ProductPdf()
        {
            var data = await _reportService.GetProductReportPdfAsync();
            return File(data, "application/pdf", $"Urun_Raporu_{DateTime.Now:yyyyMMdd}.pdf");
        }

        public async Task<IActionResult> CategoryExcel()
        {
            var data = await _reportService.GetCategoryReportExcelAsync();
            return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Kategori_Raporu_{DateTime.Now:yyyyMMdd}.xlsx");
        }

        public async Task<IActionResult> CategoryPdf()
        {
            var data = await _reportService.GetCategoryReportPdfAsync();
            return File(data, "application/pdf", $"Kategori_Raporu_{DateTime.Now:yyyyMMdd}.pdf");
        }

        public async Task<IActionResult> CityExcel()
        {
            var data = await _reportService.GetCityReportExcelAsync();
            return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Sehir_Raporu_{DateTime.Now:yyyyMMdd}.xlsx");
        }

        public async Task<IActionResult> CityPdf()
        {
            var data = await _reportService.GetCityReportPdfAsync();
            return File(data, "application/pdf", $"Sehir_Raporu_{DateTime.Now:yyyyMMdd}.pdf");
        }

        public async Task<IActionResult> MonthlyTargetExcel()
        {
            var data = await _reportService.GetMonthlyTargetReportExcelAsync();
            return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Hedef_Raporu_{DateTime.Now:yyyyMMdd}.xlsx");
        }

        public async Task<IActionResult> MonthlyTargetPdf()
        {
            var data = await _reportService.GetMonthlyTargetReportPdfAsync();
            return File(data, "application/pdf", $"Hedef_Raporu_{DateTime.Now:yyyyMMdd}.pdf");
        }
    }
}
