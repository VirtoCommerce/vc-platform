using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Model
{
    public class AccountEntity : AuditableEntity
    {
        public AccountEntity()
        {
            RoleAssignments = new NullCollection<RoleAssignmentEntity>();
			ApiAccounts = new NullCollection<ApiAccountEntity>();
        }
        [StringLength(128)]
        public string StoreId { get; set; }
        [StringLength(64)]
        public string MemberId { get; set; }
        [Required]
        [StringLength(128)]
        public string UserName { get; set; }
        public RegisterType RegisterType { get; set; }
        public AccountState AccountState { get; set; }

		public virtual ObservableCollection<RoleAssignmentEntity> RoleAssignments { get; set; }
		public virtual ObservableCollection<ApiAccountEntity> ApiAccounts { get; set; }
    }
}
