using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using VirtoCommerce.Content.Data.Converters;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Content.Data.Repositories;
using VirtoCommerce.Content.Data.Utility;
using VirtoCommerce.Foundation.Assets.Repositories;

namespace VirtoCommerce.Content.Data.Services
{
	public class ThemeServiceImpl : IThemeService
	{
		private readonly object _lockObject = new object();
		private readonly IFileRepository _repository;
		private readonly IBlobStorageProvider _blobProvider;
		private readonly string _tempPath;

		public ThemeServiceImpl(IFileRepository repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");

			_repository = repository;
		}

		public ThemeServiceImpl(IFileRepository repository, IBlobStorageProvider blobProvider, string tempPath)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");

			if (blobProvider == null)
				throw new ArgumentNullException("blobProvider");

			_repository = repository;
			_blobProvider = blobProvider;
			_tempPath = HostingEnvironment.MapPath("~/App_Data/Uploads/");
		}

		public Task<IEnumerable<Theme>> GetThemes(string storeId)
		{
			var themePath = GetThemePath(storeId, string.Empty);

			var items = _repository.GetThemes(themePath);
			return items;
		}

		public async Task DeleteTheme(string storeId, string themeId)
		{
			var themePath = GetThemePath(storeId, themeId);

			await _repository.DeleteTheme(themePath);
		}

		public async Task<IEnumerable<ThemeAsset>> GetThemeAssets(string storeId, string themeName, GetThemeAssetsCriteria criteria)
		{
			var themePath = GetThemePath(storeId, themeName);
			var items = await _repository.GetContentItems(themePath, criteria);

			foreach (var item in items)
			{
				item.Path = FixPath(themePath, item.Path);
				item.ContentType = ContentTypeUtility.GetContentType(item.Name, item.ByteContent);
			}

			return items.Select(c => c.AsThemeAsset());
		}

		public async Task<ThemeAsset> GetThemeAsset(string storeId, string themeId, string path)
		{
			var fullPath = GetFullPath(storeId, themeId, path);
			var item = await _repository.GetContentItem(fullPath);

			item.Path = FixPath(GetThemePath(storeId, themeId), item.Path);
			item.ContentType = ContentTypeUtility.GetContentType(item.Name, item.ByteContent);

			return item.AsThemeAsset();
		}

		public async Task SaveThemeAsset(string storeId, string themeId, Models.ThemeAsset asset)
		{
			var retVal = false;

			var fullPath = GetFullPath(storeId, themeId, asset.Id);

			retVal = await _repository.SaveContentItem(fullPath, asset.AsContentItem());
		}

		public async Task DeleteThemeAssets(string storeId, string themeId, params string[] assetIds)
		{
			foreach (var assetId in assetIds)
			{
				var fullPath = GetFullPath(storeId, themeId, assetId);

				await _repository.DeleteContentItem(fullPath);
			}
		}

		private string GetStorePath(string storeId)
		{
			return string.Format("{0}/", storeId);
		}

		private string GetThemePath(string storeId, string themeName)
		{
			if (string.IsNullOrEmpty(themeName))
			{
				return string.Format("{0}", storeId);
			}
			return string.Format("{0}/{1}", storeId, themeName);
		}

		private string GetFullPath(string storeId, string themeName, string path)
		{
			return string.Format("{0}/{1}/{2}", storeId, themeName, path);
		}

		private string FixPath(string themePath, string path)
		{
			return path.ToLowerInvariant().Replace(themePath.ToLowerInvariant(), string.Empty).Trim('/');
		}

		private static byte[] ReadFully(Stream input)
		{
			byte[] buffer = new byte[16 * 1024];
			using (MemoryStream ms = new MemoryStream())
			{
				int read;
				while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
				{
					ms.Write(buffer, 0, read);
				}
				return ms.ToArray();
			}
		}

		private bool IsFolder(ZipArchiveEntry entry)
		{
			return entry.FullName.EndsWith("/");
		}

		public async Task UploadTheme(string storeId, string themeName, System.IO.Compression.ZipArchive archive)
		{
			foreach (ZipArchiveEntry entry in archive.Entries)
			{
				if (!IsFolder(entry))
				{
					using (var stream = entry.Open())
					{
						var asset = new ThemeAsset
						{
							AssetName = entry.FullName.Replace(themeName, ""),
							Id = entry.FullName.Replace(themeName, "")
						};

						var arr = ReadFully(stream);
						asset.ByteContent = arr;
						asset.ContentType = ContentTypeUtility.GetContentType(entry.FullName, arr);

						await SaveThemeAsset(storeId, themeName.Trim('/'), asset);
					}
				}
			}
		}
	}
}
