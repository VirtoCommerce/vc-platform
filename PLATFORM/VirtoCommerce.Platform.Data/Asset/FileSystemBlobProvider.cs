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
            _basePublicUrl = properties["publicUrl"];

            if (_basePublicUrl != null && !_basePublicUrl.EndsWith("/"))
            {
                _basePublicUrl += "/";
            }
        }

        #region IBlobStorageProvider members

        public string Upload(UploadStreamInfo request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            string folderName;
            string fileName;
            string key;
            string storagePath = _storagePath;

            folderName = request.FolderName;
            fileName = request.FileName;
            key = Path.Combine(folderName, fileName);
            storagePath = string.Empty;


            if (!string.IsNullOrEmpty(folderName))
                CreateFolder(_storagePath, folderName);

            var storageFileName = Path.Combine(_storagePath, folderName, fileName);
            UpdloadFile(request.FileByteStream, storageFileName);

            return key;
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

            var filePath = Path.Combine(_storagePath, url.IsAbsoluteUrl() ? new Uri(url).AbsolutePath : url);

            var stream = LoadFile(filePath);

            return stream;
        }

        /// <summary>
        /// Search folders and blobs in folder
        /// </summary>
        /// <param name="folderUrl">absolute or relative path</param>
        /// <returns></returns>
        public BlobSearchResult Search(string folderUrl)
        {
            var retVal = new BlobSearchResult();
            var path = _storagePath;
            folderUrl = folderUrl ?? _basePublicUrl;

            var relativeUri = new Uri(folderUrl.IsAbsoluteUrl() ? folderUrl.Replace(_basePublicUrl, String.Empty) : folderUrl, UriKind.Relative);
            var absoluteUri = folderUrl.IsAbsoluteUrl() ? new Uri(folderUrl) : new Uri(new Uri(_basePublicUrl), relativeUri);

            if (!String.IsNullOrEmpty(folderUrl))
            {
                path = Path.Combine(_storagePath, relativeUri.ToString());
            }

            foreach (var directory in Directory.EnumerateDirectories(path))
            {
                retVal.Folders.Add(new BlobFolder
                {
                    Name = Path.GetFileName(directory),
                    Url = new Uri(absoluteUri, directory.Replace(_storagePath, String.Empty).TrimStart('\\')).ToString(),
                    ParentUrl = absoluteUri.ToString()
                });
            }
            foreach (var file in Directory.EnumerateFiles(path))
            {
                var fileInfo = new FileInfo(file);
                retVal.Items.Add(new BlobInfo
                {
                    Url = new Uri(absoluteUri, fileInfo.Name).ToString(),
                    ContentType = MimeTypeResolver.ResolveContentType(fileInfo.Name),
                    Size = fileInfo.Length,
                    FileName = fileInfo.Name
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
                path = Path.Combine(_storagePath, folder.ParentUrl.Replace(_basePublicUrl, String.Empty));
            }
            CreateFolder(path, folder.Name);
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
            foreach(var url in urls)
            {
               var path = Path.Combine(_storagePath, url.Replace(_basePublicUrl, String.Empty));
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

        public string GetAbsoluteUrl(string assetKey)
        {
            var retVal = assetKey;
            if (!Uri.IsWellFormedUriString(assetKey, UriKind.Absolute))
            {
                retVal = _basePublicUrl + assetKey;

            }
            return retVal;
        }

        #endregion


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


        private static string CreateFolder(string storagePath, string folderName)
        {
            var directory = string.Format(CultureInfo.CurrentCulture, @"{0}\{1}", storagePath, folderName);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            return directory;
        }


    }
}
