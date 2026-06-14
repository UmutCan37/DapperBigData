namespace DapperBigData.Dtos.ProductDtos
{
    public class CreateProductDto
    {
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public string Sku { get; set; }
        public decimal UnitPrice { get; set; }
        public int StockQuantity { get; set; }
    }
}
