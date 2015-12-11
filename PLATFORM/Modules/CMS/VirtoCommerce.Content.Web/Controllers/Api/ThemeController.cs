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
using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Asset;
using System.Text;

#endregion

namespace VirtoCommerce.Content.Web.Controllers.Api
{
    [RoutePrefix("api/cms/{storeId}")]
    public class ThemeController : ContentBaseController
    {
        private readonly IThemeStorageProvider _themeProvider;

        #region Constructors and Destructors
        public ThemeController(IThemeStorageProvider themeProvider, ISecurityService securityService, IPermissionScopeService permissionScopeService)
            : base(securityService, permissionScopeService)
        {
            _themeProvider = themeProvider;

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

            var blobItem = _themeProvider.GetBlobInfo("/" + storeId + "/" + themeId + "/" + assetId);

            if (blobItem != null)
            {
                var themeAsset = blobItem.ToThemeAssetWebModel();
                themeAsset.Id = assetId;
                using (var stream = _themeProvider.OpenRead(blobItem.Url))
                {
                    var data = stream.ReadFully();
                    if (ContentTypeUtility.IsImageContentType(themeAsset.ContentType))
                    {
                        themeAsset.ByteContent = data;
                    }
                    else if (ContentTypeUtility.IsTextContentType(themeAsset.ContentType))
                    {
                        themeAsset.Content = Encoding.UTF8.GetString(data);
                    }
                }
                return Ok(themeAsset);
            }
            return NotFound();

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
            _themeProvider.Remove(new string[] { "/" + storeId + "/" + themeId });

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

            var retVal = new List<ThemeAssetFolder>();
            var result = _themeProvider.Search("/" + storeId + "/" + themeId, null);
            foreach (var folder in result.Folders)
            {
                var themeFolder = folder.ToThemeFolderWebModel();
                themeFolder.Assets.AddRange(LoadFolderAssetRecursive(folder, folder.Name));
                retVal.Add(themeFolder);
            }
            return Ok(retVal.ToArray());
        }

        /// <summary>
        /// Get themes by store id
        /// </summary>
        /// <param name="storeId">Store id</param>
        [HttpGet]
        [ResponseType(typeof(Theme[]))]
        [Route("themes")]
        [ClientCache(Duration = 60)]
        public IHttpActionResult GetThemes(string storeId)
        {
            base.CheckCurrentUserHasPermissionForObjects(ContentPredefinedPermissions.Read, new ContentScopeObject { StoreId = storeId });
            var result = _themeProvider.Search("/" + storeId, null);
            return Ok(result.Folders.Select(x => x.ToThemeWebModel()).ToArray());
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

            var data = asset.ByteContent;

            if (!string.IsNullOrEmpty(asset.AssetUrl))
            {
                using (var webClient = new WebClient())
                {
                    data = webClient.DownloadData(asset.AssetUrl);
                }
            }
            else if (data == null)
            {
                data = Encoding.UTF8.GetBytes(asset.Content);
            }
            using (var stream = _themeProvider.OpenWrite("/" + storeId + "/" + themeId + "/" + asset.Id))
            using (var memStream = new MemoryStream(data))
            {
                memStream.CopyTo(stream);
            }

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
            _themeProvider.Remove(assetIds.Select(x=> "/" + storeId + "/" + themeId + "/" + x).ToArray());
       
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
                using (var stream = webClient.OpenRead(new Uri(themeFileUrl)))
                using (ZipArchive archive = new ZipArchive(stream))
                {
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (!entry.FullName.EndsWith("/"))
                        {
                            var fileName = String.Join("/", entry.FullName.Split('/').Skip(1));
                            using (var entryStream = entry.Open())
                            using (var targetStream = _themeProvider.OpenWrite("/" + storeId + "/" + fileName))
                            {
                                entryStream.CopyTo(targetStream);
                            }
                        }
                    }
                }
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

            CopyRecursive("default", storeId + "/default");

            return Ok();
        }

        private void CopyRecursive(string sourcePath, string tragetPath)
        {
            var result = _themeProvider.Search(sourcePath, null);
            foreach (var blobFolder in result.Folders)
            {
                CopyRecursive(sourcePath + "/" + blobFolder.Name, tragetPath + "/" + blobFolder.Name);
            }
            foreach (var blobItem in result.Items)
            {
                using (var sourceStream = _themeProvider.OpenRead(blobItem.Url))
                using (var targetStream = _themeProvider.OpenWrite(tragetPath + "/" + blobItem.FileName))
                {
                    sourceStream.CopyTo(targetStream);
                }
            }
        }
        private ThemeAsset[] LoadFolderAssetRecursive(BlobFolder blobFolder, string path, int level = 0)
        {
            var retVal = new List<ThemeAsset>();

            var result = _themeProvider.Search(blobFolder.Url, null);
            foreach (var childFolder in result.Folders)
            {
                retVal.AddRange(LoadFolderAssetRecursive(childFolder, path + "/" + childFolder.Name, level + 1));
            }
            foreach (var item in result.Items)
            {
                var themeAssetItem = item.ToThemeAssetWebModel();
                themeAssetItem.Id = path + "/" + item.FileName;
                if (level > 0)
                {
                    themeAssetItem.Name = blobFolder.Name + "/" + themeAssetItem.Name;
                }
                retVal.Add(themeAssetItem);
            }
            return retVal.ToArray();
        }



    }
}