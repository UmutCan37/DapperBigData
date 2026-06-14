namespace DapperBigData.Dtos.PaymentMethodDtos
{
    public class GetByIdPaymentMethodDto
    {
        public int PaymentMethodId { get; set; }
        public string MethodName { get; set; }
        public string IconClass { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal RevenueShare { get; set; }
    }
}
