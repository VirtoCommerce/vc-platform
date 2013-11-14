using System.Collections.Generic;
using VirtoCommerce.ManagementClient.Configuration.Model;

namespace VirtoCommerce.ManagementClient.Configuration
{
    public static class ConfigurationManager
    {
        private static List<ConfigurationSection> _Settings;
        public static List<ConfigurationSection> Settings
        {
            get { return _Settings ?? (_Settings = new List<ConfigurationSection>()); }
        }
    }
}
