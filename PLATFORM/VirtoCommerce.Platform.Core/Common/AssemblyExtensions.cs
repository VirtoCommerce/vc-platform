using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace VirtoCommerce.Platform.Core.Common
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
    }
}
