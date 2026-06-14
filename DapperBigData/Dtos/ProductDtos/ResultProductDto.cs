namespace DapperBigData.Dtos.ProductDtos
{
    public class ResultProductDto
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public string Sku { get; set; }
        public decimal UnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public string CategoryName { get; set; }
        public string CategoryColor { get; set; }

        public int TotalSold { get; set; }      
        public decimal TotalRevenue { get; set; }
    }
}
