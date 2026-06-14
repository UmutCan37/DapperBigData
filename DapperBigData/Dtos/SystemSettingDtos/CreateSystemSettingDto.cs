namespace DapperBigData.Dtos.SystemSettingDtos
{
    public class CreateSystemSettingDto
    {
        public string SettingKey { get; set; }
        public string SettingValue { get; set; }
        public bool IsToggle { get; set; }
        public string Description { get; set; }
    }
}
