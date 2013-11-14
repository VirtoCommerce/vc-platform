using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data.Services.Common;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Foundation.Frameworks;
using System.Collections.ObjectModel;

namespace VirtoCommerce.Foundation.Marketing.Model
{
	[DataContract]
	[EntitySet("CouponSets")]
	[DataServiceKey("CouponSetId")]
	public class CouponSet : StorageEntity
	{
		public CouponSet()
		{
			_CouponSetId = GenerateNewKey();
		}

		private string _CouponSetId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
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

		private string _Name;
		[DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				SetValue(ref _Name, () => this.Name, value);
			}
		}

		#region Navigation Properties

		private ObservableCollection<Coupon> _coupons;
		[DataMember]
		public ObservableCollection<Coupon> Coupons
		{
			get
			{
				if (_coupons == null)
					_coupons = new ObservableCollection<Coupon>();

				return _coupons;
			}
		} 

		#endregion

	}
}
