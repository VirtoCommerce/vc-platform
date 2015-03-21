using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.ComponentModel.DataAnnotations.Schema;


namespace VirtoCommerce.Foundation.Marketing.Model
{
	[DataContract]
	[EntitySet("PromotionRewards")]
	[DataServiceKey("PromotionRewardId")]
	public abstract class PromotionReward : StorageEntity
	{
		public PromotionReward()
		{
			_PromotionRewardId = GenerateNewKey();
		}

		private string _PromotionRewardId;
		[Key]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[DataMember]
		public string PromotionRewardId
		{
			get
			{
				return _PromotionRewardId;
			}
			set
			{
				SetValue(ref _PromotionRewardId, () => this.PromotionRewardId, value);
			}

		}

		private int _RewardAmountId;
		[DataMember]
		public int RewardAmountId
		{
			get
			{
				return _RewardAmountId;
			}
			set
			{
				SetValue(ref _RewardAmountId, () => this.RewardAmountId, value);
			}

		}

		private decimal _Amount;
		[DataMember]
		public decimal Amount
		{
			get
			{
				return _Amount;
			}
			set
			{
				SetValue(ref _Amount, () => this.Amount, value);
			}
		}

		private int _AmountTypeId;
		[DataMember]
		public int AmountTypeId
		{
			get
			{
				return _AmountTypeId;
			}
			set
			{
				SetValue(ref _AmountTypeId, () => this.AmountTypeId, value);
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


		public override string ToString()
		{
			return Amount.ToString() + " " + AmountType2String((RewardAmountType)AmountTypeId);
		}

		private static string AmountType2String(RewardAmountType amountType)
		{
			var retVal = "$";
			if (amountType == RewardAmountType.Relative)
			{
				retVal = "%";
			}
			else if (amountType == RewardAmountType.Gift)
			{
				retVal = "free";
			}
			return retVal;
		}
	}
}
