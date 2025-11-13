namespace VirtoCommerce.Platform.Core
{
    public class PlatformUIOptions
    {
        public bool Enable { get; set; } = true;

        public EnvironmentBannerOptions EnvironmentBanner { get; set; } = new EnvironmentBannerOptions();
    }
}
