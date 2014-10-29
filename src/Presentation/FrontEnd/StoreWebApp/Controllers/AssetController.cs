using System.Web.Mvc;
using VirtoCommerce.Foundation.Assets;
using VirtoCommerce.Web.Client.Actions;
using VirtoCommerce.Web.Client.Extensions.Filters.Caching;

namespace VirtoCommerce.Web.Controllers
{
    using VirtoCommerce.Foundation.Data.Infrastructure;

    /// <summary>
	/// Class AssetController.
	/// </summary>
	public class AssetController : ControllerBase
    {
		/// <summary>
		/// Downloads asset from specified path.
		/// </summary>
		/// <param name="path">The asset path.</param>
		/// <returns>DownloadResult.</returns>
        [CustomOutputCache(CacheProfile = "AssetCache")]
        public ActionResult Index(string path)
        {
            var folder = ConnectionHelper.GetConnectionString(AssetConfiguration.Instance.Connection.StorageConnectionStringName);
            folder = folder.EndsWith("/") ? folder : folder + "/";
            return new DownloadResult {VirtualPath = path, VirtualBasePath = folder};
        }
    }
}