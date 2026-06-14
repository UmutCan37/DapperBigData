namespace DapperBigData.Dtos.OrderDtos
{
    public class CreateOrderDto
    {
        public int CustomerId { get; set; }
        public int PaymentMethodId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; }
    }
}
