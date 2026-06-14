namespace DapperBigData.Dtos.MonthlyTargetDtos
{
    public class GetByIdMonthlyTargetDto
    {
        public int TargetYear { get; set; }
        public int TargetMonth { get; set; }
        public decimal RevenueTarget { get; set; }
        public int OrderTarget { get; set; }
        public decimal ActualRevenue { get; set; }
        public int ActualOrders { get; set; }
        public decimal RevenueAchievementPct { get; set; }
        public decimal OrderAchievementPct { get; set; }
    }
}
