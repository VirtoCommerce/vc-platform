using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.Platform.Core
{
    public class PlatformUIOptions
    {
        public bool Enable { get; set; } = true;

        public EnvironmentBannerOptions EnvironmentBanner { get; set; } = new EnvironmentBannerOptions();

        /// <summary>
        /// Controls which authenticated users may enter the admin UI.
        /// </summary>
        public AdminUIAccessOptions Access { get; set; } = new AdminUIAccessOptions();
    }
}
