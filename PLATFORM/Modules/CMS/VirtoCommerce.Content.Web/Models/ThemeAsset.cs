using System;

namespace VirtoCommerce.Content.Web.Models
{
    public class ThemeAsset
    {
        /// <summary>
        /// Id, contains full path relative to theme root folder
        /// </summary>
		public string Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Theme asset text content (text files - html, css, js &amp; etc), based on content type
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Theme asset byte content (non-text files - images, fonts, zips &amp; etc), based on content type
        /// </summary>
        public byte[] ByteContent { get; set; }

        public string AssetUrl { get; set; }

        public string ContentType { get; set; }

        /// <summary>
        /// Theme asset last update date
        /// </summary>
        public DateTime Updated { get; set; }

        public string[] SecurityScopes { get; set; }
    }
}