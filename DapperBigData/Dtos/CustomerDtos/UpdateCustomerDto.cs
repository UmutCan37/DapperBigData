namespace DapperBigData.Dtos.CustomerDtos
{
    public class UpdateCustomerDto
    {
        public int CustomerId { get; set; }
        public int CityId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
    }
}
