namespace VirtoCommerce.Content.Data.Models
{
    #region

    using System;
    using VirtoCommerce.Foundation.Frameworks;

    #endregion

    public class ContentItem : Entity, IAuditable
    {
        #region Public Properties

        //[Required]
        public string Content { get; set; }

		//[Required]
		public string ContentType { get; set; }

		//[Required]
		//[StringLength(64)]
		public string CreatedBy { get; set; }

		//[Required]
		public DateTime CreatedDate { get; set; }

		//[StringLength(64)]
		public string ModifiedBy { get; set; }

		public DateTime? ModifiedDate { get; set; }

        //[Required]
        public string Name { get; set; }

        //[Required]
        public string Path { get; set; }

		public byte[] ByteContent { get; set; }

		public string FileUrl { get; set; }

        #endregion
    }
}