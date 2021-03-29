using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Assets;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.Exceptions;

namespace VirtoCommerce.Platform.Assets.FileSystem
{
    public class FileSystemBlobProvider : IBlobStorageProvider, IBlobUrlResolver
    {
        public const string ProviderName = "LocalStorage";

        private readonly string _storagePath;
        private readonly string _basePublicUrl;

        public FileSystemBlobProvider(IOptions<FileSystemBlobOptions> options)
        {
            // extra replace step to prevent windows path getting into linux environment
            _storagePath = options.Value.RootPath.TrimEnd(Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
            _basePublicUrl = options.Value.PublicUrl;
            _basePublicUrl = _basePublicUrl?.TrimEnd('/');
        }

        #region IBlobStorageProvider members

        /// <summary>
        /// Get blog info by url
        /// </summary>
        /// <param name="blobUrl"></param>
        /// <returns></returns>
        public virtual Task<BlobInfo> GetBlobInfoAsync(string blobUrl)
        {
            if (string.IsNullOrEmpty(blobUrl))
            {
                throw new ArgumentNullException(nameof(blobUrl));
            }

            BlobInfo retVal = null;
            var filePath = GetStoragePathFromUrl(blobUrl);

            ValidatePath(filePath);

            if (File.Exists(filePath))
            {
                var fileInfo = new FileInfo(filePath);

                retVal = AbstractTypeFactory<BlobInfo>.TryCreateInstance();
                retVal.Url = GetAbsoluteUrlFromPath(filePath);
                retVal.ContentType = MimeTypeResolver.ResolveContentType(fileInfo.Name);
                retVal.Size = fileInfo.Length;
                retVal.Name = fileInfo.Name;
                retVal.ModifiedDate = fileInfo.LastWriteTimeUtc;
                retVal.RelativeUrl = GetRelativeUrl(retVal.Url);
            }

            return Task.FromResult(retVal);
        }

        /// <summary>
        /// Open blob for read by relative or absolute url
        /// </summary>
        /// <param name="blobUrl"></param>
        /// <returns>blob stream</returns>
        public virtual Stream OpenRead(string blobUrl)
        {
            var filePath = GetStoragePathFromUrl(blobUrl);

            ValidatePath(filePath);

            return File.Open(filePath, FileMode.Open, FileAccess.Read);
        }

        /// <summary>
        /// Open blob for write by relative or absolute url
        /// </summary>
        /// <param name="blobUrl"></param>
        /// <returns>blob stream</returns>
        public virtual Stream OpenWrite(string blobUrl)
        {
            var filePath = GetStoragePathFromUrl(blobUrl);
            var folderPath = Path.GetDirectoryName(filePath);

            ValidatePath(filePath);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            return File.Open(filePath, FileMode.Create);
        }

        /// <summary>
        /// SearchAsync folders and blobs in folder
        /// </summary>
        /// <param name="folderUrl">absolute or relative path</param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual Task<BlobEntrySearchResult> SearchAsync(string folderUrl, string keyword)
        {
            var retVal = AbstractTypeFactory<BlobEntrySearchResult>.TryCreateInstance();
            folderUrl = folderUrl ?? _basePublicUrl;

            var storageFolderPath = GetStoragePathFromUrl(folderUrl);

            ValidatePath(storageFolderPath);

            if (!Directory.Exists(storageFolderPath))
            {
                return Task.FromResult(retVal);
            }
            var directories = string.IsNullOrEmpty(keyword) ? Directory.GetDirectories(storageFolderPath) : Directory.GetDirectories(storageFolderPath, "*" + keyword + "*", SearchOption.AllDirectories);
            foreach (var directory in directories)
            {
                var directoryInfo = new DirectoryInfo(directory);

                var folder = AbstractTypeFactory<BlobFolder>.TryCreateInstance();
                folder.Name = Path.GetFileName(directory);
                folder.Url = GetAbsoluteUrlFromPath(directory);
                folder.ParentUrl = GetAbsoluteUrlFromPath(directoryInfo.Parent?.FullName);
                folder.RelativeUrl = GetRelativeUrl(folder.Url);
                retVal.Results.Add(folder);
            }

            var files = string.IsNullOrEmpty(keyword) ? Directory.GetFiles(storageFolderPath) : Directory.GetFiles(storageFolderPath, "*" + keyword + "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var fileInfo = new FileInfo(file);

                var blobInfo = AbstractTypeFactory<BlobInfo>.TryCreateInstance();
                blobInfo.Url = GetAbsoluteUrlFromPath(file);
                blobInfo.ContentType = MimeTypeResolver.ResolveContentType(fileInfo.Name);
                blobInfo.Size = fileInfo.Length;
                blobInfo.Name = fileInfo.Name;
                blobInfo.ModifiedDate = fileInfo.LastWriteTimeUtc;
                blobInfo.RelativeUrl = GetRelativeUrl(blobInfo.Url);
                retVal.Results.Add(blobInfo);
            }

            retVal.TotalCount = retVal.Results.Count;
            return Task.FromResult(retVal);
        }

        /// <summary>
        /// Create folder in file system within to base directory
        /// </summary>
        /// <param name="folder"></param>
        public virtual Task CreateFolderAsync(BlobFolder folder)
        {
            if (folder == null)
            {
                throw new ArgumentNullException(nameof(folder));
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

            return Task.CompletedTask;
        }

        /// <summary>
        /// Remove folders and blobs by absolute or relative urls
        /// </summary>
        /// <param name="urls"></param>
        public virtual Task RemoveAsync(string[] urls)
        {
            if (urls == null)
            {
                throw new ArgumentNullException(nameof(urls));
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

            return Task.CompletedTask;
        }

        public virtual void Move(string srcUrl, string destUrl)
        {
            var srcPath = GetStoragePathFromUrl(srcUrl);
            var dstPath = GetStoragePathFromUrl(destUrl);

            if (srcPath != dstPath)
            {
                if (Directory.Exists(srcPath) && !Directory.Exists(dstPath))
                {
                    Directory.Move(srcPath, dstPath);
                }
                else if (File.Exists(srcPath) && !File.Exists(dstPath))
                {
                    File.Move(srcPath, dstPath);
                }
            }
        }

        public virtual void Copy(string srcUrl, string destUrl)
        {
            var srcPath = GetStoragePathFromUrl(srcUrl);
            var destPath = GetStoragePathFromUrl(destUrl);

            CopyDirectoryRecursive(srcPath, destPath);
        }

        private static void CopyDirectoryRecursive(string sourcePath, string destPath)
        {
            if (!Directory.Exists(destPath))
            {
                Directory.CreateDirectory(destPath);
            }

            foreach (var file in Directory.GetFiles(sourcePath))
            {
                var dest = Path.Combine(destPath, Path.GetFileName(file));
                File.Copy(file, dest);
            }

            foreach (var folder in Directory.GetDirectories(sourcePath))
            {
                var dest = Path.Combine(destPath, Path.GetFileName(folder));
                CopyDirectoryRecursive(folder, dest);
            }
        }

        #endregion IBlobStorageProvider members

        #region IBlobUrlResolver Members

        public virtual string GetAbsoluteUrl(string blobKey)
        {
            if (blobKey == null)
            {
                throw new ArgumentNullException(nameof(blobKey));
            }

            var retVal = blobKey;
            if (!blobKey.IsAbsoluteUrl())
            {
                retVal = _basePublicUrl + "/" + blobKey.TrimStart('/').TrimEnd('/');
            }
            return new Uri(retVal).ToString();
        }

        #endregion IBlobUrlResolver Members

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
            var retVal = _basePublicUrl + "/" + path.Replace(_storagePath, string.Empty)
                             .TrimStart(Path.DirectorySeparatorChar)
                             .Replace(Path.DirectorySeparatorChar, '/');
            return Uri.EscapeUriString(retVal);
        }

        protected string GetStoragePathFromUrl(string url)
        {
            var retVal = _storagePath;
            if (url != null)
            {
                retVal = _storagePath + Path.DirectorySeparatorChar;
                retVal += GetRelativeUrl(url);
                retVal = retVal.Replace('/', Path.DirectorySeparatorChar)
                               .Replace($"{Path.DirectorySeparatorChar}{Path.DirectorySeparatorChar}", $"{Path.DirectorySeparatorChar}");
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
