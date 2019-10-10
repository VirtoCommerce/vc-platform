// https://github.com/natemcmaster/DotNetCorePlugins/blob/406c9b2ac18167e3ecac4a91ff14d7f12a79f0f3/src/Plugins/Internal/RuntimeOptions.cs
namespace VirtoCommerce.Platform.Modules.AssemblyLoading
{
    internal class RuntimeOptions
    {
        public string Tfm { get; set; }

        public string[] AdditionalProbingPaths { get; set; }
    }
}
