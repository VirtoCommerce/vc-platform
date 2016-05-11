using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Assets
{
    public class BlobSearchResult
    {
        public BlobSearchResult()
        {
            Folders = new List<BlobFolder>();
            Items = new List<BlobInfo>();
        }
        public ICollection<BlobFolder> Folders { get; set; }
        public ICollection<BlobInfo> Items { get; set; }
    }
}
