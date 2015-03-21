using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;


namespace VirtoCommerce.Foundation.Customers.Model
{
	[DataContract]
	[EntitySet("Contracts")]
	[DataServiceKey("ContractId")]
	public class Contract : StorageEntity
	{
		public Contract()
		{
			_ContractId = GenerateNewKey();
		}
		private string _ContractId;
		[Key]
		[DataMember]
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
		public string ContractId
		{
			get
			{
				return _ContractId;
			}
			set
			{
				SetValue(ref _ContractId, () => this.ContractId, value);
			}
		}

		private int _MemberType;
		[DataMember]
		public int MemberType
		{
			get
			{
				return _MemberType;
			}
			set
			{
				SetValue(ref _MemberType, () => this.MemberType, value);
			}
		}

		private int _ContractState;
		[DataMember]
		public int ContractState
		{
			get
			{
				return _ContractState;
			}
			set
			{
				SetValue(ref _ContractState, () => this.ContractState, value);
			}
		}

		private string _ContractVersion;
		[DataMember]
		[StringLength(32, ErrorMessage = "Only 32 characters allowed.")]
		public string ContractVersion
		{
			get
			{
				return _ContractVersion;
			}
			set
			{
				SetValue(ref _ContractVersion, () => this.ContractVersion, value);
			}
		}

		private string _Comments;
		[DataMember]
		[StringLength(512, ErrorMessage = "Only 512 characters allowed.")]
		public string Comments
		{
			get
			{
				return _Comments;
			}
			set
			{
				SetValue(ref _Comments, () => this.Comments, value);
			}
		}

		private bool _CreditAllowed;
		[DataMember]
		public bool CreditAllowed
		{
			get
			{
				return _CreditAllowed;
			}
			set
			{
				SetValue(ref _CreditAllowed, () => this.CreditAllowed, value);
			}
		}

		private decimal _CreditLimit;
		[DataMember]
		public decimal CreditLimit
		{
			get
			{
				return _CreditLimit;
			}
			set
			{
				SetValue(ref _CreditLimit, () => this.CreditLimit, value);
			}
		}

		private string _CreditLimitCurrency;
		[DataMember]
		[StringLength(16, ErrorMessage = "Only 16 characters allowed.")]
		public string CreditLimitCurrency
		{
			get
			{
				return _CreditLimitCurrency;
			}
			set
			{
				SetValue(ref _CreditLimitCurrency, () => this.CreditLimitCurrency, value);
			}
		}

		private decimal _SpendingBalance;
		[DataMember]
		public decimal SpendingBalance
		{
			get
			{
				return _SpendingBalance;
			}
			set
			{
				SetValue(ref _SpendingBalance, () => this.SpendingBalance, value);
			}
		}

		#region Navigation Properties

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

		[DataMember]
        [ForeignKey("MemberId")]
        [Parent]
		public Member Member { get; set; }
		#endregion
	}
}
