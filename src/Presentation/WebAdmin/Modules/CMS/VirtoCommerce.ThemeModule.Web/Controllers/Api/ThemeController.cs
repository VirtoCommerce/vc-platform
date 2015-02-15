using VirtoCommerce.Content.Data.Models;

namespace VirtoCommerce.ThemeModule.Web.Controllers.Api
{
	#region

	using System;
	using System.Linq;
	using System.Web.Http;
	using System.Web.Http.Description;
	using VirtoCommerce.Content.Data.Repositories;
	using VirtoCommerce.Content.Data.Services;
	using VirtoCommerce.Framework.Web.Settings;
	using VirtoCommerce.ThemeModule.Web.Converters;
	using VirtoCommerce.ThemeModule.Web.Models;

	#endregion

	[RoutePrefix("api/cms/{storeId}/themes/{themeId}")]
	public class ThemeController : ApiController
	{
		#region Fields

		private readonly IThemeService _themeService;

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

		[HttpDelete]
		[Route("assets")]
		public IHttpActionResult DeleteAssets(string storeId, string themeId, string[] assetIds)
		{
			_themeService.DeleteThemeAssets(assetIds);
			return Ok();
		}

		[HttpGet]
		[ResponseType(typeof(Theme[]))]
		[Route("themes")]
		public IHttpActionResult GetThemes(string storeId)
		{
			var items = _themeService.GetThemes(storeId);
			return Ok(items.Select(s => s.ToWebModel()));
		}

		[HttpGet]
		[ResponseType(typeof(ThemeAsset))]
		[Route("assets/{*assetId}")]
		public IHttpActionResult GetThemeAsset(string assetId, string storeId, string themeId)
		{
			var item = _themeService.GetThemeAsset(assetId);
			return Ok(item.ToWebModel());
		}

		[HttpGet]
		[ResponseType(typeof(ThemeAsset[]))]
		[Route("assets")]
		public IHttpActionResult GetThemeAssets(string storeId, string themeId)
		{
			var items = _themeService.GetThemeAssets(storeId, themeId);
			return Ok(items.Select(s => s.ToWebModel()).ToArray());
		}

		[HttpPost]
		[Route("assets")]
		public IHttpActionResult SaveItem(ThemeAsset item, string storeId, string themeName)
		{
			_themeService.SaveThemeAsset(item.ToDomainModel());
			return Ok();
		}

		#endregion
	}
}