namespace VirtoCommerce.Platform.Core.Modularity
{
    public class ManifestAppInfo
    {
        public ManifestAppInfo()
        {
        }

        public ManifestAppInfo(ManifestApp item)
        {
            Id = item.Id;
            Title = item.Title;
            Description = item.Description;
            IconUrl = item.IconUrl;
            RelativeUrl = $"/apps/{item.Id}";
            Permission = item.Permission;
            ContentPath = item.ContentPath;
            SupportEmbeddedMode = item.SupportEmbeddedMode;
        }

        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string IconUrl { get; set; }

        public string RelativeUrl { get; set; }

        public string Permission { get; set; }

        /// <summary>
        /// Path to App content in the module. By default, Content/{moduleApp.Id}
        /// </summary>
        public string ContentPath { get; set; }

        public bool SupportEmbeddedMode { get; set; }
    }
}
