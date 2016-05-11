using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Platform.Web.Model.Asset
{
    public class AssetListItem
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string Url { get; set; }
        public string Size { get; set; }
        public string ParentUrl { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}