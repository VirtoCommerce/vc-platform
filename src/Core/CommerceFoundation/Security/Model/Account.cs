using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
            _AccountId = GenerateNewKey();
		}

        private string _AccountId;
		[Key]
		[DataMember]
        public string AccountId
		{
			get
			{
                return _AccountId;
			}
			set
			{
                SetValue(ref _AccountId, () => this.AccountId, value);
			}
		}

        private string _StoreId;
        [DataMember]
        [StringLength(128)]
        public string StoreId
        {
            get
            {
                return _StoreId;
            }
            set
            {
                SetValue(ref _StoreId, () => this.StoreId, value);
            }
        }

        private string _MemberId;
        [DataMember]
        [StringLength(64)]
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

        private string _UserName;
        [DataMember]
        [Required]
        [StringLength(128)]
        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                SetValue(ref _UserName, () => this.UserName, value);
            }
        }

        /// <summary>
        /// registration type, can be 
        /// R - Registered user
        /// G - Guest User
        /// A - Administrator
        /// S - Site Administrator
        /// </summary>
        private int _RegisterType;
        [DataMember]
        public int RegisterType
        {
            get
            {
                return _RegisterType;
            }
            set
            {
                SetValue(ref _RegisterType, () => this.RegisterType, value);
            }
        }

        private int _AccountState;
        [DataMember]
        public int AccountState
        {
            get
            {
                return _AccountState;
            }
            set
            {
                SetValue(ref _AccountState, () => this.AccountState, value);
            }
        }

        #region Navigation Properties

        private ObservableCollection<RoleAssignment> _roleAssignments = null;
        [DataMember]
        public ObservableCollection<RoleAssignment> RoleAssignments
        {
            get
            {
                if (_roleAssignments == null)
                    _roleAssignments = new ObservableCollection<RoleAssignment>();

                return _roleAssignments;
            }
        }

        #endregion

    }
}
