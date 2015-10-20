#region
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
using VirtoCommerce.Content.Data.Services;
using VirtoCommerce.Content.Data.Utility;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Content.Web.Converters;
using VirtoCommerce.Content.Web.Models;
using VirtoCommerce.Platform.Core.PushNotifications;
using VirtoCommerce.Content.Web.Security;

#endregion

namespace VirtoCommerce.Content.Web.Controllers.Api
{
	[RoutePrefix("api/cms/{storeId}")]
	public class ThemeController : ContentBaseController
    {
		#region Fields
		private readonly IThemeService _themeService;
		private readonly string _pathForMultipart;
		private readonly string _pathForFiles;
		private readonly string _defaultThemePath;
        #endregion

        #region Constructors and Destructors
        public ThemeController(Func<string, IThemeService> factory, ISettingsManager manager, ISecurityService securityService, IPermissionScopeService permissionScopeService,
                               string pathForMultipart, string pathForFiles, string defaultThemePath)
            : base(securityService, permissionScopeService)
		{
			if (factory == null)
			{
				throw new ArgumentNullException("factory");
			}

			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}

			if (string.IsNullOrEmpty(pathForMultipart))
				throw new ArgumentNullException("pathForMultipart");

			if (string.IsNullOrEmpty(pathForFiles))
				throw new ArgumentNullException("pathForFiles");

			var chosenRepository = manager.GetValue(
				"VirtoCommerce.Content.MainProperties.ThemesRepositoryType",
				string.Empty);

			_pathForMultipart = pathForMultipart;
			_pathForFiles = pathForFiles;
			_defaultThemePath = defaultThemePath;

            var themeService = factory.Invoke(chosenRepository);
			this._themeService = themeService;
		}
        #endregion

        /// <summary>
        /// Get theme asset
        /// </summary>
        /// <remarks>Get theme asset by store id, theme id and asset id. Asset id - asset path relative to root theme path</remarks>
        /// <param name="storeId">Store id</param>
        /// <param name="themeId">Theme id</param>
        /// <param name="assetId">Theme asset id</param>
        /// <response code="200"></response>
        /// <response code="404">Theme asset not found</response>
        [HttpGet]
		[ResponseType(typeof(ThemeAsset))]
		[Route("themes/{themeId}/assets/{*assetId}")]
		public IHttpActionResult GetThemeAsset(string assetId, string storeId, string themeId)
		{
            base.CheckCurrentUserHasPermissionForObjects(ContentPredefinedPermissions.Read, new ContentScopeObject { StoreId = storeId });
			var item = this._themeService.GetThemeAsset(storeId, themeId, assetId);
			if (item != null)
			{
                var retVal = item.ToWebModel();
                retVal.SecurityScopes = base.GetObjectPermissionScopeStrings(new ContentScopeObject { StoreId = storeId });
				return this.Ok(retVal);
			}

			return NotFound();
		}

		/// <summary>
		/// Search theme assets
		/// </summary>
		/// <remarks>Search theme assets by store id, theme id and criteria</remarks>
		/// <param name="storeId">Store id</param>
		/// <param name="themeId">Theme id</param>
		/// <param name="criteria">Searching theme assets criteria</param>
		[HttpGet]
		[ResponseType(typeof(ThemeAsset[]))]
		[Route("themes/{themeId}/assets")]
		public IHttpActionResult SearchThemeAssets(string storeId, string themeId, [FromUri]GetThemeAssetsCriteria criteria)
		{
            base.CheckCurrentUserHasPermissionForObjects(ContentPredefinedPermissions.Read, new ContentScopeObject { StoreId = storeId });

            var items = this._themeService.GetThemeAssets(storeId, themeId, criteria.ToCoreModel());

			return this.Ok(items.OrderBy(x => x.Updated).Select(s =>
            {
                var result = s.ToWebModel();
                result.SecurityScopes = base.GetObjectPermissionScopeStrings(new ContentScopeObject { StoreId = storeId });
                return result;

            }).ToArray());
		}

		/// <summary>
		/// Delete theme
		/// </summary>
		/// /// <remarks>Search theme assets by store id and theme id</remarks>
		/// <param name="storeId">Store id</param>
		/// <param name="themeId">Theme id</param>
		[HttpDelete]
		[ResponseType(typeof(void))]
		[Route("themes/{themeId}")]
		public IHttpActionResult DeleteTheme(string storeId, string themeId)
		{
            base.CheckCurrentUserHasPermissionForObjects(ContentPredefinedPermissions.Delete, new ContentScopeObject { StoreId = storeId });

            this._themeService.DeleteTheme(storeId, themeId);

			return this.Ok();
		}

