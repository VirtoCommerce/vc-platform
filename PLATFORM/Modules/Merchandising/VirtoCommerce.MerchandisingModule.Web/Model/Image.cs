namespace VirtoCommerce.MerchandisingModule.Web.Model
{
    public class Image
    {
        /// <summary>
        /// Gets or sets the image file content as bytes array
        /// </summary>
        public byte[] Attachement { get; set; }

        /// <summary>
        /// Gets or sets the value for image name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value of image gorup name
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Gets or sets the value of image absolute URL
        /// </summary>
        public string Src { get; set; }

        /// <summary>
        /// Gets or sets the value of thumbnail image absolute URL
        /// </summary>
        public string ThumbSrc { get; set; }
    }
}