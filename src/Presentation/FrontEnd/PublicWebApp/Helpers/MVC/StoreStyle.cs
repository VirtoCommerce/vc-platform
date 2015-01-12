using System.IO;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using VirtoCommerce.ApiWebClient.Helpers;

namespace VirtoCommerce.Web.Helpers.MVC
{
    public static class StoreStyle
    {
        /// <summary>
        /// Renders the styles for current store using specified path format.
        /// </summary>
        /// <param name="pathFormat">The path format.</param>
        /// <returns></returns>
        public static IHtmlString Render(params string[] pathFormat)
        {
            var validPaths = from path in pathFormat
                             select string.Format(path, StoreHelper.CustomerSession.StoreId)
                                 into formatedPath
                                 let bundle = BundleTable.Bundles.GetBundleFor(formatedPath)
                                 where bundle != null
                                 select formatedPath;

            return Styles.Render(validPaths.ToArray());
        }

        /// <summary>
        /// Registers the css for given path format for each store, if such directory exsits.
        /// </summary>
        /// <param name="bundles">The bundles.</param>
        /// <param name="pathFormat">The path format.</param>
        public static void Register(BundleCollection bundles, string pathFormat)
        {
            if (StoreHelper.StoreClient != null)
            {
                foreach (var store in StoreHelper.StoreClient.GetStores())
                {
                    var path = string.Format(pathFormat, store.Id);
                    var pPath = HttpContext.Current.Server.MapPath(path);
                    if (Directory.Exists(pPath))
                    {
                        bundles.Add(new StyleBundle(path + "/css", store.Id).IncludeDirectory(path, "*.css"));
                    }
                }
            }
        }
    }
}