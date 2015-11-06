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

        public FileSystemBlobProvider(string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException("connectionString");
            }

            var properties = connectionString.ToDictionary(";", "=");

            _storagePath = HostingEnvironment.MapPath(properties["rootPath"]);
            if (_storagePath != null)
            {
                _storagePath = _storagePath.TrimEnd('\\');
            }

            _basePublicUrl = properties["publicUrl"];
            if (_basePublicUrl != null)
            {
                _basePublicUrl = _basePublicUrl.TrimEnd('/');
            }
        }

        #region IBlobStorageProvider members

        public string Upload(UploadStreamInfo request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            var folderPath = GetAbsoluteStoragePathFromUrl(request.FolderName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var storageFileName = Path.Combine(folderPath, request.FileName);
            UpdloadFile(request.FileByteStream, storageFileName);

            return Path.Combine(request.FolderName, request.FileName);
        }

        /// <summary>
        /// Open blob by relative or absolute url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public Stream OpenReadOnly(string url)
        {
            if (string.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");

            var filePath = GetAbsoluteStoragePathFromUrl(url);

            var stream = LoadFile(filePath);

            return stream;
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

            var storageFolderPath = GetAbsoluteStoragePathFromUrl(folderUrl);

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
                path = GetAbsoluteStoragePathFromUrl(folder.ParentUrl);
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
                var path = GetAbsoluteStoragePathFromUrl(url);
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

        private string GetAbsoluteStoragePathFromUrl(string url)
        {
            var retVal = _storagePath;
            if (url != null)
            {
                retVal = _storagePath + "\\" + url.Replace(_basePublicUrl, String.Empty).Replace("/", "\\");
            }
            return Uri.UnescapeDataString(retVal);
        }


        private static void UpdloadFile(Stream stream, string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (var uploadedFileStream = File.Open(filePath, FileMode.Create))
            {
                var buffer = new byte[256];
                int count;
                while ((count = stream.Read(buffer, 0, 256)) > 0)
                    uploadedFileStream.Write(buffer, 0, count);
                uploadedFileStream.Flush();
            }
        }


        private static Stream LoadFile(string filePath)
        {
            Stream copyStream;
            using (var uploadedFileStream = File.Open(filePath, FileMode.Open))
            {
                copyStream = new MemoryStream();
                uploadedFileStream.CopyTo(copyStream);
                copyStream.Seek(0, SeekOrigin.Begin);
            }
            return copyStream;
        }

    }
}
