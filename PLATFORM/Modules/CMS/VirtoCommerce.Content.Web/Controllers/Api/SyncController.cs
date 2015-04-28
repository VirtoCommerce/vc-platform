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
using VirtoCommerce.Platform.Core.Settings;
using GetPagesCriteria = VirtoCommerce.Content.Data.Models.GetPagesCriteria;
using GetThemeAssetsCriteria = VirtoCommerce.Content.Data.Models.GetThemeAssetsCriteria;
using ThemeAsset = VirtoCommerce.Content.Web.Models.ThemeAsset;

namespace VirtoCommerce.Content.Web.Controllers.Api
{
    [RoutePrefix("api/cms/sync/stores/{storeId}")]
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

        [HttpGet]
        [ResponseType(typeof(SyncAsset[]))]
        [Route("assets")]
        public async Task<IHttpActionResult> SyncAssets(string storeId, string theme, DateTime since)
        {
            var themeItems = await this._themeService.GetThemeAssets(storeId, theme, new GetThemeAssetsCriteria() { LoadContent = true, LastUpdateDate = since});
            var pageItems = _pagesService.GetPages(storeId, new GetPagesCriteria(){ LastUpdateDate = since}).Select(s => s.ToWebModel());

            var assets = themeItems.Select(s => s.ToWebModel()).Select(x=>x.ToSyncModel(theme));
            assets = assets.Concat(pageItems.Select(x => x.ToSyncModel()));

            return Ok(assets.OrderBy(x=>x.Updated));
        }
    }
}