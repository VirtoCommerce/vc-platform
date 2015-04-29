using System;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Model
{
    public class Promotion : AuditableEntity
    {
		public Promotion()
		{
			Coupons = new ObservableCollection<Coupon>();
			PromotionUsages = new ObservableCollection<PromotionUsage>();
		}

		[StringLength(128)]
		public string StoreId { get; set; }

		[StringLength(128)]
		public string CatalogId { get; set; }

		[StringLength(128)]
		public string CouponCode { get; set; }

        [Required]
		[StringLength(128)]
		public string Name { get; set; }

     	[StringLength(1024)]
		public string Description { get; set; }

		public bool IsActive { get; set; }

		[Required]
		public DateTime StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public int Priority { get; set; }
		public bool IsExclusive { get; set; }

		public string PredicateSerialized { get; set; }

		public string PredicateVisualTreeSerialized { get; set; }

		public string RewardsSerialized { get; set; }

		public int PerCustomerLimit { get; set; }

		public int TotalLimit { get; set; }

       
        #region Navigation Properties

		public ObservableCollection<Coupon> Coupons { get; set; }

		public ObservableCollection<PromotionUsage> PromotionUsages { get; set; }

        #endregion

    }
}
