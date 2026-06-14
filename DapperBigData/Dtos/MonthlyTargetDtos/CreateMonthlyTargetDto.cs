namespace DapperBigData.Dtos.MonthlyTargetDtos
{
    public class CreateMonthlyTargetDto
    {
        public int TargetYear { get; set; }
        public int TargetMonth { get; set; }
        public decimal RevenueTarget { get; set; }
        public int OrderTarget { get; set; }

    }
}
