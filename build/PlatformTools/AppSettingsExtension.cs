using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace PlatformTools
{
    public static class AppSettingsExtension
    {
        public static string GetModulesDiscoveryPath(this IConfiguration configuration)
        {
            var virtoSection = configuration.GetSection("VirtoCommerce");
            var result = virtoSection.GetValue<string>("DiscoveryPath", "./modules");
            return result;
        }
    }
}
