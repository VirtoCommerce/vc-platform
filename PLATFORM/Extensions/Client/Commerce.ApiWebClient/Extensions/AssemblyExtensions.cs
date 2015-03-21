using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace VirtoCommerce.ApiWebClient.Extensions
{
    public static class AssemblyExtensions
    {
        public static string GetInformationalVersion(this Assembly assembly)
        {
            return
                assembly.GetCustomAttributes(false)
                        .OfType<AssemblyInformationalVersionAttribute>()
                        .Single<AssemblyInformationalVersionAttribute>()
                        .InformationalVersion;
        }

        public static string GetFileVersion(this Assembly assembly)
        {
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            var version = fvi.FileBuildPart.ToString();
            return version;
        }

        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException("assembly");
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        public static IEnumerable<Type> GetTypesSafe(this Assembly a)
        {
            try
            {
                return a.GetLoadableTypes();
            }
            catch
            {
                return Enumerable.Empty<Type>();
            }
        }
    }
}
