using System;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient
{
    public class GlobalConfigService : IGlobalConfigService
    {
        private readonly IAppSettings _settings;

        public GlobalConfigService()
        {
            _settings = new AppSettings();
        }

        public void Update(string settingName, object value)
        {
            if (String.IsNullOrEmpty(settingName))
                throw new ArgumentNullException("Setting name must be provided");

            var setting = _settings[settingName];

            if (setting == null)
            {
                throw new ArgumentException("Setting " + settingName + " not found.");
            }
            else if (setting.GetType() != value.GetType())
            {
                throw new InvalidCastException("Unable to cast value to " + setting.GetType());
            }
            else
            {
                _settings[settingName] = value;
                _settings.Save();
            }

        }

        public object Get(string settingName)
        {
            if (String.IsNullOrEmpty(settingName))
                throw new ArgumentNullException("Setting name must be provided");

            return _settings[settingName];
        }
    }
}
