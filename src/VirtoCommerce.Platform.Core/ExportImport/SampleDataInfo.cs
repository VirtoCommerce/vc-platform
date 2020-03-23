using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.ExportImport
{
    public class SampleDataInfo : ValueObject
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Size { get; set; }
        public string Url { get; set; }
        public string PlatformVersion { get; set; }
    }
}
