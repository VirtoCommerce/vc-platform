using Microsoft.Practices.ServiceLocation;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;
using VirtoCommerce.Framework.Web.Modularity;

namespace VirtoCommerce.Platform.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery.mousewheel.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/underscore").Include("~/Scripts/underscore.js"));
			bundles.Add(new ScriptBundle("~/bundles/moment").Include("~/Scripts/moment.js"));

            //AngularJS 
            //Note: must match the real path (~/Scripts/.) to find source map files references from .min.js (ex. # sourceMappingURL=angular-resource.min.js.map)
            bundles.Add(new ScriptBundle("~/Scripts/angular").Include(
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
                "~/Scripts/sortable.js"));
            //Angular ui
            bundles.Add(new ScriptBundle("~/bundles/angularui").Include(
                "~/Scripts/AngularUI/ui-router.js",
                "~/Scripts/angular-ui/ui-bootstrap.js",
                "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                "~/Scripts/angular-ui/ui-utils.js",
                "~/Scripts/angular-ui/ui-utils-ieshiv.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularmisc").Include(
                "~/Scripts/angular-multi-select.js",
                "~/Scripts/ng-context-menu.js",
                "~/Scripts/xeditable.js",
                "~/Scripts/ng-focus-on.js",
                "~/Scripts/ng-google-chart.js",
                "~/Scripts/ng-tags-input.js",
                "~/Scripts/textAngular.min.js",
                "~/Scripts/textAngular-rangy.min.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/angular-multi-select.css",
                "~/Content/ng-tags-input.css",
                "~/Content/textAngular.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
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
                "~/Content/themes/base/xeditable.css"
            ));

            //Metro UI
            bundles.Add(new StyleBundle("~/Content/themes/metro/css").Include(
                "~/Content/themes/metro/css/metro-bootstrap.css",
                "~/Content/themes/metro/css/metro-bootstrap-responsive.css",
                "~/Content/themes/metro/css/iconFont.css",
                "~/Content/themes/metro/css/custom.css"
            ));

            //Chosen
            bundles.Add(new StyleBundle("~/Content/chosen").Include(
                "~/Scripts/chosen/chosen.css"
            ));

            bundles.Add(new ScriptBundle("~/Scripts/chosen").Include(
                "~/Scripts/chosen/chosen.jquery.js"
            ));


            //Angular App
            bundles.Add(new ScriptBundle("~/bundles/app")
                .IncludeDirectory("~/Scripts/app/", "*.js", true)
                .IncludeDirectory("~/Scripts/common/", "*.js", true));

            // Register styles and scripts listed in module manifests ordered by dependency.
            var moduleCatalog = ServiceLocator.Current.GetInstance<IModuleCatalog>();
            var allModules = moduleCatalog.Modules.ToArray();
            var manifestModules = moduleCatalog.CompleteListWithDependencies(allModules).OfType<ManifestModuleInfo>().ToArray();
            var styles = manifestModules.SelectMany(m => m.Styles).ToArray();
            var scripts = manifestModules.SelectMany(m => m.Scripts).ToArray();

            bundles.Add(new StyleBundle("~/Content/modules").Include(styles));
            bundles.Add(new ScriptBundle("~/bundles/modules").Include(scripts));
        }
    }


    internal static class BundleExtensions
    {
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
                    bundle.IncludeDirectory(directory.VirtualPath, directory.SearchPattern, directory.SearchSubdirectories);
                }
            }

            return bundle;
        }
    }
}
