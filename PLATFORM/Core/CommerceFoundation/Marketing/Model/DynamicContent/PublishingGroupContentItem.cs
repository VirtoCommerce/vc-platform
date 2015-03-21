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
	[EntitySet("PublishingGroupContentItems")]
	[DataServiceKey("PublishingGroupContentItemId")]
	public class PublishingGroupContentItem : StorageEntity
	{
		public PublishingGroupContentItem()
		{
			_PublishingGroupContentItemId = GenerateNewKey();
		}

		private string _PublishingGroupContentItemId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string PublishingGroupContentItemId
		{
			get
			{
				return _PublishingGroupContentItemId;
			}
			set
			{
				SetValue(ref _PublishingGroupContentItemId, () => this.PublishingGroupContentItemId, value);
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

		private string _ContentItemId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        [ForeignKey("ContentItem")]
		[Required]
		public string DynamicContentItemId
		{
			get
			{
				return _ContentItemId;
			}
			set
			{
				SetValue(ref _ContentItemId, () => this.DynamicContentItemId, value);
			}
		}

		[DataMember]
		public virtual DynamicContentItem ContentItem { get; set; }
				
		#endregion
	}
}
