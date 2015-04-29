using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtoCommerce.Content.Web.Models
{
    public class SyncAsset
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public byte[] ByteContent { get; set; }

        public string AssetUrl { get; set; }

        public string ContentType { get; set; }

        public DateTime Updated { get; set; }
    }
}