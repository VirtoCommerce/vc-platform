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
        public static readonly string[] ManagedAssemblyExtensions = new[]
        {
                ".dll",
                ".ni.dll",
                ".exe",
                ".ni.exe"
        };
        public static readonly string NuGetPackagesCache;
        public static readonly char DirectorySeparator;

        [SuppressMessage("SonarLint", "S3963", Justification = "Such conditional initialization looks better in constructor, than inlined.")]
        static PlatformInformation()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                NativeLibraryPrefixes = new[] { "" };
                NativeLibraryExtensions = new[] { ".dll" };
                NuGetPackagesCache = Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), ".nuget", "packages");
                DirectorySeparator = Path.DirectorySeparatorChar;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                NativeLibraryPrefixes = new[] { "", "lib", };
                NativeLibraryExtensions = new[] { ".dylib" };
                NuGetPackagesCache = Path.Combine("~", ".nuget", "packages");
                DirectorySeparator = Path.AltDirectorySeparatorChar;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                NativeLibraryPrefixes = new[] { "", "lib" };
                NativeLibraryExtensions = new[] { ".so", ".so.1" };
                NuGetPackagesCache = Path.Combine("~", ".nuget", "packages");
                DirectorySeparator = Path.AltDirectorySeparatorChar;
            }
            else
            {
                Debug.Fail("Unknown OS type");
                NativeLibraryPrefixes = Array.Empty<string>();
                NativeLibraryExtensions = Array.Empty<string>();
                NuGetPackagesCache = string.Empty;
                DirectorySeparator = '\0';
            }
        }
    }
}
