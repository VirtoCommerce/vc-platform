// Based on https://github.com/natemcmaster/DotNetCorePlugins/blob/1001cdede224c0d335f21ec7b1a9f3fa7ad7fa84/src/Plugins/Internal/PlatformInformation.cs
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.InteropServices;

namespace VirtoCommerce.Platform.Modules.AssemblyLoading
{
    internal static class PlatformInformation
    {
        public static readonly string[] NativeLibraryExtensions;
        public static readonly string[] NativeLibraryPrefixes;
        public static readonly string[] ManagedAssemblyExtensions =
        [
            ".dll",
            ".ni.dll",
            ".exe",
            ".ni.exe",
        ];
        public static readonly string NuGetPackagesCache;
        public static readonly char DirectorySeparator;

        [SuppressMessage("SonarLint", "S3963", Justification = "Such conditional initialization looks better in constructor, than inlined.")]
        static PlatformInformation()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                NativeLibraryPrefixes = [];
                NativeLibraryExtensions = [".dll"];
                NuGetPackagesCache = GetNuGetPackagesPath();
                DirectorySeparator = Path.DirectorySeparatorChar;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                NativeLibraryPrefixes = ["lib"];
                NativeLibraryExtensions = [".dylib"];
                NuGetPackagesCache = GetNuGetPackagesPath();
                DirectorySeparator = Path.DirectorySeparatorChar;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                NativeLibraryPrefixes = ["lib"];
                NativeLibraryExtensions = [".so", ".so.1"];
                NuGetPackagesCache = GetNuGetPackagesPath();
                DirectorySeparator = Path.DirectorySeparatorChar;
            }
            else
            {
                Debug.Fail("Unknown OS type");
                NativeLibraryPrefixes = [];
                NativeLibraryExtensions = [];
                NuGetPackagesCache = string.Empty;
                DirectorySeparator = '\0';
            }
        }

        private static string GetNuGetPackagesPath()
        {
            var path = Environment.GetEnvironmentVariable("NUGET_PACKAGES");
            if (!string.IsNullOrEmpty(path))
            {
                return path;
            }

            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".nuget", "packages");
        }
    }
}
