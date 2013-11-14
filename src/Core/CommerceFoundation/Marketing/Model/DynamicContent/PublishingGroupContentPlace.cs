using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using VirtoCommerce.Foundation.Frameworks;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtoCommerce.Foundation.Marketing.Model.DynamicContent
{
	[DataContract]
	[EntitySet("PublishingGroupContentPlaces")]
	[DataServiceKey("PublishingGroupContentPlaceId")]
	public class PublishingGroupContentPlace : StorageEntity
	{
		public PublishingGroupContentPlace()
		{
			_PublishingGroupContentPlaceId = GenerateNewKey();
		}

		private string _PublishingGroupContentPlaceId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string PublishingGroupContentPlaceId
		{
			get
			{
				return _PublishingGroupContentPlaceId;
			}
			set
			{
				SetValue(ref _PublishingGroupContentPlaceId, () => this.PublishingGroupContentPlaceId, value);
			}
		}
		
		#region Navigation Properties

		private string _PublishingGroupId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        [ForeignKey("PublishingGroup")]
		[Required]
		public string DynamicContentPublishingGroupId
		{
			get
			{
				return _PublishingGroupId;
			}
			set
			{
				SetValue(ref _PublishingGroupId, () => this.DynamicContentPublishingGroupId, value);
			}
		}

		[DataMember]
		public virtual DynamicContentPublishingGroup PublishingGroup { get; set; }

		private string _ContentPlaceId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        [ForeignKey("ContentPlace")]
		[Required]
		public string DynamicContentPlaceId
		{
			get
			{
				return _ContentPlaceId;
			}
			set
			{
				SetValue(ref _ContentPlaceId, () => this.DynamicContentPlaceId, value);
			}
		}

		[DataMember]
		public virtual DynamicContentPlace ContentPlace { get; set; }
				
		#endregion
	}
}
