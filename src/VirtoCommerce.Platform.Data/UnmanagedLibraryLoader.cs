using System;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core;
using VirtoCommerce.Platform.Core.Modularity;

namespace VirtoCommerce.Platform.Data
{
    public class UnmanagedLibraryLoader
    {
        private readonly IAssemblyResolver _assemblyResolver;
        private readonly PlatformOptions _platformOptions;

        public UnmanagedLibraryLoader(IAssemblyResolver assemblyResolver, IOptions<PlatformOptions> platfromOptions)
        {
            _assemblyResolver = assemblyResolver;
            _platformOptions = platfromOptions.Value;
        }

        public void Load()
        {
            var architectureFolder = (IntPtr.Size == 8) ? "64 bit" : "32 bit";

            foreach (var sourceFilePath in Directory.EnumerateFiles(_platformOptions.LibraryPath, "*", SearchOption.AllDirectories)
                                                        .Where(x => x.Contains(architectureFolder) &&
                                                            x.EndsWith(PlatformInformation.NativeLibraryExtensions.FirstOrDefault())))
            {
                _assemblyResolver.LoadUnmanagedLibrary(sourceFilePath);
            }
        }
    }
}
