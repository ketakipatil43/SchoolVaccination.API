using DataHelper.Entities;
using Microsoft.Extensions.Configuration;

namespace DataHelper.Package
{
    public static class AppConfiguration
    {
        public static AppConfigurationModel ApplyConfiguration(IConfigurationBuilder configuration)
        {
            var configVal = configuration.Build();
            var _appconfig = new AppConfigurationModel();

            _appconfig.DBConnectionString = configVal.GetConnectionString("SqlServer") ?? string.Empty;
            _appconfig.CorsURL = configVal.GetValue<string>("CorsURL") ?? string.Empty;
            configVal.GetSection("JwtSettings").Bind(_appconfig.JwtSettings);
            return _appconfig;
        }
    }
}
