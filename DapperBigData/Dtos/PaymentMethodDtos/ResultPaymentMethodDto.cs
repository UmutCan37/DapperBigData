namespace DapperBigData.Dtos.PaymentMethodDtos
{
    public class ResultPaymentMethodDto
    {
        public int PaymentMethodId { get; set; }
        public string MethodName { get; set; }
        public string IconClass { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
