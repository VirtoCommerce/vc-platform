using System.Web.Mvc;
using VirtoCommerce.Foundation.Assets;
using VirtoCommerce.Web.Client.Actions;
using VirtoCommerce.Web.Client.Extensions.Filters;
using VirtoCommerce.Web.Virto.Helpers.MVC;

namespace VirtoCommerce.Web.Controllers
{
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
            var folder = AssetConfiguration.Instance.Connection.StorageFolder;
            folder = folder.EndsWith("/") ? folder : folder + "/";
            return new DownloadResult {VirtualPath = path, VirtualBasePath = folder};
        }
    }
}