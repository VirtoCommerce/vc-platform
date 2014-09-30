using System.Web.Optimization;
using VirtoCommerce.Web.Virto.Helpers.MVC;

namespace VirtoCommerce.Web
{
    /// <summary>
    /// Class BundleConfig.
    /// </summary>
	public class BundleConfig
	{
        public const string VirtoAdminStyles = "~/Areas/VirtoAdmin/Content/styles";

		// For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        /// <summary>
        /// Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
						"~/Scripts/jquery-ui-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.unobtrusive*",
						"~/Scripts/jquery.validate*"));

			bundles.Add(new ScriptBundle("~/bundles/jquerymisc").Include(
			"~/Scripts/v/virto-jquery.js",
            "~/Scripts/cloudzoom.js",
            "~/Scripts/ajaxq.js",
			"~/Scripts/jquery.rateit.js",
			"~/Scripts/v/virto-commerce.js",
            "~/Scripts/responsive/main.js",
			"~/Scripts/v/validation.js"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/responsive").Include(
                "~/Scripts/responsive/virto.slider.js",
                "~/Scripts/responsive/main.slider.js",
                "~/Scripts/responsive/camera.js",
                "~/Scripts/responsive/jquery.easing.{version}.js"
                ));

            //SignalR
            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                 "~/Scripts/v/virto-jquery.js",
                "~/Scripts/jquery.signalR-{version}.js",
                "~/Scripts/v/virto-commerce.js",
                "~/Scripts/v/validation.js"
                ));

            //Geocomplete
            bundles.Add(new ScriptBundle("~/bundles/geo").Include("~/Scripts/jquery.geocomplete.js"));

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
						"~/Content/themes/base/jquery.ui.tooltip.css",
						"~/Content/themes/base/jquery.ui.spinner.css",
						"~/Content/themes/base/jquery.ui.menu.css"));

            bundles.Add(new StyleBundle("~/Content/themes/default/css").Include(
                "~/Content/themes/default/reset.css",
                "~/Content/themes/default/custom.css",
                "~/Content/themes/default/grid.css",
                "~/Content/themes/default/forms.css",
                "~/Content/themes/default/popup.css",
                "~/Content/themes/default/transition.css",
                "~/Content/themes/default/main.css",
                "~/Content/themes/default/responsive.css",
                "~/Content/themes/default/camera.css",
                "~/Content/themes/default/cloudzoom.css",
                "~/Content/themes/default/messages.css",
                "~/Content/themes/default/rateit.css",
                "~/Content/themes/default/flags.css"
                ));


            //Register store specific css.
            StoreStyle.Register(bundles,@"~/Content/themes/{0}");

            //Admin area
            bundles.Add(new StyleBundle(VirtoAdminStyles).Include(
            "~/Areas/VirtoAdmin/Content/reset.css",
            "~/Areas/VirtoAdmin/Content/main.css",
            "~/Areas/VirtoAdmin/Content/responsive.css"
            ));
		}
	}
}