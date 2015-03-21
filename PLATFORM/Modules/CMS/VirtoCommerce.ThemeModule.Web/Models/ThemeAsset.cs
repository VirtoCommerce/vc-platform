using System;

namespace VirtoCommerce.ThemeModule.Web.Models
{
	public class ThemeAsset
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