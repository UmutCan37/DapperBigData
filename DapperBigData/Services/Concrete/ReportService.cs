using ClosedXML.Excel;
using DapperBigData.Services.Abstract;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Drawing;
using System.Xml.Linq;

namespace DapperBigData.Services.Concrete
{
    public class ReportService : IReportService
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly ICityService _cityService;
        private readonly IMonthlyTargetService _monthlyTargetService;

        public ReportService(
            IOrderService orderService,
            ICustomerService customerService,
            IProductService productService,
            ICategoryService categoryService,
            ICityService cityService,
            IMonthlyTargetService monthlyTargetService)
        {
            _orderService = orderService;
            _customerService = customerService;
            _productService = productService;
            _categoryService = categoryService;
            _cityService = cityService;
            _monthlyTargetService = monthlyTargetService;
        }

        // ── HELPERS ──
        private static readonly BaseColor HeaderColor = new BaseColor(99, 102, 241);
        private static readonly BaseColor RowEvenColor = new BaseColor(248, 250, 252);
        private static readonly BaseColor TextColor = new BaseColor(30, 41, 59);
        private static readonly BaseColor BorderColor = new BaseColor(226, 232, 240);

        private byte[] CreatePdf(string title, string[] headers, List<string[]> rows)
        {
            using var ms = new MemoryStream();
            var doc = new Document(PageSize.A4.Rotate(), 20f, 20f, 30f, 20f);
            PdfWriter.GetInstance(doc, ms);
            doc.Open();

            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16, TextColor);
            var subFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, new BaseColor(100, 116, 139));
            var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, BaseColor.WHITE);
            var dataFont = FontFactory.GetFont(FontFactory.HELVETICA, 8, TextColor);

            doc.Add(new Paragraph(title, titleFont) { SpacingAfter = 4f });
            doc.Add(new Paragraph($"Oluşturulma tarihi: {DateTime.Now:dd.MM.yyyy HH:mm}", subFont) { SpacingAfter = 14f });

            var table = new PdfPTable(headers.Length) { WidthPercentage = 100 };

            foreach (var header in headers)
            {
                table.AddCell(new PdfPCell(new Phrase(header, headerFont))
                {
                    BackgroundColor = HeaderColor,
                    Padding = 8f,
                    BorderColor = HeaderColor,
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
            }

            for (int i = 0; i < rows.Count; i++)
            {
                var bgColor = i % 2 == 0 ? RowEvenColor : BaseColor.WHITE;
                foreach (var cell in rows[i])
                {
                    table.AddCell(new PdfPCell(new Phrase(cell, dataFont))
                    {
                        BackgroundColor = bgColor,
                        Padding = 6f,
                        BorderColor = BorderColor
                    });
                }
            }

            doc.Add(table);
            doc.Close();
            return ms.ToArray();
        }

        private XLWorkbook CreateExcel(string sheetName, string[] headers, List<string[]> rows)
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add(sheetName);

            for (int i = 0; i < headers.Length; i++)
                ws.Cell(1, i + 1).Value = headers[i];

            var headerRange = ws.Range(1, 1, 1, headers.Length);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#6366F1");
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

            for (int i = 0; i < rows.Count; i++)
            {
                for (int j = 0; j < rows[i].Length; j++)
                    ws.Cell(i + 2, j + 1).Value = rows[i][j];

                if (i % 2 == 0)
                    ws.Row(i + 2).Style.Fill.BackgroundColor = XLColor.FromHtml("#F8FAFC");
            }

            ws.Columns().AdjustToContents();
            ws.Range(1, 1, rows.Count + 1, headers.Length).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.Range(1, 1, rows.Count + 1, headers.Length).Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            return wb;
        }

        // ── ORDER ──
        public async Task<byte[]> GetOrderReportExcelAsync()
        {
            var orders = await _orderService.GetAllAsync(1, 10000);
            var headers = new[] { "Sipariş ID", "Sipariş No", "Müşteri", "Şehir", "Ödeme", "Tarih", "Tutar", "Durum" };
            var rows = orders.Select(o => new[]
            {
            o.OrderId.ToString(),
            o.OrderNumber,
            o.CustomerFullName,
            o.CityName,
            o.PaymentMethod,
            o.OrderDate.ToString("dd.MM.yyyy"),
            $"{o.TotalAmount:N2}",
            o.OrderStatus == "Completed" ? "Tamamlandı" : o.OrderStatus == "Pending" ? "Beklemede" : "İptal"
        }).ToList();

            using var wb = CreateExcel("Siparişler", headers, rows);
            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }

        public async Task<byte[]> GetOrderReportPdfAsync()
        {
            var orders = await _orderService.GetAllAsync(1, 10000);
            var headers = new[] { "ID", "Sipariş No", "Müşteri", "Şehir", "Ödeme", "Tarih", "Tutar", "Durum" };
            var rows = orders.Select(o => new[]
            {
            o.OrderId.ToString(),
            o.OrderNumber,
            o.CustomerFullName,
            o.CityName,
            o.PaymentMethod,
            o.OrderDate.ToString("dd.MM.yyyy"),
            $"{o.TotalAmount:N2} ₺",
            o.OrderStatus == "Completed" ? "Tamamlandı" : o.OrderStatus == "Pending" ? "Beklemede" : "İptal"
        }).ToList();
            return CreatePdf("Sipariş Raporu", headers, rows);
        }

        // ── CUSTOMER ──
        public async Task<byte[]> GetCustomerReportExcelAsync()
        {
            var customers = await _customerService.GetAllAsync(1, 10000);
            var headers = new[] { "Müşteri ID", "Ad Soyad", "E-posta", "Telefon", "Şehir", "Kayıt Tarihi", "Durum" };
            var rows = customers.Select(c => new[]
            {
            c.CustomerId.ToString(),
            c.FullName,
            c.Email,
            c.Phone,
            c.CityName,
            c.CreatedAt.ToString("dd.MM.yyyy"),
            c.IsActive ? "Aktif" : "Pasif"
        }).ToList();

            using var wb = CreateExcel("Müşteriler", headers, rows);
            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }

        public async Task<byte[]> GetCustomerReportPdfAsync()
        {
            var customers = await _customerService.GetAllAsync(1, 10000);
            var headers = new[] { "ID", "Ad Soyad", "E-posta", "Telefon", "Şehir", "Kayıt Tarihi", "Durum" };
            var rows = customers.Select(c => new[]
            {
            c.CustomerId.ToString(),
            c.FullName,
            c.Email,
            c.Phone,
            c.CityName,
            c.CreatedAt.ToString("dd.MM.yyyy"),
            c.IsActive ? "Aktif" : "Pasif"
        }).ToList();
            return CreatePdf("Müşteri Raporu", headers, rows);
        }

        // ── PRODUCT ──
        public async Task<byte[]> GetProductReportExcelAsync()
        {
            var products = await _productService.GetAllAsync(1, 10000);
            var headers = new[] { "Ürün ID", "Ürün Adı", "SKU", "Kategori", "Birim Fiyat", "Stok", "Durum", "Tarih" };
            var rows = products.Select(p => new[]
            {
            p.ProductId.ToString(),
            p.ProductName,
            p.Sku,
            p.CategoryName,
            $"{p.UnitPrice:N2}",
            p.StockQuantity.ToString(),
            p.IsActive ? "Aktif" : "Pasif",
            p.CreatedAt.ToString("dd.MM.yyyy")
        }).ToList();

            using var wb = CreateExcel("Ürünler", headers, rows);
            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }

        public async Task<byte[]> GetProductReportPdfAsync()
        {
            var products = await _productService.GetAllAsync(1, 10000);
            var headers = new[] { "ID", "Ürün Adı", "SKU", "Kategori", "Fiyat", "Stok", "Durum", "Tarih" };
            var rows = products.Select(p => new[]
            {
            p.ProductId.ToString(),
            p.ProductName,
            p.Sku,
            p.CategoryName,
            $"{p.UnitPrice:N2} ₺",
            p.StockQuantity.ToString(),
            p.IsActive ? "Aktif" : "Pasif",
            p.CreatedAt.ToString("dd.MM.yyyy")
        }).ToList();
            return CreatePdf("Ürün Raporu", headers, rows);
        }

        // ── CATEGORY ──
        public async Task<byte[]> GetCategoryReportExcelAsync()
        {
            var categories = await _categoryService.GetAllAsync(1, 100);
            var headers = new[] { "Kategori ID", "Kategori Adı", "İkon", "Renk", "Ürün Sayısı" };
            var rows = categories.Select(c => new[]
            {
            c.CategoryId.ToString(),
            c.CategoryName,
            c.IconClass,
            c.ColorCode,
            c.ProductCount.ToString()
        }).ToList();

            using var wb = CreateExcel("Kategoriler", headers, rows);
            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }

        public async Task<byte[]> GetCategoryReportPdfAsync()
        {
            var categories = await _categoryService.GetAllAsync(1, 100);
            var headers = new[] { "ID", "Kategori Adı", "İkon", "Renk", "Ürün Sayısı" };
            var rows = categories.Select(c => new[]
            {
            c.CategoryId.ToString(),
            c.CategoryName,
            c.IconClass,
            c.ColorCode,
            c.ProductCount.ToString()
        }).ToList();
            return CreatePdf("Kategori Raporu", headers, rows);
        }

        // ── CITY ──
        public async Task<byte[]> GetCityReportExcelAsync()
        {
            var cities = await _cityService.GetAllAsync(1, 100);
            var headers = new[] { "Şehir ID", "Şehir Adı", "Plaka", "Toplam Sipariş", "Toplam Ciro" };
            var rows = cities.Select(c => new[]
            {
            c.CityId.ToString(),
            c.CityName,
            c.PlateCode.ToString(),
            c.TotalOrders.ToString("N0"),
            $"{c.TotalRevenue:N2}"
        }).ToList();

            using var wb = CreateExcel("Şehirler", headers, rows);
            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }

        public async Task<byte[]> GetCityReportPdfAsync()
        {
            var cities = await _cityService.GetAllAsync(1, 100);
            var headers = new[] { "ID", "Şehir Adı", "Plaka", "Toplam Sipariş", "Toplam Ciro" };
            var rows = cities.Select(c => new[]
            {
            c.CityId.ToString(),
            c.CityName,
            c.PlateCode.ToString(),
            c.TotalOrders.ToString("N0"),
            $"{c.TotalRevenue:N2} ₺"
        }).ToList();
            return CreatePdf("Şehir Raporu", headers, rows);
        }

        // ── MONTHLY TARGET ──
        public async Task<byte[]> GetMonthlyTargetReportExcelAsync()
        {
            var targets = await _monthlyTargetService.GetAllAsync(1, 100);
            string[] months = { "", "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık" };
            var headers = new[] { "Yıl", "Ay", "Ciro Hedefi", "Sipariş Hedefi" };
            var rows = targets.Select(t => new[]
            {
            t.TargetYear.ToString(),
            months[t.TargetMonth],
            $"{t.RevenueTarget:N2}",
            t.OrderTarget.ToString("N0")
        }).ToList();

            using var wb = CreateExcel("Aylık Hedefler", headers, rows);
            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            return ms.ToArray();
        }

        public async Task<byte[]> GetMonthlyTargetReportPdfAsync()
        {
            var targets = await _monthlyTargetService.GetAllAsync(1, 100);
            string[] months = { "", "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık" };
            var headers = new[] { "Yıl", "Ay", "Ciro Hedefi", "Sipariş Hedefi" };
            var rows = targets.Select(t => new[]
            {
            t.TargetYear.ToString(),
            months[t.TargetMonth],
            $"{t.RevenueTarget:N2} ₺",
            t.OrderTarget.ToString("N0")
        }).ToList();
            return CreatePdf("Aylık Hedef Raporu", headers, rows);
        }
    }
}
