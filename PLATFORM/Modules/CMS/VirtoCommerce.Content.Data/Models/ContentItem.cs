using System;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Content.Data.Models
{
    public class ContentItem : AuditableEntity
    {
        #region Public Properties

		//[Required]
		public string ContentType { get; set; }
        //[Required]
        public string Name { get; set; }

        //[Required]
        public string Path { get; set; }

		public byte[] ByteContent { get; set; }

		public string FileUrl { get; set; }

        #endregion
    }
}