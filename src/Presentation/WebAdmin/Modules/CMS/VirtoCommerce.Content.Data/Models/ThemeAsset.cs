using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Content.Data.Models
{
    public class ThemeAsset
    {
        public string Id { get; set; }

		public string AssetName { get; set; }

		public byte[] ByteContent { get; set; }

		public string ContentType { get; set; }
    }
}
