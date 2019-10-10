using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Web.TagHelpers.Internal;

namespace VirtoCommerce.Platform.Web.TagHelpers
{
    public abstract class ModulesBundleTagHelperBase : TagHelper
    {
        private readonly ILocalModuleCatalog _localModuleCatalog;
        private readonly IPlatformMemoryCache _platformMemoryCache;
        private readonly LocalStorageModuleCatalogOptions _localStorageModuleCatalogOptions;
        private FileVersionHashProvider _fileVersionProvider;

        public ModulesBundleTagHelperBase(ILocalModuleCatalog localModuleCatalog, IOptions<LocalStorageModuleCatalogOptions> options, IPlatformMemoryCache platformMemoryCache)
        {
            _localModuleCatalog = localModuleCatalog;
            _platformMemoryCache = platformMemoryCache;
            _localStorageModuleCatalogOptions = options.Value;
        }

        [HtmlAttributeName("asp-append-version")]
        public bool AppendVersion { get; set; }

        [HtmlAttributeName("bundle-path")]
        public string BundlePath { get; set; }

        protected abstract TagBuilder GetTagBuilder(string bundleVirtualPath, string version);

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            var sucesfullyLoadedModules = _localModuleCatalog.Modules.OfType<ManifestModuleInfo>().Where(x => x.Errors.IsNullOrEmpty());
            foreach (var module in sucesfullyLoadedModules)
            {
                var normalizedBundlePath = BundlePath.Replace("~/", "").Replace("\\", "/").TrimStart('/');
                var moduleBundleVirtualPath = $"/Modules/$({module.ModuleName})/{normalizedBundlePath}";
                var bundlePhysicalPath = Path.Combine(module.FullPhysicalPath, normalizedBundlePath);
                if (File.Exists(bundlePhysicalPath))
                {
                    string version = null;
                    if (AppendVersion)
                    {
                        EnsureFileVersionProvider();
                        version = _fileVersionProvider.GetFileVersionHash(bundlePhysicalPath);
                    }
                    var tagBuilder = GetTagBuilder(moduleBundleVirtualPath, version);
                    output.Content.AppendHtml(tagBuilder);
                    output.Content.AppendHtml(Environment.NewLine);
                }
            }
        }

        private void EnsureFileVersionProvider()
        {
            if (_fileVersionProvider == null)
            {
                _fileVersionProvider = new FileVersionHashProvider(_localStorageModuleCatalogOptions.DiscoveryPath, _platformMemoryCache);
            }
        }
    }
}
