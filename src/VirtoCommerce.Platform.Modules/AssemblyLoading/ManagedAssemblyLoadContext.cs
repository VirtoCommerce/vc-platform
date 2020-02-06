using System;
using System.Collections.Generic;
using System.Runtime.Loader;

namespace VirtoCommerce.Platform.Modules.AssemblyLoading
{
    public class ManagedAssemblyLoadContext : AssemblyLoadContext
    {
        public string BasePath { get; set; }
        public IEnumerable<string> AdditionalProdingPath { get; set; }

        public IntPtr LoadUnmanagedLibrary(string absolutePath)
        {
            return LoadUnmanagedDll(absolutePath);
        }
        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            return LoadUnmanagedDllFromPath(unmanagedDllName);
        }
    }
}
