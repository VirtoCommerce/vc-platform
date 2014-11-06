using VirtoCommerce.Foundation.Frameworks;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Security.Model
{
	[DataContract]
	[EntitySet("RoleAssignments")]
	[DataServiceKey("RoleAssignmentId")]
	public class RoleAssignment : StorageEntity
	{
		public RoleAssignment()
		{
			_RoleAssignmentId = GenerateNewKey();
		}
		private string _RoleAssignmentId;
        [Key]
		[DataMember]
        [StringLength(64)]
		public string RoleAssignmentId
		{
			get
			{
				return _RoleAssignmentId;
			}
			set
			{
				SetValue(ref _RoleAssignmentId, () => this.RoleAssignmentId, value);
			}
		}

		private string _OrganizationId;
        /// <summary>
        /// Organization within which member has a defined role
        /// </summary>
		[DataMember]
        [StringLength(64)]
		public string OrganizationId
		{
			get
			{
				return _OrganizationId;
			}
			set
			{
				SetValue(ref _OrganizationId, () => this.OrganizationId, value);
			}
		}

        #region Navigaton Properties
        private string _AccountId;
		[Required]
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

        [DataMember]
        [Parent]
		[ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
	
		private string _RoleId;
		[Required]
		[DataMember]
        [StringLength(64)]
		public string RoleId
		{
			get
			{
				return _RoleId;
			}
			set
			{
				SetValue(ref _RoleId, () => this.RoleId, value);
			}
		}

		[DataMember]
        [Parent]
		[ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
		#endregion      
	}
}
