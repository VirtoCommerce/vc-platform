using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Orders.Model
{
	[DataContract]
	[DataServiceKey("DiscountId")]
	public abstract class Discount : StorageEntity
	{
		public Discount()
		{
			DiscountId = GenerateNewKey();
		}

		private string _DiscountId;
       
		[Key]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string DiscountId
		{
            get
			{
				return _DiscountId;
			}
			set
			{
				SetValue(ref _DiscountId, () => this.DiscountId, value);
			}
		}

		private string _PromotionId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
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
		
		private decimal _DiscountAmount;
		[DataMember]
		public decimal DiscountAmount
		{
			get
			{
				return _DiscountAmount;
			}
			set
			{
				SetValue(ref _DiscountAmount, () => this.DiscountAmount, value);
			}
		}

		private string _DiscountCode;
        [StringLength(128)]
		[DataMember]
		public string DiscountCode
		{
			get
			{
				return _DiscountCode;
			}
			set
			{
				SetValue(ref _DiscountCode, () => this.DiscountCode, value);
			}
		}

		private string _DiscountName;
        [StringLength(128)]
		[DataMember]
		public string DiscountName
		{
			get
			{
				return _DiscountName;
			}
			set
			{
				SetValue(ref _DiscountName, () => this.DiscountName, value);
			}
		}

		private string _DisplayMessage;
        [StringLength(256)]
		[DataMember]
		public string DisplayMessage
		{
			get
			{
				return _DisplayMessage;
			}
			set
			{
				SetValue(ref _DisplayMessage, () => this.DisplayMessage, value);
			}
		}
	}
}
