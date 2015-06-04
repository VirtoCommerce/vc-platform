using System.Collections.ObjectModel;
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

        public string StoreId { get; set; }
        public string MemberId { get; set; }
        public string UserName { get; set; }
        public RegisterType RegisterType { get; set; }
        public AccountState AccountState { get; set; }

		public virtual ObservableCollection<RoleAssignmentEntity> RoleAssignments { get; set; }
		public virtual ObservableCollection<ApiAccountEntity> ApiAccounts { get; set; }
    }
}