		/// <summary>
		/// Get theme assets folders
		/// </summary>
		/// <remarks>Get theme assets folders by store id and theme id</remarks>
		/// <param name="storeId">Store id</param>
		/// <param name="themeId">Theme id</param>
		[HttpGet]
		[ResponseType(typeof(ThemeAssetFolder[]))]
		[Route("themes/{themeId}/folders")]
		public IHttpActionResult GetThemeAssets(string storeId, string themeId)
		{
            base.CheckCurrentUserHasPermissionForObjects(ContentPredefinedPermissions.Read, new ContentScopeObject { StoreId = storeId });

            var items = this._themeService.GetThemeAssets(storeId, themeId, null);

			return this.Ok(items.ToWebModel());
		}

		/// <summary>
		/// Get themes by store id
		/// </summary>
		/// <param name="storeId">Store id</param>
		[HttpGet]
		[ResponseType(typeof(Theme[]))]
		[Route("themes")]
		public IHttpActionResult GetThemes(string storeId)
		{
            base.CheckCurrentUserHasPermissionForObjects(ContentPredefinedPermissions.Read, new ContentScopeObject { StoreId = storeId });

            var items = this._themeService.GetThemes(storeId);
			return this.Ok(items.Select(s => s.ToWebModel()).ToArray());
		}

		/// <summary>
		/// Save theme asset
		/// </summary>
		/// <remarks>Save theme asset considering store id and theme id</remarks>
		/// <param name="storeId">Store id</param>
		/// <param name="themeId">Theme id</param>
		/// <param name="asset">Theme asset</param>
		[HttpPost]
		[Route("themes/{themeId}/assets")]
		[ResponseType(typeof(void))]
		public IHttpActionResult SaveItem(ThemeAsset asset, string storeId, string themeId)
		{
            base.CheckCurrentUserHasPermissionForObjects(ContentPredefinedPermissions.Update, new ContentScopeObject { StoreId = storeId });

            if (!string.IsNullOrEmpty(asset.AssetUrl))
			{
                using (var webClient = new WebClient())
                {
                    asset.ByteContent = webClient.DownloadData(asset.AssetUrl);
                }
			}

			this._themeService.SaveThemeAsset(storeId, themeId, asset.ToDomainModel());
			return this.Ok();
		}

		/// <summary>
		/// Delete theme assets by assetIds
		/// </summary>
		/// <remarks>Delete theme assets considering store id, theme id and assetIds</remarks>
		/// <param name="storeId">Store id</param>
		/// <param name="themeId">Theme id</param>
		/// <param name="assetIds">Deleted asset ids</param>
		[HttpDelete]
		[Route("themes/{themeId}/assets")]
		[ResponseType(typeof(void))]
		public IHttpActionResult DeleteAssets(string storeId, string themeId, [FromUri]string[] assetIds)
		{
            base.CheckCurrentUserHasPermissionForObjects(ContentPredefinedPermissions.Delete, new ContentScopeObject { StoreId = storeId });

            this._themeService.DeleteThemeAssets(storeId, themeId, assetIds);
			return this.Ok();
		}

		/// <summary>
		/// Create new theme
		/// </summary>
		/// <remarks>Create new theme considering store id, theme file url and theme name</remarks>
		/// <param name="storeId">Store id</param>
		/// <param name="themeFileUrl">Theme file url</param>
		/// <param name="themeName">Theme name</param>
		[HttpGet]
		[Route("themes/file")]
		[ResponseType(typeof(void))]
		[CheckPermission(Permission = ContentPredefinedPermissions.Create)]
		public IHttpActionResult CreateNewTheme(string storeId, string themeFileUrl, string themeName)
		{
            base.CheckCurrentUserHasPermissionForObjects(ContentPredefinedPermissions.Create, new ContentScopeObject { StoreId = storeId });

            using (var webClient = new WebClient())
			{
				var filePath = string.Format("~/App_Data/Uploads/{0}.zip", Guid.NewGuid().ToString());
				var fullFilePath = HostingEnvironment.MapPath(filePath);

                try
                {
                    webClient.DownloadFile(new Uri(themeFileUrl), fullFilePath);
                }
                catch(WebException ex)
                {
                    var response = ex.Response as HttpWebResponse;
                    if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                    {
                        throw new HttpResponseException(new HttpResponseMessage(response.StatusCode)
                        {
                            ReasonPhrase = ex.Message
                        });
                    }
                }
				using (ZipArchive archive = ZipFile.OpenRead(fullFilePath))
				{
					_themeService.UploadTheme(storeId, themeName, archive);
				}

				File.Delete(fullFilePath);
			}

			return Ok();
		}

		/// <summary>
		/// Create default theme by store id
		/// </summary>
		/// <param name="storeId">Store id</param>
		[HttpGet]
		[Route("themes/createdefault")]
		[ResponseType(typeof(void))]
		public IHttpActionResult CreateDefaultTheme(string storeId)
		{
            base.CheckCurrentUserHasPermissionForObjects(ContentPredefinedPermissions.Create, new ContentScopeObject { StoreId = storeId });

            _themeService.CreateDefaultTheme(storeId, _defaultThemePath);
			
			return Ok();
		}
	}
}