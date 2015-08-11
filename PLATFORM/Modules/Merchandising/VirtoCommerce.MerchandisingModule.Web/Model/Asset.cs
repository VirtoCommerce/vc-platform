namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class Asset
    {
        /// <summary>
        /// Gets or sets the value of asset absolute URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the value of asset group name
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Gets or sets the value of asset name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of asset file size in bytes
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// Gets or sets the value of asset MIME type
        /// </summary>
        public string MimeType { get; set; }
    }
}