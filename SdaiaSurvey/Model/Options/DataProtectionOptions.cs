namespace SdaiaSurvey.Model.Options
{
    public class DataProtectionKeyOptions
    {
        public bool Enable { get; set; }
        public string Thumbprint { get; set; }
        public string RedisConnectionString { get; set; }
        public string ApplicationName { get; set; }
        public string KeyFilePath { get; set; }
        public bool UseRedis { get; set; }
    }

}