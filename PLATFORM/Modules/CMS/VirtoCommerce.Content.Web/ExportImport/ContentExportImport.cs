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

namespace VirtoCommerce.Content.Web.ExportImport
{
	public sealed class BackupObject
	{
		public ICollection<MenuLinkList> MenuLinkLists { get; set; }
	}

	public sealed class ContentExportImport
	{
		private readonly IMenuService _menuService;
		private readonly IStoreService _storeService;

		public ContentExportImport(IMenuService menuService, IStoreService storeService)
		{
			_menuService = menuService;
			_storeService = storeService;
		}

		public void DoExport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
		{
			var backupObject = GetBackupObject(progressCallback);

			backupObject.MenuLinkLists.ForEach(x => x.MenuLinks = x.MenuLinks.Where(m => m.IsActive).ToList());

			backupObject.SerializeJson(backupStream);
		}

		public void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
		{
			var backupObject = backupStream.DeserializeJson<BackupObject>();
			var originalObject = GetBackupObject(progressCallback);

			var progressInfo = new ExportImportProgressInfo();
			progressInfo.Description = String.Format("{0} menu link lists importing...", backupObject.MenuLinkLists.Count());
			progressCallback(progressInfo);

			UpdateStores(originalObject.MenuLinkLists, backupObject.MenuLinkLists);
		}

		private void UpdateStores(ICollection<MenuLinkList> original, ICollection<MenuLinkList> backup)
		{
			var toUpdate = new List<MenuLinkList>();

			backup.CompareTo(original, EqualityComparer<MenuLinkList>.Default, (state, x, y) =>
			{
				switch (state)
				{
					case EntryState.Modified:
						toUpdate.Add(x);
						break;
					case EntryState.Added:
						_menuService.Update(x);
						break;
				}
			});

			foreach (var item in toUpdate)
			{
				_menuService.Update(item);
			}
		}

		private BackupObject GetBackupObject(Action<ExportImportProgressInfo> progressCallback)
		{
			var progressInfo = new ExportImportProgressInfo();
			progressInfo.Description = "menu link lists loading...";
			progressCallback(progressInfo);

			var stores = _storeService.GetStoreList();
			var menuLinkLists = new List<MenuLinkList>();
			foreach(var store in stores)
			{
				var storeLists = _menuService.GetListsByStoreId(store.Id);
				foreach(var storeList in storeLists)
				{
					menuLinkLists.Add(storeList);
				}
			}

			return new BackupObject
			{
				MenuLinkLists = menuLinkLists
			};
		}
	}
}