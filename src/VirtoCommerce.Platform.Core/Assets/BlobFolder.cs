using System;

namespace VirtoCommerce.Platform.Core.Assets
{
    [Obsolete("Deprecated. Use assets from Assets module.")]
    public class BlobFolder : BlobEntry
    {
        public BlobFolder()
        {
            Type = "folder";
        }
        public string ParentUrl { get; set; }
    }
}
