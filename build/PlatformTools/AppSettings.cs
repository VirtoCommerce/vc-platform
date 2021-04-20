using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace PlatformTools
{
    class AppSettings
    {
        private static IConfiguration _configuration;

        public static IConfiguration GetConfiguration(string basePath, string appsettingsPath)
        {
            if(_configuration == null && System.IO.File.Exists(appsettingsPath))
            {
                var builder = new ConfigurationBuilder().SetBasePath(basePath).AddJsonFile(appsettingsPath);
                _configuration = builder.Build();
            }
            return _configuration;
        }
    }
}
