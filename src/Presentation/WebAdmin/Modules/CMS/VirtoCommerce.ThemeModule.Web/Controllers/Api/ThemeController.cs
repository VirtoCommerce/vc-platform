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
using VirtoCommerce.Foundation.Assets.Repositories;
using VirtoCommerce.Framework.Web.Asset;
using VirtoCommerce.Framework.Web.Common;
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
		private readonly string _pathForMultipart;
		private readonly string _pathForFiles;
		#endregion

		#region Constructors and Destructors
		public ThemeController(Func<string, IThemeService> factory, ISettingsManager manager, string pathForMultipart, string pathForFiles)
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
				"VirtoCommerce.ThemeModule.MainProperties.ThemesRepositoryType",
				string.Empty);

			_pathForMultipart = pathForMultipart;
			_pathForFiles = pathForFiles;

			var themeService = factory.Invoke(chosenRepository);
			this._themeService = themeService;
		}
		#endregion

		#region Public Methods and Operators
		[HttpGet]
		[ResponseType(typeof(ThemeAsset))]
		[Route("themes/{themeId}/assets/{*assetId}")]
		public async Task<IHttpActionResult> GetThemeAsset(string assetId, string storeId, string themeId)
		{
			var item = await this._themeService.GetThemeAsset(storeId, themeId, assetId);
			return this.Ok(item.ToWebModel());
		}

		[HttpGet]
		[ResponseType(typeof(ThemeAsset[]))]
		[Route("themes/{themeId}/assets")]
		public async Task<IHttpActionResult> GetThemeAssets(string storeId, string themeId, [FromUri]GetThemeAssetsCriteria criteria)
		{
			var items = await this._themeService.GetThemeAssets(storeId, themeId, criteria.ToCoreModel());

			return this.Ok(items.OrderBy(x => x.Updated).Select(s => s.ToWebModel()).ToArray());
		}

		[HttpGet]
		[ResponseType(typeof(ThemeAssetFolder[]))]
		[Route("themes/{themeId}/folders")]
		public async Task<IHttpActionResult> GetThemeAssets(string storeId, string themeId)
		{
			var items = await this._themeService.GetThemeAssets(storeId, themeId, new VirtoCommerce.Content.Data.Models.GetThemeAssetsCriteria());

			return this.Ok(items.ToWebModel());
		}

		[HttpGet]
		[ResponseType(typeof(Theme[]))]
        [ClientCache(Duration = 30)]
		[Route("themes")]
		public async Task<IHttpActionResult> GetThemes(string storeId)
		{
			var items = await this._themeService.GetThemes(storeId);
			return this.Ok(items.Select(s => s.ToWebModel()).ToArray());
		}

		[HttpPost]
		[Route("themes/{themeId}/assets")]
		public async Task<IHttpActionResult> SaveItem(ThemeAsset asset, string storeId, string themeId)
		{
			if (!string.IsNullOrEmpty(asset.AssetUrl))
			{
				var filePath = string.Format("{0}{1}", _pathForFiles, asset.AssetUrl);
				asset.ByteContent = File.ReadAllBytes(filePath);
			}

			await this._themeService.SaveThemeAsset(storeId, themeId, asset.ToDomainModel());
			return this.Ok();
		}

		[HttpDelete]
		[Route("themes/{themeId}/assets")]
		public async Task<IHttpActionResult> DeleteAssets(string storeId, string themeId, [FromUri]string[] assetIds)
		{
			await this._themeService.DeleteThemeAssets(storeId, themeId, assetIds);
			return this.Ok();
		}

		[HttpPost]
		[Route("themes/file")]
		public async Task<IHttpActionResult> UploadThemeFile(string storeId)
		{
			if (!Request.Content.IsMimeMultipartContent())
			{
				throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
			}

			var provider = new MultipartFileStreamProvider(_pathForMultipart);

			await Request.Content.ReadAsMultipartAsync(provider);

			foreach (var file in provider.FileData)
			{
				using (ZipArchive archive = ZipFile.OpenRead(file.LocalFileName))
				{
					var fileName = Path.GetFileNameWithoutExtension(file.Headers.ContentDisposition.FileName.Replace("\"", string.Empty));
					await _themeService.UploadTheme(storeId, fileName, archive);
				}
			}

			return Ok();
		}

		[HttpPost]
		[Route("themes/{themeId}/assets/file/{folderName}")]
		public async Task<IHttpActionResult> SaveImageItem(string storeId, string themeId, string folderName)
		{
			if (!Request.Content.IsMimeMultipartContent())
			{
				throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
			}

			var provider = new MultipartFileStreamProvider(_pathForMultipart);

			await Request.Content.ReadAsMultipartAsync(provider);

			var loadItemInfo = new LoadItemInfo();

			foreach (var file in provider.FileData)
			{
				var fileInfo = new FileInfo(file.LocalFileName);
				using (FileStream stream = fileInfo.OpenRead())
				{
					var fileName = file.Headers.ContentDisposition.FileName.Replace("\"", string.Empty);
					var filePath = string.Format("{0}{1}", _pathForFiles, fileName);

					using (var f = File.Create(filePath))
					{
						await stream.CopyToAsync(f);
					}

					loadItemInfo.Name = fileName;
					loadItemInfo.ContentType = file.Headers.ContentType.MediaType;
					if (ContentTypeUtility.IsImageContentType(loadItemInfo.ContentType))
					{
						loadItemInfo.Content = ContentTypeUtility.
							ConvertImageToBase64String(File.ReadAllBytes(filePath), file.Headers.ContentType.MediaType);
					}
				}
			}

			return this.Ok(loadItemInfo);
		}
		#endregion
	}
}