using System.Web.Optimization;

namespace VirtoCommerce.Web
{
    /// <summary>
    /// Class BundleConfig.
    /// </summary>
	public class BundleConfig
	{
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
			"~/Scripts/jquery-jqzoom.js",
				//"~/Scripts/jquery.gzoom.js",
			"~/Scripts/jquery.nivo.slider.js",
			"~/Scripts/jquery.rateit.js",
			"~/Scripts/jquery-thickbox.js",
			"~/Scripts/v/virto-commerce.js",
			"~/Scripts/v/validation.js"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new StyleBundle("~/Content/themes/default/s").Include(
				"~/Content/themes/default/styles.css",
				"~/Content/themes/default/widgets.css",
				"~/Content/themes/default/thickbox.css",
				"~/Content/themes/default/jquery.ui.autocomplete.css",
				"~/Content/themes/default/jquery.jqzoom.css",
				"~/Content/themes/default/redesign.css",
				"~/Content/themes/default/rateit.css",
				"~/Content/themes/default/flags.css",
				"~/Content/themes/default/virto_reviews.css",
				"~/Content/themes/default/nivo-slider.css"));

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
		}
	}
}