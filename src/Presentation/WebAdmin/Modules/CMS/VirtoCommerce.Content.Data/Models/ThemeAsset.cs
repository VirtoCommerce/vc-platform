using System;

namespace VirtoCommerce.Content.Data.Models
{
    public class ThemeAsset
    {
        public string Id { get; set; }

		public string AssetName { get; set; }

		public string AssetUrl { get; set; }

		public byte[] ByteContent { get; set; }

		public string ContentType { get; set; }
        public DateTime Updated { get; set; }
    }
}
