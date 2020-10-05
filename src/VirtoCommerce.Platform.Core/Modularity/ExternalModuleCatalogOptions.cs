using System;

namespace VirtoCommerce.Platform.Core.Modularity
{
    public class ExternalModuleCatalogOptions
    {
        public Uri ModulesManifestUrl { get; set; }
        //Allows to set additional data sources for loading module manifests.
        //The resulting manifests will be merged with manifests that are loaded from data source is set by ModulesManifestUrl setting.
        //Example: "ExternalModules:ExtraModulesManifestUrls": [ "c:\\my-modules-registry-from-file.json", "http://somewhere.com/my-modules-registry.json" ]
        public Uri[] ExtraModulesManifestUrls { get; set; }
        public string AuthorizationToken { get; set; }
        public string[] AutoInstallModuleBundles { get; set; }
        public bool IncludePrerelease { get; set; }
    }
}
