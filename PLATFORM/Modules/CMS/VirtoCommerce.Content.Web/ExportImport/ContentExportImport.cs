﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using webModels = VirtoCommerce.Content.Web.Models;
using VirtoCommerce.Content.Data.Services;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Content.Web.Converters;

namespace VirtoCommerce.Content.Web.ExportImport
{
    public class ContentFolder
    {
        public ContentFolder()
        {
            Folders = new List<ContentFolder>();
            Files = new List<ContentFile>();
        }
        public string Url { get; set; }
        public ICollection<ContentFolder> Folders { get; set; }
        public ICollection<ContentFile> Files { get; set; }
    }
    public class ContentFile
    {
        public string Url { get; set; }
        public byte[] Data { get; set; }
    }

    public sealed class BackupObject
    {
        public BackupObject()
        {
            MenuLinkLists = new List<webModels.MenuLinkList>();
            ContentFolders = new List<ContentFolder>();
        }
        public ICollection<webModels.MenuLinkList> MenuLinkLists { get; set; }
        public ICollection<ContentFolder> ContentFolders { get; set; }
    }

    public sealed class ContentExportImport
    {
        private static string[] _exportedFolders = { "Pages", "Themes" };
        private readonly IMenuService _menuService;
        private readonly IContentStorageProvider _contentStorageProvider;

        public ContentExportImport(IMenuService menuService, Func<IContentStorageProvider> themesStorageProviderFactory)
        {
            _menuService = menuService;
            _contentStorageProvider = themesStorageProviderFactory();
        }

        public void DoExport(Stream backupStream, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
        {
            var backupObject = GetBackupObject(progressCallback, manifest.HandleBinaryData);
            backupObject.SerializeJson(backupStream);
        }

        public void DoImport(Stream backupStream, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
        {
            var backupObject = backupStream.DeserializeJson<BackupObject>();
            var originalObject = GetBackupObject(progressCallback, false);

            var progressInfo = new ExportImportProgressInfo();
            progressInfo.Description = String.Format("{0} menu link lists importing...", backupObject.MenuLinkLists.Count());
            progressCallback(progressInfo);
            UpdateMenuLinkLists(backupObject.MenuLinkLists);

            if (manifest.HandleBinaryData)
            {
                progressInfo.Description = String.Format("importing binary data:  themes and pages importing...");
                progressCallback(progressInfo);
                foreach (var folder in backupObject.ContentFolders)
                {
                    SaveContentFolderRecursive(folder);
                }
            }
        }

        private void UpdateMenuLinkLists(ICollection<webModels.MenuLinkList> linkLIsts)
        {
            foreach (var item in linkLIsts.Select(x => x.ToCoreModel()))
            {
                _menuService.Update(item);
            }
        }



        private BackupObject GetBackupObject(Action<ExportImportProgressInfo> progressCallback, bool handleBynaryData)
        {
            var retVal = new BackupObject();

            var progressInfo = new ExportImportProgressInfo();
            progressInfo.Description = "cms content loading...";
            progressCallback(progressInfo);

            retVal.MenuLinkLists = _menuService.GetAllLinkLists().Select(x => x.ToWebModel()).ToList();

            if (handleBynaryData)
            {
                var result = _contentStorageProvider.Search("/", null);
                foreach (var blobFolder in result.Folders.Where(x => _exportedFolders.Contains(x.Name)))
                {
                    var contentFolder = new ContentFolder
                    {
                        Url = blobFolder.Url
                    };
                    ReadContentFoldersRecurive(contentFolder);
                    retVal.ContentFolders.Add(contentFolder);
                }
            }

            return retVal;
        }

        private void SaveContentFolderRecursive(ContentFolder folder)
        {
            foreach (var childFolder in folder.Folders)
            {
                SaveContentFolderRecursive(childFolder);
            }
            foreach (var folderFile in folder.Files)
            {
                using (var stream = _contentStorageProvider.OpenWrite(folderFile.Url))
                using (var memStream = new MemoryStream(folderFile.Data))
                {
                    memStream.CopyTo(stream);
                }
            }
        }

        private void ReadContentFoldersRecurive(ContentFolder folder)
        {
            var result = _contentStorageProvider.Search(folder.Url, null);
            foreach (var blobFolder in result.Folders)
            {
                //Do not export default theme its will distributed with code
                if (blobFolder.Url != "/Themes/default")
                {
                    var contentFolder = new ContentFolder()
                    {
                        Url = blobFolder.Url
                    };
                    ReadContentFoldersRecurive(contentFolder);
                    folder.Folders.Add(contentFolder);
                }
            }

            foreach (var blobItem in result.Items)
            {
                var contentFile = new ContentFile
                {
                    Url = blobItem.Url
                };
                using (var stream = _contentStorageProvider.OpenRead(blobItem.Url))
                {
                    contentFile.Data = stream.ReadFully();
                }
                folder.Files.Add(contentFile);
            }
        }


    }

}
