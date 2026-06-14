namespace DapperBigData.Dtos.CityDtos
{
    public class GetByIdCityDto
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string PlateCode { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalCustomers { get; set; }
    }
}
