using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;

namespace VirtoCommerce.Platform.Web.Infrastructure
{
    /// <summary>
    /// Wraps an <see cref="IFileProvider"/> and reports files and directories under the configured path prefixes as not found.
    /// </summary>
    public class ProtectedPathsFileProvider : IFileProvider
    {
        private readonly IFileProvider _innerProvider;
        private readonly string[] _protectedPaths;

        public ProtectedPathsFileProvider(IFileProvider innerProvider, IEnumerable<string> protectedPaths)
        {
            ArgumentNullException.ThrowIfNull(innerProvider);
            ArgumentNullException.ThrowIfNull(protectedPaths);

            _innerProvider = innerProvider;
            _protectedPaths = protectedPaths
                .Select(NormalizePath)
                .Where(path => !string.IsNullOrEmpty(path))
                .ToArray();
        }

        public IFileInfo GetFileInfo(string subpath)
        {
            return IsProtected(subpath)
                ? new NotFoundFileInfo(subpath)
                : _innerProvider.GetFileInfo(subpath);
        }

        public IDirectoryContents GetDirectoryContents(string subpath)
        {
            return IsProtected(subpath)
                ? NotFoundDirectoryContents.Singleton
                : _innerProvider.GetDirectoryContents(subpath);
        }

        public IChangeToken Watch(string filter)
        {
            return _innerProvider.Watch(filter);
        }

        private bool IsProtected(string subpath)
        {
            var path = NormalizePath(subpath);

            return !string.IsNullOrEmpty(path) && _protectedPaths.Any(prefix =>
                path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) &&
                (path.Length == prefix.Length || path[prefix.Length] == '/'));
        }

        private static string NormalizePath(string path)
        {
            return path?.Replace('\\', '/').Trim('/');
        }
    }
}
