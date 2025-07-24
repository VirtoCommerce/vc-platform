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
            SupportEmbeddedMode = moduleAppInfo.SupportEmbeddedMode;
        }


        public string Title { get; set; }

        public string Description { get; set; }

        public string IconUrl { get; set; }

        public string RelativeUrl { get; set; }

        public string Permission { get; set; }

        public bool SupportEmbeddedMode { get; set; }
    }
}
