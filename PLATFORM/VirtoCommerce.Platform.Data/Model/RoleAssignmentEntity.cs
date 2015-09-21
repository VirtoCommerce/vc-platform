using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Model
{
    public class RoleAssignmentEntity : AuditableEntity
    {
        public string AccountId { get; set; }
        public string RoleId { get; set; }

        public virtual RoleEntity Role { get; set; }
		public virtual AccountEntity Account { get; set; }
      
    }
}
