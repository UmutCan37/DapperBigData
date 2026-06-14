namespace DapperBigData.Dtos.OrderDetailDtos
{
    public class ResultOrderDetailDto
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal LineTotal => Quantity * UnitPrice;
        public string ProductName { get; set; }
        public string Sku { get; set; }
        public string CategoryName { get; set; }
    }
}
