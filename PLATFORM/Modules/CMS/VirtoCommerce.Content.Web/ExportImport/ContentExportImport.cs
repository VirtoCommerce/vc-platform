using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using VirtoCommerce.Content.Data.Models;
using VirtoCommerce.Content.Data.Services;
using VirtoCommerce.Platform.Core.ExportImport;
using Microsoft.Practices.ObjectBuilder2;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Domain.Store.Services;
using System.Threading.Tasks;

namespace VirtoCommerce.Content.Web.ExportImport
{
	public sealed class BackupObject
	{
		public ICollection<MenuLinkList> MenuLinkLists { get; set; }
		public ICollection<ThemeAsset> ThemeAssets { get; set; }
		public ICollection<Page> Pages { get; set; }
	}

	public sealed class ContentExportImport
	{
		private readonly IMenuService _menuService;
		private readonly IStoreService _storeService;
		private readonly IThemeService _themeService;
		private readonly IPagesService _pagesService;

		public ContentExportImport(IMenuService menuService, IThemeService themeService, IPagesService pagesService, IStoreService storeService)
		{
			_menuService = menuService;
			_storeService = storeService;
			_themeService = themeService;
			_pagesService = pagesService;
		}

		public void DoExport(Stream backupStream, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
		{
			var backupObject = GetBackupObject(progressCallback, manifest.HandleBinaryData);

			backupObject.MenuLinkLists.ForEach(x => x.MenuLinks = x.MenuLinks.Where(m => m.IsActive).ToList());

			backupObject.SerializeJson(backupStream);
		}

		public void DoImport(Stream backupStream, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
		{
			var backupObject = backupStream.DeserializeJson<BackupObject>();
			var originalObject = GetBackupObject(progressCallback, manifest.HandleBinaryData);

			var progressInfo = new ExportImportProgressInfo();
			progressInfo.Description = String.Format("{0} menu link lists importing...", backupObject.MenuLinkLists.Count());
			progressCallback(progressInfo);

			UpdateMenuLinkLists(originalObject.MenuLinkLists, backupObject.MenuLinkLists);
			if (manifest.HandleBinaryData)
			{
				progressInfo.Description = String.Format("importing binary data: {0} pages importing...", backupObject.Pages.Count());
				progressCallback(progressInfo);

				UpdatePages(originalObject.Pages, backupObject.Pages);

				progressInfo.Description = String.Format("importing binary data: {0} theme assets importing...", backupObject.ThemeAssets.Count());
				progressCallback(progressInfo);

				UpdateThemeAssets(originalObject.ThemeAssets, backupObject.ThemeAssets);
			}
		}

		private void UpdateMenuLinkLists(ICollection<MenuLinkList> original, ICollection<MenuLinkList> backup)
		{
			foreach (var item in backup)
			{
				_menuService.Update(item);
			}
		}

		private void UpdatePages(ICollection<Page> original, ICollection<Page> backup)
		{
			foreach (var item in backup)
			{
				_pagesService.SavePage(GetStoreIdForPage(item), item);
			}
		}

		private void UpdateThemeAssets(ICollection<ThemeAsset> original, ICollection<ThemeAsset> backup)
		{
			foreach (var item in backup)
			{
				_themeService.SaveThemeAsset(GetStoreIdForThemeAsset(item), GetThemeIdForThemeAsset(item), item);
			}
		}

		private string GetStoreIdForPage(Page page)
		{
			return page.FullPath.Split(new char[] { '/' })[0];
		}

		private string GetThemeIdForThemeAsset(ThemeAsset themeAsset)
		{
			return themeAsset.Path.Split(new char[] { '/' })[1];
		}

		private string GetStoreIdForThemeAsset(ThemeAsset themeAsset)
		{
			return themeAsset.Path.Split(new char[] { '/' })[0];
		}

		private BackupObject GetBackupObject(Action<ExportImportProgressInfo> progressCallback, bool handleBynaryData)
		{
			var progressInfo = new ExportImportProgressInfo();
			progressInfo.Description = "cms content loading...";
			progressCallback(progressInfo);

			var stores = _storeService.GetStoreList();
			var menuLinkLists = new List<MenuLinkList>();
			var contentItems = new List<ThemeAsset>();
			var contentPages = new List<Page>();
			foreach(var store in stores)
			{
				var storeLists = _menuService.GetListsByStoreId(store.Id);
				menuLinkLists.AddRange(storeLists);

				if (handleBynaryData)
				{
					var storePages = _pagesService.GetPages(store.Id, null);
					contentPages.AddRange(storePages);

					var storeThemes = _themeService.GetThemes(store.Id).Result.ToArray();
					foreach (var storeTheme in storeThemes)
					{
						var themeContentItems = _themeService.GetThemeAssets(store.Id, storeTheme.Name, null).Result.ToArray();
						contentItems.AddRange(themeContentItems);
					}
				}
			}

			return new BackupObject
			{
				MenuLinkLists = menuLinkLists,
				ThemeAssets = contentItems,
				Pages = contentPages
			};
		}
	}
}