using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Services.Common;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Foundation.Frameworks;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtoCommerce.Foundation.Marketing.Model
{
	[DataContract]
	[EntitySet("PromotionUsage")]
	[DataServiceKey("PromotionUsageId")]
	public class PromotionUsage : StorageEntity
	{
		public PromotionUsage()
        {
			_PromotionUsageId = GenerateNewKey();
        }

		private string _PromotionUsageId;
        [Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        [DataMember]
		public string PromotionUsageId
        {
            get
            {
				return _PromotionUsageId;
            }
            set
            {
				SetValue(ref _PromotionUsageId, () => this.PromotionUsageId, value);
            }
        }

		private string _MemberId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string MemberId
		{
			get
			{
				return _MemberId;
			}
			set
			{
				SetValue(ref _MemberId, () => this.MemberId, value);
			}
		}

		private string _OrderGroupId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string OrderGroupId
		{
			get
			{
				return _OrderGroupId;
			}
			set
			{
				SetValue(ref _OrderGroupId, () => this.OrderGroupId, value);
			}
		}

		private string _CouponCode;
		[DataMember]
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
		public string CouponCode
		{
			get
			{
				return _CouponCode;
			}
			set
			{
				SetValue(ref _CouponCode, () => this.CouponCode, value);
			}
		}

		private int _status;
		[DataMember]
		public int Status
		{
			get
			{
				return _status;
			}
			set
			{
				SetValue(ref _status, () => this.Status, value);
			}
		}

		private DateTime? _usageDate;
		[DataMember]
		public DateTime? UsageDate
		{
			get
			{
				return _usageDate;
			}
			set
			{
				SetValue(ref _usageDate, () => this.UsageDate, value);
			}
		}
	
		#region Navigation Properties

		private string _PromotionId;
        [DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[ForeignKey("Promotion")]
		public string PromotionId
        {
            get
            {
				return _PromotionId;
            }
            set
            {
				SetValue(ref _PromotionId, () => this.PromotionId, value);
            }
        }

        [DataMember]
        public virtual Promotion Promotion { get; set; }

		#endregion

	}
}
