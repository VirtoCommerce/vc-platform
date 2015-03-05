#region
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Content.Data.Services;
using VirtoCommerce.Foundation.Assets.Repositories;
using VirtoCommerce.Framework.Web.Asset;
using VirtoCommerce.Framework.Web.Settings;
using VirtoCommerce.ThemeModule.Web.Converters;
using VirtoCommerce.ThemeModule.Web.Models;

#endregion

namespace VirtoCommerce.ThemeModule.Web.Controllers.Api
{
	[RoutePrefix("api/cms/{storeId}")]
	public class ThemeController : ApiController
	{
		#region Fields
		private readonly IThemeService _themeService;
		private readonly IBlobStorageProvider _blobProvider;
		private readonly string _tempPath;
		#endregion

		#region Constructors and Destructors
		public ThemeController(Func<string, IThemeService> factory, ISettingsManager manager, IBlobStorageProvider blobProvider)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}

			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}

			if (blobProvider == null)
			{
				throw new ArgumentNullException("blobProvider");
			}

			var chosenRepository = manager.GetValue(
				"VirtoCommerce.ThemeModule.MainProperties.ThemesRepositoryType",
				string.Empty);

			var themeService = factory.Invoke(chosenRepository);
			this._themeService = themeService;

			_blobProvider = blobProvider;
			_tempPath = HostingEnvironment.MapPath("~/App_Data/Uploads/");
		}
		#endregion

		#region Public Methods and Operators
		[HttpGet]
		[ResponseType(typeof(ThemeAsset))]
		[Route("themes/{themeId}/assets/{*assetId}")]
		public IHttpActionResult GetThemeAsset(string assetId, string storeId, string themeId)
		{
			var item = this._themeService.GetThemeAsset(storeId, themeId, assetId);
			return this.Ok(item.ToWebModel());
		}

		[HttpGet]
		[Route("themes/{themeId}/assets")]
		public IHttpActionResult GetThemeAssets(string storeId, string themeId, bool loadContent = false)
		{
			var items = this._themeService.GetThemeAssets(storeId, themeId, loadContent);

			if (!loadContent)
			{
				return this.Ok(items.ToWebModel());
			}

			return this.Ok(items.Select(s => s.ToWebModel()).ToArray());
		}

		[HttpGet]
		[ResponseType(typeof(Theme[]))]
		[Route("themes")]
		public IHttpActionResult GetThemes(string storeId)
		{
			var items = this._themeService.GetThemes(storeId);
			return this.Ok(items.Select(s => s.ToWebModel()).ToArray());
		}

		[HttpPost]
		[Route("themes/{themeId}/assets")]
		public IHttpActionResult SaveItem(ThemeAsset asset, string storeId, string themeId)
		{
			this._themeService.SaveThemeAsset(storeId, themeId, asset.ToDomainModel());
			return this.Ok();
		}

		[HttpDelete]
		[Route("themes/{themeId}/assets")]
		public IHttpActionResult DeleteAssets(string storeId, string themeId, [FromUri]string[] assetIds)
		{
			this._themeService.DeleteThemeAssets(storeId, themeId, assetIds);
			return this.Ok();
		}

		//[HttpPost]
		//[Route("themes/{themeId}/assets/image")]
		//public IHttpActionResult SaveImageItem()
		//{
		//	if (!Request.Content.IsMimeMultipartContent())
		//	{
		//		throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
		//	}

		//	var blobMultipartProvider = new BlobStorageMultipartProvider(_blobProvider, _tempPath, "theme");
		//	Request.Content.ReadAsMultipartAsync(blobMultipartProvider).Wait();

		//	var retVal = new List<webModel.BlobInfo>();

		//	foreach (var blobInfo in blobMultipartProvider.BlobInfos)
		//	{
		//		retVal.Add(new webModel.BlobInfo
		//		{
		//			Name = blobInfo.Name,
		//			Size = blobInfo.Size.ToHumanReadableSize(),
		//			MimeType = blobInfo.ContentType,
		//			Url = blobInfo.Location
		//		});
		//	}
		//}
		#endregion
	}
}