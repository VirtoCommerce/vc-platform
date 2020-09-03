// Based on https://github.com/natemcmaster/DotNetCorePlugins/blob/406c9b2ac18167e3ecac4a91ff14d7f12a79f0f3/src/Plugins/Loader/RuntimeConfigExtensions.cs

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace VirtoCommerce.Platform.Modules.AssemblyLoading
{
    /// <summary>
    /// Extensions for creating a load context using settings from a runtimeconfig.json file
    /// </summary>
    public static class RuntimeConfigExtensions
    {
        private const string JsonExt = ".json";

        /// <summary>
        /// Tries to get additional probing paths using settings found in the runtimeconfig.json and runtimeconfig.dev.json files.
        /// </summary>
        /// <param name="runtimeConfigPath">The path to the runtimeconfig.json file</param>
        /// <param name="isDevelopmentEnvironment">If it is development environment, should be true. Will add configs local probing paths in that case.</param>
        /// <param name="error">The error, if one occurs while parsing runtimeconfig.json</param>
        /// <returns>Additional probing paths (including NuGet and DotNet package caches).</returns>
        public static IEnumerable<string> TryGetAdditionalProbingPathFromRuntimeConfig(
            this string runtimeConfigPath,
            bool isDevelopmentEnvironment,
            out Exception error)
        {
            // Add NuGet cache path always
            var result = new HashSet<string>() { PlatformInformation.NuGetPackagesCache };
            error = null;
            try
            {
                var config = TryReadConfig(runtimeConfigPath);
                if (config == null)
                {
                    return result;
                }

                var configDevPath = runtimeConfigPath.Substring(0, runtimeConfigPath.Length - JsonExt.Length) + ".dev.json";
                var devConfig = TryReadConfig(configDevPath);

                var tfm = config.runtimeOptions?.Tfm ?? devConfig?.runtimeOptions?.Tfm;

                if (isDevelopmentEnvironment && config.runtimeOptions != null)
                {
                    result.UnionWith(ExtractProbingPaths(config.runtimeOptions, tfm));
                }

                if (isDevelopmentEnvironment && devConfig?.runtimeOptions != null)
                {
                    result.UnionWith(ExtractProbingPaths(devConfig.runtimeOptions, tfm));
                }

                // Add dotnet store path
                if (tfm != null)
                {
                    var dotnet = Process.GetCurrentProcess().MainModule.FileName;
                    if (string.Equals(Path.GetFileNameWithoutExtension(dotnet), "dotnet", StringComparison.OrdinalIgnoreCase))
                    {
                        var dotnetHome = Path.GetDirectoryName(dotnet);
                        result.Add(Path.Combine(dotnetHome, "store", RuntimeInformation.OSArchitecture.ToString().ToLowerInvariant(), tfm));
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex;
            }
            return result;
        }

        private static IEnumerable<string> ExtractProbingPaths(RuntimeOptions options, string tfm)
        {
            var result = new List<string>();
            if (options.AdditionalProbingPaths == null)
            {
                return result;
            }

            foreach (var item in options.AdditionalProbingPaths)
            {
                var path = item;
                if (path.Contains("|arch|"))
                {
                    path = path.Replace("|arch|", RuntimeInformation.OSArchitecture.ToString().ToLowerInvariant());
                }

                if (path.Contains("|tfm|"))
                {
                    if (tfm == null)
                    {
                        // We don't have enough information to parse this
                        continue;
                    }

                    path = path.Replace("|tfm|", tfm);
                }

                result.Add(path);
            }
            return result;
        }

        private static RuntimeConfig TryReadConfig(string path)
        {
            if (File.Exists(path))
            {
                using (var file = File.OpenText(path))
                using (var json = new JsonTextReader(file))
                {
                    var serializer = new JsonSerializer
                    {
                        ContractResolver = new DefaultContractResolver
                        {
                            NamingStrategy = new CamelCaseNamingStrategy(),
                        },
                    };
                    return serializer.Deserialize<RuntimeConfig>(json);
                }
            }
            return null;
        }
    }
}

