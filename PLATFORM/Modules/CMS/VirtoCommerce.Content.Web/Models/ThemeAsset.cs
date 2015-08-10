using System;

namespace VirtoCommerce.Content.Web.Models
{
	/// <summary>
	/// Theme asset
	/// </summary>
	public class ThemeAsset
	{
		/// <summary>
		/// Theme asset id
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Theme asset name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Theme asset text content
		/// </summary>
		public string Content { get; set; }

		/// <summary>
		/// Theme asset byte content
		/// </summary>
		public byte[] ByteContent { get; set; }

		public string AssetUrl { get; set; }

		/// <summary>
		/// Theme asset content type
		/// </summary>
		public string ContentType { get; set; }

		/// <summary>
		/// Theme asset updated date
		/// </summary>
	    public DateTime Updated { get; set; }
	}
}