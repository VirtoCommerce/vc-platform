using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace VirtoCommerce.Platform.Modules.AssemblyLoading
{
    [SuppressMessage("SonarLint", "S101", Justification = "TPA is common abbreviation for Trusted Platform Assemblies, not need to change to Pascal Case.")]
    internal static class TPA
    {
        private static readonly string[] _TPAPaths = GetTPAList();

        static TPA()
        {
        }

        /// <summary>
        /// Gets the list of Trusted Platform Assemblies (TPA).
        /// <para>
        /// https://github.com/dotnet/coreclr/issues/919#issuecomment-285928695
        /// https://docs.microsoft.com/en-US/dotnet/core/tutorials/netcore-hosting#step-5---preparing-appdomain-settings
        /// </para>
        /// </summary>
        /// <returns>Returns the list of TPA paths.</returns>
        static string[] GetTPAList()
        {
            var tpa = (string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES");
            return tpa.Split(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ";" : ":");
        }

        /// <summary>
        /// Checks whether we have assembly with the same file name in TPA list.
        /// </summary>
        /// <param name="assemblyFileName">File name with extension and without path.</param>
        /// <returns>True if TPA list contains assembly with the same file name, otherwise false.</returns>
        public static bool ContainsAssembly(string assemblyFileName)
        {
            return _TPAPaths.Any(x => x.EndsWith(Path.DirectorySeparatorChar + assemblyFileName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
