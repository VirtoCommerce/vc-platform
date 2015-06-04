using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid.FileSystems;

namespace VirtoCommerce.Web.Views.Engines.Liquid
{
    public interface IFileSystemFactory
    {
        /// <summary>
        /// Gets a <see cref="IFileSystem"/> instance for the provided <paramref name="context"/>.
        /// </summary>
        /// <param name="context">The context that the filesystem should be created for.</param>
        /// <param name="extensions">View extensions to search for</param>
        /// <returns>An <see cref="IFileSystem"/> instance.</returns>
        IFileSystem GetFileSystem(IEnumerable<string> extensions);
    }
}
