using System;
using System.Globalization;
using System.IO;
using System.Web.Hosting;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Common;


namespace VirtoCommerce.Platform.Data.Asset
{
    public class FileSystemBlobProvider : IBlobStorageProvider, IBlobUrlResolver
    {
        public const string ProviderName = "LocalStorage";

        private readonly string _storagePath;
        private readonly string _basePublicUrl;

        public FileSystemBlobProvider(string storagePath, string publicUrl = "")
        {
            if (storagePath == null)
            {
                throw new ArgumentNullException("storagePath");
            }

            _storagePath = storagePath.TrimEnd('\\');

            _basePublicUrl = publicUrl;
            if (_basePublicUrl != null)
            {
                _basePublicUrl = _basePublicUrl.TrimEnd('/');
            }
        }

        #region IBlobStorageProvider members
        /// <summary>
        /// Get blog info by url
        /// </summary>
        /// <param name="blobUrl"></param>
        /// <returns></returns>
        public BlobInfo GetBlobInfo(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");

            BlobInfo retVal = null;
            var filePath = GetStoragePathFromUrl(url);
            if (File.Exists(filePath))
            {
                var fileInfo = new FileInfo(filePath);
                retVal = new BlobInfo
                {
                    Url = GetAbsoluteUrlFromPath(filePath),
                    ContentType = MimeTypeResolver.ResolveContentType(fileInfo.Name),
                    Size = fileInfo.Length,
                    FileName = fileInfo.Name,
                    ModifiedDate = fileInfo.LastWriteTimeUtc
                };
            }
            return retVal; 
        }

        /// <summary>
        /// Open blob for read by relative or absolute url
        /// </summary>
        /// <param name="url"></param>
        /// <returns>blob stream</returns>
        public Stream OpenRead(string url)
        {
            var filePath = GetStoragePathFromUrl(url);
            return File.Open(filePath, FileMode.Open);
        }

        /// <summary>
        /// Open blob for write by relative or absolute url
        /// </summary>
        /// <param name="url"></param>
        /// <returns>blob stream</returns>
        public Stream OpenWrite(string url)
        {
            var filePath = GetStoragePathFromUrl(url);
            var folderPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            return File.Open(filePath, FileMode.Create);
        }


        /// <summary>
        /// Search folders and blobs in folder
        /// </summary>
        /// <param name="folderUrl">absolute or relative path</param>
        /// <returns></returns>
        public BlobSearchResult Search(string folderUrl, string keyword)
        {
            var retVal = new BlobSearchResult();
            var storagePath = _storagePath;
            folderUrl = folderUrl ?? _basePublicUrl;

            var storageFolderPath = GetStoragePathFromUrl(folderUrl);
            if(!Directory.Exists(storageFolderPath))
            {
                return retVal;
            }
            var directories = String.IsNullOrEmpty(keyword) ? Directory.GetDirectories(storageFolderPath) : Directory.GetDirectories(storageFolderPath, "*" + keyword + "*", SearchOption.AllDirectories);
            foreach (var directory in directories)
            {
                var directoryInfo = new DirectoryInfo(directory);
                retVal.Folders.Add(new BlobFolder
                {
                    Name = Path.GetFileName(directory),
                    Url = GetAbsoluteUrlFromPath(directory),
                    ParentUrl = GetAbsoluteUrlFromPath(directoryInfo.Parent.FullName)
                });
            }

            var files = String.IsNullOrEmpty(keyword) ? Directory.GetFiles(storageFolderPath) : Directory.GetFiles(storageFolderPath, "*" + keyword + "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                retVal.Items.Add(new BlobInfo
                {
                    Url = GetAbsoluteUrlFromPath(file),
                    ContentType = MimeTypeResolver.ResolveContentType(fileInfo.Name),
                    Size = fileInfo.Length,
                    FileName = fileInfo.Name,
                    ModifiedDate = fileInfo.LastWriteTimeUtc
                });
            }
            return retVal;
        }

        /// <summary>
        /// Create folder in file system within to base directory
        /// </summary>
        /// <param name="folder"></param>
        public void CreateFolder(BlobFolder folder)
        {
            if (folder == null)
            {
                throw new ArgumentNullException("folder");
            }
            var path = _storagePath;
            if (folder.ParentUrl != null)
            {
                path = GetStoragePathFromUrl(folder.ParentUrl);
            }
            path = Path.Combine(path, folder.Name);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

        }

        /// <summary>
        /// Remove folders and blobs by absolute or relative urls
        /// </summary>
        /// <param name="urls"></param>
        public void Remove(string[] urls)
        {
            if (urls == null)
            {
                throw new ArgumentNullException("urls");
            }
            foreach (var url in urls)
            {
                var path = GetStoragePathFromUrl(url);
                // get the file attributes for file or directory
                var attr = File.GetAttributes(path);

                //detect whether its a directory or file
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    Directory.Delete(path, true);
                }
                else
                {
                    File.Delete(path);
                }
            }
        }

        #endregion

        #region IBlobUrlResolver Members

        public string GetAbsoluteUrl(string relativeUrl)
        {
            if (relativeUrl == null)
            {
                throw new ArgumentNullException("relativeUrl");
            }
            var retVal = relativeUrl;
            if (!relativeUrl.IsAbsoluteUrl())
            {
                retVal = _basePublicUrl + "/" + relativeUrl.TrimStart('/').TrimEnd('/');
            }
            return new Uri(retVal).ToString();
        }

        #endregion

        private string GetAbsoluteUrlFromPath(string path)
        {
            var retVal = _basePublicUrl + "/" + path.Replace(_storagePath, String.Empty).TrimStart('\\').Replace('\\', '/');
            return Uri.EscapeUriString(retVal);
        }

        private string GetStoragePathFromUrl(string url)
        {
            var retVal = _storagePath;
            if (url != null)
            {
                retVal = _storagePath + "\\";
                if (!String.IsNullOrEmpty(_basePublicUrl))
                {
                    url = url.Replace(_basePublicUrl, String.Empty);
                }
                retVal += url;
                retVal = retVal.Replace("/", "\\");
            }
            return Uri.UnescapeDataString(retVal);
        }

    }
}
