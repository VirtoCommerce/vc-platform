using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Foundation.Frameworks;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtoCommerce.Foundation.Reviews.Model
{
    [DataContract]
	[EntitySet("MediaContents")]
    [DataServiceKey("MediaContentId")]
    public class MediaContent : StorageEntity
    {

		private string _mediaContentId;
        [Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
        public string MediaContentId
        {
            get
            {
				return _mediaContentId;
            }
            set
            {
                SetValue(ref _mediaContentId, () => this.MediaContentId, value);
            }
        }

		private string _reviewId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string ReviewId
        {
            get
            {
				return _reviewId;
            }
            set
            {
                SetValue(ref _reviewId, () => this.ReviewId, value);
            }
        }

		private string _smallUrl;
		[StringLength(1024, ErrorMessage = "Only 1024 characters allowed.")]
		[DataMember]
        public string SmallUrl
        {
            get
            {
				return _smallUrl;
            }
            set
            {
                SetValue(ref _smallUrl, () => this.SmallUrl, value);
            }
        }

		private string _largeUrl;
		[StringLength(1024, ErrorMessage = "Only 1024 characters allowed.")]
		[DataMember]
        public string LargeUrl
        {
            get
            {
				return _largeUrl;
            }
            set
            {
                SetValue(ref _largeUrl, () => this.LargeUrl, value);
            }
        }

		private string _mediumUrl;
		[StringLength(1024, ErrorMessage = "Only 1024 characters allowed.")]
		[DataMember]
        public string MediumUrl
        {
            get
            {
				return _mediumUrl;
            }
            set
            {
                SetValue(ref _mediumUrl, () => this.MediumUrl, value);
            }
        }

		private string _contentType;
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
		[DataMember]
        public string ContentType
        {
            get
            {
				return _contentType;
            }
            set
            {
                SetValue(ref _contentType, () => this.ContentType, value);
            }
        } // Image, Video

		private string _description;
		[DataMember]
		[StringLength(256, ErrorMessage = "Only 256 characters allowed.")]
        public string Description
        {
            get
            {
				return _description;
            }
            set
            {
                SetValue(ref _description, () => this.Description, value);
            }
        }

		[DataMember]
        [Parent]
        [ForeignKey("ReviewId")]
		public Review Review
		{
			get;
			set;
		}
    }
}
