using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Content.Data.Services;
using VirtoCommerce.Content.Web.Converters;
using VirtoCommerce.Content.Web.Models;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Settings;
using GetPagesCriteria = VirtoCommerce.Content.Data.Models.GetPagesCriteria;
using GetThemeAssetsCriteria = VirtoCommerce.Content.Data.Models.GetThemeAssetsCriteria;
using ThemeAsset = VirtoCommerce.Content.Web.Models.ThemeAsset;

namespace VirtoCommerce.Content.Web.Controllers.Api
{
    [RoutePrefix("api/cms/sync")]
    public class SyncController : ApiController
    {
        #region Fields
		private readonly IThemeService _themeService;
        private readonly IPagesService _pagesService;
		#endregion

		#region Constructors and Destructors
        public SyncController(Func<string, IThemeService> themeFactory, Func<string, IPagesService> pageFactory, ISettingsManager manager)
		{
			if (themeFactory == null)
			{
				throw new ArgumentNullException("factory");
			}

			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			var chosenThemeRepository = manager.GetValue(
				"VirtoCommerce.Content.MainProperties.ThemesRepositoryType",
				string.Empty);

            var chosenPagesRepository = manager.GetValue(
                "VirtoCommerce.Content.MainProperties.PagesRepositoryType",
                string.Empty);

            this._themeService = themeFactory.Invoke(chosenThemeRepository);
            this._pagesService = pageFactory.Invoke(chosenPagesRepository);
		}
		#endregion

		/// <summary>
		/// Sync assets elements
		/// </summary>
		/// <remarks>
		/// Method allows synchronize asset elements(theme assets and pages). For synchronization used store id, theme id and last theme and pages update date.
		/// If last update dates = null, returns all pages or theme assets for that store and theme.
		/// </remarks>
		/// <param name="storeId">Store id</param>
		/// <param name="theme">Theme name</param>
		/// <param name="themeUpdated">Last theme updated date</param>
		/// <param name="pagesUpdated">Last pages updated date</param>
        [HttpGet]
        [ResponseType(typeof(SyncAssetGroup[]))]
        [Route("stores/{storeId}/assets")]
        [ClientCache(Duration = 60)]
        public IHttpActionResult SyncAssets(string storeId, string theme, DateTime? themeUpdated, DateTime? pagesUpdated)
        {
            var themeItems = this._themeService.GetThemeAssets(storeId, theme, new GetThemeAssetsCriteria() { LoadContent = true, LastUpdateDate = themeUpdated});
            var pageItems = _pagesService.GetPages(storeId, new GetPagesCriteria() { LastUpdateDate = pagesUpdated }).Select(s => s.ToWebModel());

            var themeGroup = new SyncAssetGroup { Type = "theme", Assets = themeItems.Select(s => s.ToWebModel()).Select(x => x.ToSyncModel()).OrderBy(x => x.Updated).ToArray() };
            var pageGroup = new SyncAssetGroup { Type = "pages", Assets = pageItems.Select(x => x.ToSyncModel()).OrderBy(x => x.Updated).ToArray() };

            return Ok(new [] { themeGroup, pageGroup });
        }
    }
}