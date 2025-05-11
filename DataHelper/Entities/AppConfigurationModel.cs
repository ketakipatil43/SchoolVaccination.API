namespace DataHelper.Entities
{
    public class AppConfigurationModel
    {
        public AppConfigurationModel()
        {
            JwtSettings = new JwtSettings();
        }
        public string DBConnectionString { get; set; }
        public string CorsURL { get; set; }
        public JwtSettings JwtSettings { get; set; }

    }

    public class JwtSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int MinutesToExpiration { get; set; }

        public TimeSpan Expire => TimeSpan.FromMinutes(MinutesToExpiration);
        public int RefreshTokenValidDays { get; set; }
        public int TokenAuthorize { get; set; }
    }
}
