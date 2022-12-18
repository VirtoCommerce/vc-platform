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

        /// <summary>
        /// Get or set an Http  Authorization Schema for ModulesManifestUrl.
        /// Authorization: [auth-scheme] [authorization-parameters]
        /// For Private GitLab
        /// Read https://docs.gitlab.com/ee/api/
        /// For Private GitHub
        /// Read https://docs.github.com/en/rest/overview/other-authentication-methods?apiVersion=2022-11-28#basic-authentication
        /// </summary>
        public string AuthorizationSchema { get; set; }

        /// <summary>
        /// Get or set an Http Authorization Parameter for ModulesManifestUrl.
        /// </summary>
        public string AuthorizationParameter { get; set; }

        /// <summary>
        /// Get or set an Authorization Token for ModulesManifestUrl that is used as part of Authorization header. 
        /// </summary>
        public string AuthorizationToken { get; set; }


        public string[] AutoInstallModuleBundles { get; set; }
        public bool IncludePrerelease { get; set; }
    }
}
