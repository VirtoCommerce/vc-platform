using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Security.Model
{
	[DataContract]
	[EntitySet("Accounts")]
	[DataServiceKey("AccountId")]
	public class Account : StorageEntity
	{
		public Account()
		{
			_accountId = GenerateNewKey();
		}

		private string _accountId;
		[Key]
		[DataMember]
		public string AccountId
		{
			get
			{
				return _accountId;
			}
			set
			{
				SetValue(ref _accountId, () => AccountId, value);
			}
		}

		private string _storeId;
		[DataMember]
		[StringLength(128)]
		public string StoreId
		{
			get
			{
				return _storeId;
			}
			set
			{
				SetValue(ref _storeId, () => StoreId, value);
			}
		}

		private string _memberId;
		[DataMember]
		[StringLength(64)]
		public string MemberId
		{
			get
			{
				return _memberId;
			}
			set
			{
				SetValue(ref _memberId, () => MemberId, value);
			}
		}

		private string _userName;
		[DataMember]
		[Required]
		[StringLength(128)]
		public string UserName
		{
			get
			{
				return _userName;
			}
			set
			{
				SetValue(ref _userName, () => UserName, value);
			}
		}

		/// <summary>
		/// registration type, can be 
		/// R - Registered user
		/// G - Guest User
		/// A - Administrator
		/// S - Site Administrator
		/// </summary>
		private int _registerType;
		[DataMember]
		public int RegisterType
		{
			get
			{
				return _registerType;
			}
			set
			{
				SetValue(ref _registerType, () => RegisterType, value);
			}
		}

		private int _accountState;
		[DataMember]
		public int AccountState
		{
			get
			{
				return _accountState;
			}
			set
			{
				SetValue(ref _accountState, () => AccountState, value);
			}
		}

		#region Navigation Properties

		private ObservableCollection<RoleAssignment> _roleAssignments;
		[DataMember]
		public ObservableCollection<RoleAssignment> RoleAssignments
		{
			get
			{
				return _roleAssignments ?? (_roleAssignments = new ObservableCollection<RoleAssignment>());
			}
		}

		private ObservableCollection<ApiAccount> _apiAccounts;
		public ObservableCollection<ApiAccount> ApiAccounts
		{
			get
			{
				return _apiAccounts ?? (_apiAccounts = new ObservableCollection<ApiAccount>());
			}
		}

		#endregion
	}
}
