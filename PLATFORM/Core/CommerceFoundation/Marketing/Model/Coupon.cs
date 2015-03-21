using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using VirtoCommerce.Foundation.Frameworks;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtoCommerce.Foundation.Marketing.Model
{
	[DataContract]
	[EntitySet("Coupons")]
	[DataServiceKey("CouponId")]
	public class Coupon : StorageEntity
	{
		public Coupon()
		{
			_CouponId = GenerateNewKey();
		}

		private string _CouponId;
		[Key]
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string CouponId
		{
			get
			{
				return _CouponId;
			}
			set
			{
				SetValue(ref _CouponId, () => this.CouponId, value);
			}
		}

	
		private string _Code;
		[DataMember]
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
		public string Code
		{
			get
			{
				return _Code;
			}
			set
			{
				SetValue(ref _Code, () => this.Code, value);
			}
		}

		#region Navigation Properties

		private string _CouponSetId;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[ForeignKey("CouponSet")]
		public string CouponSetId
		{
			get
			{
				return _CouponSetId;
			}
			set
			{
				SetValue(ref _CouponSetId, () => this.CouponSetId, value);
			}
		}

		[DataMember]
		public virtual CouponSet CouponSet { get; set; }
		#endregion
	}
}
