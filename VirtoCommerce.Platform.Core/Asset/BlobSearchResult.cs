using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Asset
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
