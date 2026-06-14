namespace DapperBigData.Dtos.SystemSettingDtos
{
    public class ResultSystemSettingDto
    {
        public int SettingId { get; set; }
        public string SettingKey { get; set; }
        public string SettingValue { get; set; }
        public bool IsToggle { get; set; }
        public string Description { get; set; }
    }
}
