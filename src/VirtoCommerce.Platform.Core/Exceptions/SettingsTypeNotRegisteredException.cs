using System;

namespace VirtoCommerce.Platform.Core.Exceptions
{
    [Obsolete("Not used in the platform, will be removed when migrating to a new version of .net")]
    public class SettingsTypeNotRegisteredException : PlatformException
    {
        public SettingsTypeNotRegisteredException(string settingsType)
            : base($"Settings for type: {settingsType} not registered, please register it first")
        {
        }
    }
}
