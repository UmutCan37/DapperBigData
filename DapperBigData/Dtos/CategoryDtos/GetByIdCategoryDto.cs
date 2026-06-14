namespace DapperBigData.Dtos.CategoryDtos
{
    public class GetByIdCategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string IconClass { get; set; }
        public string ColorCode { get; set; }
        public int ProductCount { get; set; }
        public int ActiveProductCount { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
