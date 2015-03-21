using System;
using VirtoCommerce.Foundation.Frameworks;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtoCommerce.Foundation.Marketing.Model
{
    [DataContract]
    [EntitySet("Promotions")]
    [DataServiceKey("PromotionId")]
    public abstract class Promotion : StorageEntity
    {
        public Promotion()
        {
            _PromotionId = GenerateNewKey();
            ExclusionTypeId = (int)ExclusivityType.None;
        }

        private string _PromotionId;
        [Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        [DataMember]
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


        private string _Name;
        [Required(ErrorMessage = "Field 'Promotion name' is required.")]
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

        private string _Description;
        [DataMember]
		[StringLength(256, ErrorMessage = "Only 256 characters allowed.")]
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                SetValue(ref _Description, () => this.Description, value);
            }
        }

        private string _Status;
        /// <summary>
        /// Gets or sets the status. The status can be "Active", "Inactive", "Archived".
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        [DataMember]
		[StringLength(32, ErrorMessage = "Only 32 characters allowed.")]
        public string Status
        {
            get
            {
                return _Status;
            }
            set
            {
                SetValue(ref _Status, () => this.Status, value);
            }
        }

        private DateTime _StartDate;
        [Required(ErrorMessage = "Field 'Enable date' is required.")]
        [DataMember]
        public DateTime StartDate
        {
            get
            {
                return _StartDate;
            }
            set
            {
                SetValue(ref _StartDate, () => this.StartDate, value);
            }
        }

        private DateTime? _EndDate;
        [DataMember]
        public DateTime? EndDate
        {
            get
            {
                return _EndDate;
            }
            set
            {
                SetValue(ref _EndDate, () => this.EndDate, value);
            }
        }


        private int _Priority;
        [DataMember]
        public int Priority
        {
            get
            {
                return _Priority;
            }
            set
            {
                SetValue(ref _Priority, () => this.Priority, value);
            }
        }

        private string _PredicateSerialized;
        [DataMember]
        public string PredicateSerialized
        {
            get
            {
                return _PredicateSerialized;
            }
            set
            {
                SetValue(ref _PredicateSerialized, () => this.PredicateSerialized, value);
            }
        }

        private string _PredicateVisualTreeSerialized;
        [DataMember]
        public string PredicateVisualTreeSerialized
        {
            get
            {
                return _PredicateVisualTreeSerialized;
            }
            set
            {
                SetValue(ref _PredicateVisualTreeSerialized, () => this.PredicateVisualTreeSerialized, value);
            }
        }

        private int _PerCustomerLimit;
        [DataMember]
        public int PerCustomerLimit
        {
            get
            {
                return _PerCustomerLimit;
            }
            set
            {
                SetValue(ref _PerCustomerLimit, () => this.PerCustomerLimit, value);
            }
        }

        private int _TotalLimit;
        [DataMember]
        public int TotalLimit
        {
            get
            {
                return _TotalLimit;
            }
            set
            {
                SetValue(ref _TotalLimit, () => this.TotalLimit, value);
            }
        }

        private int _ExclusionTypeId;

        [DataMember]
        public int ExclusionTypeId
        {
            get
            {
                return _ExclusionTypeId;
            }
            set
            {
                SetValue(ref _ExclusionTypeId, () => this.ExclusionTypeId, value);
            }
        }

        #region Navigation Properties

        private string _SegmentSetId;
        [DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        [ForeignKey("SegmentSet")]
        public string SegmentSetId
        {
            get
            {
                return _SegmentSetId;
            }
            set
            {
                SetValue(ref _SegmentSetId, () => this.SegmentSetId, value);
            }
        }

        [DataMember]
        public virtual SegmentSet SegmentSet { get; set; }

        private string _CouponId;
        [DataMember]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        [ForeignKey("Coupon")]
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

        [DataMember]
        public Coupon Coupon { get; set; }


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


        private ObservableCollection<PromotionReward> _Rewards = null;
        [DataMember]
        public ObservableCollection<PromotionReward> Rewards
        {
            get
            {
                if (_Rewards == null)
                    _Rewards = new ObservableCollection<PromotionReward>();

                return _Rewards;
            }
        }

        #endregion

    }
}
