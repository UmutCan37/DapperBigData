namespace DapperBigData.Dtos.CustomerDtos
{
    public class GetByIdCustomerDto
    {
        public int CustomerId { get; set; }
        public int CityId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
        public string CityName { get; set; }
        public string PlateCode { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
    }
}
