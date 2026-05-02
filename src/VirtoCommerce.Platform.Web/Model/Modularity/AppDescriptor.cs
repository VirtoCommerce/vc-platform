using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Web.Model.Modularity
{
    public class AppDescriptor : Entity
    {
        public AppDescriptor()
        {
        }

        public AppDescriptor(ManifestAppInfo moduleAppInfo)
           : this()
        {
            Id = moduleAppInfo.Id;
            Title = moduleAppInfo.Title;
            Description = moduleAppInfo.Description;
            IconUrl = moduleAppInfo.IconUrl;
            RelativeUrl = moduleAppInfo.RelativeUrl;
            Permission = moduleAppInfo.Permission;
            Placement = moduleAppInfo.Placement;
            // Kept for backwards compatibility with clients that still read
            // the legacy boolean (older VC-Shell builds, third-party JS).
            // Always derived from Placement so the two stay consistent.
            SupportEmbeddedMode = moduleAppInfo.Placement == AppPlacement.MainMenu;
        }


        public string Title { get; set; }

        public string Description { get; set; }

        public string IconUrl { get; set; }

        public string RelativeUrl { get; set; }

        public string Permission { get; set; }

        /// <summary>
        /// Where the app surfaces in the admin navigation
        /// (<c>AppMenu</c> / <c>MainMenu</c> / <c>Hidden</c>).
        /// </summary>
        public AppPlacement Placement { get; set; }

        /// <summary>
        /// Deprecated. Use <see cref="Placement"/> instead. Equivalent to
        /// <c>Placement == AppPlacement.MainMenu</c>; kept on the JSON
        /// contract for backwards compatibility.
        /// </summary>
        public bool SupportEmbeddedMode { get; set; }
    }
}
