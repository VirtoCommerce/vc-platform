using System.Collections.ObjectModel;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Model
{
    public class AccountEntity : Entity
    {
        public AccountEntity()
        {
            RoleAssignments = new ObservableCollection<RoleAssignmentEntity>();
            ApiAccounts = new ObservableCollection<ApiAccountEntity>();
        }

        public string StoreId { get; set; }
        public string MemberId { get; set; }
        public string UserName { get; set; }
        public RegisterType RegisterType { get; set; }
        public AccountState AccountState { get; set; }

        public ObservableCollection<RoleAssignmentEntity> RoleAssignments { get; set; }
        public ObservableCollection<ApiAccountEntity> ApiAccounts { get; set; }
    }
}
