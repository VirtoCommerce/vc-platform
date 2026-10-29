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

        /// <summary>
        /// The <c>&lt;app&gt;</c> id of the admin UI served at the site root. Empty: the installed admin UI with the
        /// highest <c>&lt;adminUI priority&gt;</c>. <c>none</c>: no admin UI at the root, the platform shows its own page.
        /// Any installed app can be named, also one without <c>&lt;adminUI&gt;</c>; it is then served like an
        /// <see cref="Modularity.AdminUIHosting.App"/> admin UI.
        /// </summary>
        public string RootApp { get; set; }
    }
}
