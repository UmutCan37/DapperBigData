namespace DapperBigData.Services.Abstract
{
    public interface IReportService
    {
        Task<byte[]> GetOrderReportExcelAsync();
        Task<byte[]> GetOrderReportPdfAsync();
        Task<byte[]> GetCustomerReportExcelAsync();
        Task<byte[]> GetCustomerReportPdfAsync();
        Task<byte[]> GetProductReportExcelAsync();
        Task<byte[]> GetProductReportPdfAsync();
        Task<byte[]> GetCategoryReportExcelAsync();
        Task<byte[]> GetCategoryReportPdfAsync();
        Task<byte[]> GetCityReportExcelAsync();
        Task<byte[]> GetCityReportPdfAsync();
        Task<byte[]> GetMonthlyTargetReportExcelAsync();
        Task<byte[]> GetMonthlyTargetReportPdfAsync();
    }
}
