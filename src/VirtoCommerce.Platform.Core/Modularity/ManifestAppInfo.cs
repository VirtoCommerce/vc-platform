using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Modularity
{
    public class ManifestAppInfo 
    {
        public ManifestAppInfo()
        {
        }

        public ManifestAppInfo(ManifestApp item)
        {
            this.Id = item.Id;
            this.Title = item.Title;
            this.Description = item.Description;
            this.IconUrl = item.IconUrl;
            this.RelativeUrl = $"/apps/{item.Id}";
            this.Permission = item.Permission;
            this.ContentPath = item.ContentPath;
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string IconUrl { get; set; }

        public string RelativeUrl { get; set; }

        public string Permission { get; set; }

        /// <summary>
        /// Path to App content in the module. By default Content/{moduleApp.Id}
        /// </summary>
        public string ContentPath { get; set; }
    }
}
