namespace DapperBigData.Dtos.OrderDtos
{
    public class ResultOrderDto
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int PaymentMethodId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public string CustomerFullName { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentIconClass { get; set; }
        public string CityName { get; set; }
    }
}
