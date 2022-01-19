using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.FileProviders.Physical;
using VirtoCommerce.Platform.Core.Caching;

namespace VirtoCommerce.Platform.Web.TagHelpers.Internal
{
    /// <summary>
    /// Provides version hash for a specified file.
    /// </summary>
    internal class FileVersionHashProvider
    {
        private readonly IPlatformMemoryCache _platformMemoryCache;
        private readonly PhysicalFilesWatcher _fileSystemWatcher;
        private readonly string _rootPath;

        public FileVersionHashProvider(string rootPath, IPlatformMemoryCache platformMemoryCache)
        {
            _platformMemoryCache = platformMemoryCache;
            _rootPath = rootPath.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
            _fileSystemWatcher = new PhysicalFilesWatcher(_rootPath, new FileSystemWatcher(_rootPath), false);
        }

        public string GetFileVersionHash(string physicalPath)
        {
            if (physicalPath == null)
            {
                throw new ArgumentNullException(nameof(physicalPath));
            }
            string result = null;
            if (File.Exists(physicalPath))
            {
                var cacheKey = CacheKey.With(GetType(), "GetVersion", physicalPath);
                result = _platformMemoryCache.GetOrCreateExclusive(cacheKey, cacheEntry =>
                {
                    cacheEntry.AddExpirationToken(_fileSystemWatcher.CreateFileChangeToken(GetRelativePath(physicalPath)));
                    using (var stream = File.OpenRead(physicalPath))
                    {
                        using (var hashAlgorithm = SHA256.Create())
                        {
                            return $"{WebEncoders.Base64UrlEncode(hashAlgorithm.ComputeHash(stream))}";
                        }
                    }
                });
            }
            return result;
        }

        private string GetRelativePath(string path)
        {
            return path.Replace(_rootPath, string.Empty).Replace(Path.DirectorySeparatorChar, '/').TrimStart('/');
        }
    }
}
