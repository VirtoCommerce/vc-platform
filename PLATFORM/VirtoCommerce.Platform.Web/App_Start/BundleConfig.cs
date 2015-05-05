using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Web
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Optimization;

    using Microsoft.Practices.ServiceLocation;

    #endregion

    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725

        #region Public Methods and Operators

        public static void RegisterBundles(BundleCollection bundles)
        {
            //BundleTable.EnableOptimizations = true;

            #region CSS

            bundles.Add(
                new BetterStyleBundle("~/css/core").Include(
                    "~/Scripts/codemirror/codemirror.css",
                    "~/Scripts/codemirror/fold/foldgutter.css",
                    "~/Scripts/codemirror/liquid.css",
                    "~/Content/select.css",
                    "~/Content/angular-gridster.min.css",
                    "~/Content/angular-multi-select.css",
                    "~/Content/ng-tags-input.css",
                    "~/Content/textAngular.css",
                    "~/Content/themes/base/jquery.ui.core.css",
                    "~/Content/themes/base/jquery.ui.resizable.css",
                    "~/Content/themes/base/jquery.ui.selectable.css",
                    "~/Content/themes/base/jquery.ui.accordion.css",
                    "~/Content/themes/base/jquery.ui.autocomplete.css",
                    "~/Content/themes/base/jquery.ui.button.css",
                    "~/Content/themes/base/jquery.ui.dialog.css",
                    "~/Content/themes/base/jquery.ui.slider.css",
                    "~/Content/themes/base/jquery.ui.tabs.css",
                    "~/Content/themes/base/jquery.ui.datepicker.css",
                    "~/Content/themes/base/jquery.ui.progressbar.css",
                    "~/Content/themes/base/jquery.ui.theme.css",
                    "~/Content/themes/base/xeditable.css",
                //SELECT2
                    "~/Content/select2.css",
                //Angular ui-grid
                    "~/Content/ui-grid-unstable.css",
                //Selectize
                    "~/Content/Selectize/css/selectize.default.css",
                //Theme UI
                    "~/Content/themes/main/css/reset.css",
                    "~/Content/themes/main/css/base-modules.css",
                    "~/Content/themes/main/css/project-modules.css",
                    "~/Content/themes/main/css/cosmetic.css"

                    ));

            #endregion

            #region JS

            bundles.Add(
                new ScriptBundle("~/scripts/jquery").Include(
                    "~/Scripts/jquery-{version}.js",
                    "~/Scripts/jquery.mousewheel.js",
                    "~/Scripts/jquery.signalR-2.2.0.min.js",
                    "~/Scripts/jquery-ui-{version}.js",
                    "~/Scripts/underscore.js",
                    "~/Scripts/moment.js"));

            //AngularJS 
            //Note: must match the real path (~/Scripts/.) to find source map files references from .min.js (ex. # sourceMappingURL=angular-resource.min.js.map)
            bundles.Add(
                new ScriptBundle("~/scripts/angular").Include(
                    "~/Scripts/angular.js",
                    "~/Scripts/angular-animate.js",
                    "~/Scripts/angular-cookies.js",
                    "~/Scripts/angular-file-upload.js",
                    "~/Scripts/angular-loader.js",
                    "~/Scripts/angular-resource.js",
                    "~/Scripts/angular-route.js",
                    "~/Scripts/angular-sanitize.js",
                    "~/Scripts/angular-route.js",
                    "~/Scripts/angular-touch.js",
                    "~/Scripts/sortable.js",
                    "~/Scripts/AngularUI/ui-router.js",
                //Angular ui
                    "~/Scripts/angular-ui/ui-bootstrap.js",
                    "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                    "~/Scripts/angular-ui/ui-utils.js",
                    "~/Scripts/angular-ui/ui-utils-ieshiv.js",
                    "~/Scripts/angular-multi-select.js",
                     "~/Scripts/angular-ui/select.js",
                //Angular ui-grid
                    "~/Scripts/ui-grid-unstable.js",
                // Angular Misc
                    "~/Scripts/angular-gridster.js",
                    "~/Scripts/ng-context-menu.js",
                    "~/Scripts/xeditable.js",
                    "~/Scripts/ng-focus-on.js",
                    "~/Scripts/ng-google-chart.js",
                    "~/Scripts/ng-tags-input.js",
                    "~/Scripts/ngStorage.min.js",
                    "~/Scripts/textAngular.min.js",
                    "~/Scripts/textAngular-rangy.min.js")
                    .IncludeDirectory("~/Scripts/codemirror/", "*.js", true)
                    .IncludeDirectory("~/Scripts/app/", "*.js", true)
                    .IncludeDirectory("~/Scripts/common/", "*.js", true));

            #endregion

            // Register styles and scripts listed in module manifests ordered by dependency.
            var moduleCatalog = ServiceLocator.Current.GetInstance<IModuleCatalog>();
            var allModules = moduleCatalog.Modules.ToArray();
            var manifestModules =
                moduleCatalog.CompleteListWithDependencies(allModules).OfType<ManifestModuleInfo>().ToArray();
            var styles = manifestModules.SelectMany(m => m.Styles).ToArray();
            var scripts = manifestModules.SelectMany(m => m.Scripts).ToArray();

            bundles.Add(new BetterStyleBundle("~/css/modules").Include(styles));
            bundles.Add(new ScriptBundle("~/scripts/modules").Include(scripts));
        }

        #endregion
    }

    internal static class BundleExtensions
    {
        #region Public Methods and Operators

        public static Bundle Include(this Bundle bundle, IEnumerable<ManifestBundleItem> items)
        {
            foreach (var item in items)
            {
                var file = item as ManifestBundleFile;
                var directory = item as ManifestBundleDirectory;

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

            return bundle;
        }

        #endregion
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
        #region Public Methods and Operators

        public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
        {
            return files;
        }

        #endregion
    }

    public class CssRewriteUrlTransformWrapper : IItemTransform
    {
        #region Public Methods and Operators

        public string Process(string includedVirtualPath, string input)
        {
            return new CssRewriteUrlTransform().Process("~" + VirtualPathUtility.ToAbsolute(includedVirtualPath), input);
        }

        #endregion
    }
}