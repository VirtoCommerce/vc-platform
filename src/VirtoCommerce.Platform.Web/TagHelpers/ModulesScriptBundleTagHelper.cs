using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Web.TagHelpers
{
    public class ModulesScriptBundleTagHelper : ModulesBundleTagHelperBase
    {
        public ModulesScriptBundleTagHelper(ILocalModuleCatalog localModuleCatalog, IOptions<LocalStorageModuleCatalogOptions> options, IPlatformMemoryCache platformMemoryCache)
            : base(localModuleCatalog, options, platformMemoryCache)
        {
        }
        protected override TagBuilder GetTagBuilder(string bundleVirtualPath, string version)
        {
            var result = new TagBuilder("script");
            if (AppendVersion && !string.IsNullOrEmpty(version))
            {
                bundleVirtualPath += $"?v={version}";
            }
            result.Attributes.Add("src", bundleVirtualPath);
            result.Attributes.Add("type", "text/javascript");
            return result;
        }

    }
}

