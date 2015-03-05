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
		public ThemeController(Func<string, IThemeService> factory, ISettingsManager manager)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}

			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}

			var chosenRepository = manager.GetValue(
				"VirtoCommerce.ThemeModule.MainProperties.ThemesRepositoryType",
				string.Empty);

			var themeService = factory.Invoke(chosenRepository);
			this._themeService = themeService;
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

		[HttpPost]
		[Route("themes/{themeId}/assets/image")]
		public IHttpActionResult SaveImageItem(string storeId, string themeId)
		{
			if (!Request.Content.IsMimeMultipartContent())
			{
				throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
			}

			var content = Request.Content.ReadAsByteArrayAsync().Result;
			var asset = ThemeAssetConverter.DomainModel("", "", content);

			this._themeService.SaveThemeAsset(storeId, themeId, asset);
			return this.Ok();
		}
		#endregion
	}
}