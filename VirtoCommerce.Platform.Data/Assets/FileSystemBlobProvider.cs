using System;
using System.IO;
using VirtoCommerce.Platform.Core.Assets;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Exceptions;

namespace VirtoCommerce.Platform.Data.Assets
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
        /// <param name="url"></param>
        /// <returns></returns>
        public virtual BlobInfo GetBlobInfo(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");

            BlobInfo retVal = null;
            var filePath = GetStoragePathFromUrl(url);

            ValidatePath(filePath);

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
                retVal.RelativeUrl = GetRelativeUrl(retVal.Url);

            }
            return retVal;
        }

        /// <summary>
        /// Open blob for read by relative or absolute url
        /// </summary>
        /// <param name="url"></param>
        /// <returns>blob stream</returns>
        public virtual Stream OpenRead(string url)
        {
            var filePath = GetStoragePathFromUrl(url);

            ValidatePath(filePath);

            return File.Open(filePath, FileMode.Open);
        }

        /// <summary>
        /// Open blob for write by relative or absolute url
        /// </summary>
        /// <param name="url"></param>
        /// <returns>blob stream</returns>
        public virtual Stream OpenWrite(string url)
        {
            var filePath = GetStoragePathFromUrl(url);
            var folderPath = Path.GetDirectoryName(filePath);

            ValidatePath(filePath);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            return File.Open(filePath, FileMode.Create, FileAccess.Write);
        }


        /// <summary>
        /// Search folders and blobs in folder
        /// </summary>
        /// <param name="folderUrl">absolute or relative path</param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual BlobSearchResult Search(string folderUrl, string keyword)
        {
            var retVal = new BlobSearchResult();
            folderUrl = folderUrl ?? _basePublicUrl;

            var storageFolderPath = GetStoragePathFromUrl(folderUrl);

            ValidatePath(storageFolderPath);

            if (!Directory.Exists(storageFolderPath))
            {
                return retVal;
            }
            var directories = String.IsNullOrEmpty(keyword) ? Directory.GetDirectories(storageFolderPath) : Directory.GetDirectories(storageFolderPath, "*" + keyword + "*", SearchOption.AllDirectories);
            foreach (var directory in directories)
            {
                var directoryInfo = new DirectoryInfo(directory);
                var folder = new BlobFolder
                {
                    Name = Path.GetFileName(directory),
                    Url = GetAbsoluteUrlFromPath(directory),
                    ParentUrl = GetAbsoluteUrlFromPath(directoryInfo.Parent.FullName)
                };
                folder.RelativeUrl = GetRelativeUrl(folder.Url);
                retVal.Folders.Add(folder);
            }

            var files = String.IsNullOrEmpty(keyword) ? Directory.GetFiles(storageFolderPath) : Directory.GetFiles(storageFolderPath, "*" + keyword + "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);
                var blobInfo = new BlobInfo
                {
                    Url = GetAbsoluteUrlFromPath(file),
                    ContentType = MimeTypeResolver.ResolveContentType(fileInfo.Name),
                    Size = fileInfo.Length,
                    FileName = fileInfo.Name,
                    ModifiedDate = fileInfo.LastWriteTimeUtc
                };
                blobInfo.RelativeUrl = GetRelativeUrl(blobInfo.Url);
                retVal.Items.Add(blobInfo);
            }
            return retVal;
        }

        /// <summary>
        /// Create folder in file system within to base directory
        /// </summary>
        /// <param name="folder"></param>
        public virtual void CreateFolder(BlobFolder folder)
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

            ValidatePath(path);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

        }

        /// <summary>
        /// Remove folders and blobs by absolute or relative urls
        /// </summary>
        /// <param name="urls"></param>
        public virtual void Remove(string[] urls)
        {
            if (urls == null)
            {
                throw new ArgumentNullException("urls");
            }
            foreach (var url in urls)
            {
                var path = GetStoragePathFromUrl(url);

                ValidatePath(path);

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

        public virtual string GetAbsoluteUrl(string relativeUrl)
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

        protected string GetRelativeUrl(string url)
        {
            var retVal = url;
            if (!string.IsNullOrEmpty(_basePublicUrl))
            {
                retVal = url.Replace(_basePublicUrl, string.Empty);
            }
            return retVal;
        }

        protected string GetAbsoluteUrlFromPath(string path)
        {
            var retVal = _basePublicUrl + "/" + path.Replace(_storagePath, string.Empty).TrimStart('\\').Replace('\\', '/');
            return Uri.EscapeUriString(retVal);
        }

        protected string GetStoragePathFromUrl(string url)
        {
            var retVal = _storagePath;
            if (url != null)
            {
                retVal = _storagePath + "\\";
                if (!String.IsNullOrEmpty(_basePublicUrl))
                {
                    url = url.Replace(_basePublicUrl, string.Empty);
                }
                retVal += url;
                retVal = retVal.Replace("/", "\\").Replace("\\\\", "\\");
            }       
            return Uri.UnescapeDataString(retVal);
        }

        protected void ValidatePath(string path)
        {
            path = Path.GetFullPath(path);
            //Do not allow the use paths located above of  the defined storagePath folder
            //for security reason (avoid the file structure manipulation through using relative paths)
            if (!path.StartsWith(_storagePath))
            {
                throw new PlatformException($"Invalid path {path}");
            }
        }
    }
}
