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

	[RoutePrefix("api/cms/theme")]
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

			var choosenRepository = manager.GetValue(
				"VirtoCommerce.ThemeModule.MainProperties.ThemesRepositoryType",
				string.Empty);

			var themeService = factory.Invoke(choosenRepository);
			this._themeService = themeService;
		}

		#endregion

		#region Public Methods and Operators

		[HttpDelete]
		[Route("delete")]
		public IHttpActionResult DeleteItem(ContentItem item, string storeId, string themeName)
		{
			var domainItem = item.ToDomainModel();
			this._themeService.DeleteContentItem(storeId, themeName, domainItem);
			return this.Ok();
		}

		[HttpGet]
		[ResponseType(typeof(ContentItem))]
		[Route("item")]
		public IHttpActionResult GetItem(string path, string storeId, string themeName)
		{
			var item = this._themeService.GetContentItem(storeId, themeName, path);
			return this.Ok(item.ToWebModel());
		}

		[HttpGet]
		[ResponseType(typeof(ContentItem[]))]
		[Route("items")]
		public IHttpActionResult GetItems(string path, string storeId, string themeName)
		{
			var items = this._themeService.GetContentItems(storeId, themeName, path);

			var retValItems = items.Select(i => i.ToWebModel());

			return this.Ok(retValItems.ToArray());
		}

		[HttpPost]
		[Route("save")]
		public IHttpActionResult SaveItem(ContentItem item, string storeId, string themeName)
		{
			var domainItem = item.ToDomainModel();
			this._themeService.SaveContentItem(storeId, themeName, domainItem);
			return this.Ok();
		}

		#endregion
	}
}