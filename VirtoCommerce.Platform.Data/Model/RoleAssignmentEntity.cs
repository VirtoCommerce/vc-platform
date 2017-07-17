using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Model
{
    public class RoleAssignmentEntity : AuditableEntity
    {
        public string AccountId { get; set; }
        public string RoleId { get; set; }

        private string _roleName = null;
        [NotMapped]
        public string RoleName
        {
            get
            {
                if(_roleName == null)
                {
                    _roleName = Role?.Name;
                }
                return _roleName;
            }
            set
            {
                _roleName = value;
            }
        }
        public virtual RoleEntity Role { get; set; }
		public virtual AccountEntity Account { get; set; }
      
    }
}
