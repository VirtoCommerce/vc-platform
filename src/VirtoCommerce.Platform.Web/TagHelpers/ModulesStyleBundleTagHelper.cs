using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Web.TagHelpers
{
    public class ModulesStyleBundleTagHelper : ModulesBundleTagHelperBase
    {
        public ModulesStyleBundleTagHelper(ILocalModuleCatalog localModuleCatalog, IOptions<LocalStorageModuleCatalogOptions> options, IPlatformMemoryCache platformMemoryCache)
            : base(localModuleCatalog, options, platformMemoryCache)
        {
        }
        protected override TagBuilder GetTagBuilder(string bundleVirtualPath, string version)
        {
            var result = new TagBuilder("link");
            if (AppendVersion && !string.IsNullOrEmpty(version))
            {
                bundleVirtualPath += $"?v={version}";
            }
            result.Attributes.Add("href", bundleVirtualPath);
            result.Attributes.Add("rel", "stylesheet");
            return result;
        }
    }
}

