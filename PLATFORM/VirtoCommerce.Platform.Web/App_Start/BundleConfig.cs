using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using Microsoft.Practices.ServiceLocation;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Modularity.Exceptions;

namespace VirtoCommerce.Platform.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725

        public static void RegisterBundles(BundleCollection bundles)
        {
            //BundleTable.EnableOptimizations = true;

            #region CSS

            bundles.Add(
                new BetterStyleBundle(Startup.VirtualRoot + "/css/core").IncludeAndFixRoot(
                    "~/Content/allStyles.css",
                    "~/Scripts/codemirror/codemirror.css",
                    "~/Scripts/codemirror/fold/foldgutter.css",
                    "~/Scripts/codemirror/liquid.css",
                    // "~/Content/angular-gridster.css", // customized style
                    //SELECT2
                    "~/Content/select2.css", // used in selectors
                    //Theme UI
                    "~/Content/themes/main/css/reset.css",
                    "~/Content/themes/main/css/font-awesome.css",
                    "~/Content/themes/main/css/base-modules.css",
                    "~/Content/themes/main/css/project-modules.css",
                    "~/Content/themes/main/css/cosmetic.css"
                    ));

            #endregion

            #region JS
            //AngularJS 
            //Note: must match the real path (~/Scripts/.) to find source map files references from .min.js (ex. # sourceMappingURL=angular-resource.min.js.map)
            bundles.Add(
                new ScriptBundle(Startup.VirtualRoot + "/scripts/angular")
                    .IncludeAndFixRoot("~/Scripts/allPackages.js")
                    .IncludeDirectoryAndFixRoot("~/Scripts/codemirror/", "*.js", true)
                    .IncludeDirectoryAndFixRoot("~/Scripts/app/", "*.js", true)
                    .IncludeDirectoryAndFixRoot("~/Scripts/common/", "*.js", true));

            #endregion

            // Register styles and scripts listed in module manifests ordered by dependency.
            var moduleCatalog = ServiceLocator.Current.GetInstance<IModuleCatalog>();
            var allModules = moduleCatalog.Modules.ToArray();
            var manifestModules = moduleCatalog.CompleteListWithDependencies(allModules)
                .OfType<ManifestModuleInfo>()
                .ToArray();

            var styles = manifestModules
                .SelectMany(m => m.Styles.Select(i => new BundleItem { Module = m, Item = i }))
                .ToArray();

            var scripts = manifestModules
                .SelectMany(m => m.Scripts.Select(i => new BundleItem { Module = m, Item = i }))
                .ToArray();

            bundles.Add(new BetterStyleBundle(Startup.VirtualRoot + "/css/modules").Include(styles));
            bundles.Add(new ScriptBundle(Startup.VirtualRoot + "/scripts/modules").Include(scripts));
        }
    }

    internal class BundleItem
    {
        public ModuleInfo Module { get; set; }
        public ManifestBundleItem Item { get; set; }
    }

    internal static class BundleExtensions
    {
        public static Bundle IncludeAndFixRoot(this Bundle bundle, params string[] items)
        {
            bundle.Include(items.Select(FixVirtualRoot).ToArray());
            return bundle;
        }

        public static Bundle IncludeDirectoryAndFixRoot(this Bundle bundle, string directoryVirtualPath, string searchPattern, bool searchSubdirectories)
        {
            var virtualPath = FixVirtualRoot(directoryVirtualPath);
            bundle.IncludeDirectory(virtualPath, searchPattern, searchSubdirectories);

            return bundle;
        }

        private static string FixVirtualRoot(string virtualPath)
        {
            if (virtualPath.StartsWith("~/"))
                virtualPath = Startup.VirtualRoot + virtualPath.Substring(1);
            return virtualPath;
        }

        public static Bundle Include(this Bundle bundle, IEnumerable<BundleItem> items)
        {
            foreach (var item in items)
            {
                var file = item.Item as ManifestBundleFile;
                var directory = item.Item as ManifestBundleDirectory;

                try
                {
                    if (file != null)
                    {
                        bundle.Include(file.VirtualPath);
                    }

                    if (directory != null)
                    {
                        bundle.IncludeDirectory(
                            directory.VirtualPath,
                            directory.SearchPattern,
                            directory.SearchSubdirectories);
                    }
                }
                catch (Exception ex)
                {
                    Exception moduleException;

                    if (item.Module.ModuleInstance != null)
                    {
                        var assemblyName = item.Module.ModuleInstance.GetType().Assembly.FullName;
                        moduleException = new ModuleInitializeException(item.Module.ModuleName, assemblyName, ex.Message, ex);
                    }
                    else
                    {
                        moduleException = new ModuleInitializeException(item.Module.ModuleName, ex.Message, ex);
                    }

                    throw moduleException;
                }
            }

            return bundle;
        }
    }

    public class BetterStyleBundle : StyleBundle
    {
        #region Constructors and Destructors

        public BetterStyleBundle(string virtualPath)
            : base(virtualPath)
        {
        }

        public BetterStyleBundle(string virtualPath, string cdnPath)
            : base(virtualPath, cdnPath)
        {
        }

        #endregion

        #region Public Properties

        public override IBundleOrderer Orderer
        {
            get
            {
                return new NonOrderingBundleOrderer();
            }
            set
            {
                throw new Exception("Unable to override Non-Ordered bundler");
            }
        }

        #endregion

        #region Public Methods and Operators

        public override Bundle Include(params string[] virtualPaths)
        {
            foreach (var virtualPath in virtualPaths)
            {
                base.Include(virtualPath, new CssRewriteUrlTransformWrapper());
            }
            return this;
        }

        #endregion
    }

    // This provides files in the same order as they have been added. 
    public class NonOrderingBundleOrderer : IBundleOrderer
    {
        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }
    }

    public class CssRewriteUrlTransformWrapper : IItemTransform
    {
        public string Process(string includedVirtualPath, string input)
        {
            return new CssRewriteUrlTransform().Process("~" + VirtualPathUtility.ToAbsolute(includedVirtualPath), input);
        }
    }
}
