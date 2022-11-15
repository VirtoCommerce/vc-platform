using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Web.Model.Modularity
{
    public class AppDescriptor: Entity
    {
        public AppDescriptor()
        {
        }

        public AppDescriptor(ManifestAppInfo moduleAppInfo)
           : this()
        {
            this.Id = moduleAppInfo.Id;
            this.Title = moduleAppInfo.Title;
            this.Description = moduleAppInfo.Description;
            this.IconUrl = moduleAppInfo.IconUrl;
            this.RelativeUrl = moduleAppInfo.RelativeUrl;
            this.Permission = moduleAppInfo.Permission;
        }


        public string Title { get; set; }

        public string Description { get; set; }

        public string IconUrl { get; set; }

        public string RelativeUrl { get; set; }

        public string Permission { get; set; }
    }
}
