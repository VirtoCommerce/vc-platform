using System;
using System.Linq;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Settings;


namespace VirtoCommerce.Platform.Core.Assets
{
    [Obsolete("Deprecated. Use assets from Assets module.")]
    public abstract class BasicBlobProvider
    {
        private readonly ISettingsManager _settingsManager;
        private readonly IOptions<PlatformOptions> _platformOptions;
        protected BasicBlobProvider(IOptions<PlatformOptions> platformOptions, ISettingsManager settingsManager)
        {
            _platformOptions = platformOptions;
            _settingsManager = settingsManager;
        }

        public virtual bool IsExtensionBlacklisted(string path)
        {
            var blackList = _platformOptions.Value.FileExtensionsBlackList.Union(
                _settingsManager?.GetObjectSettingAsync(PlatformConstants.Settings.Security.FileExtensionsBlackList.Name).Result.AllowedValues.Cast<string>() ?? new string[0]);
            return (blackList.Any(x => path.Trim().EndsWith(x.Trim(), StringComparison.OrdinalIgnoreCase)));
        }
    }
}
