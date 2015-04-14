using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Security.Model
{
    [DataContract]
    [EntitySet("Roles")]
    [DataServiceKey("RoleId")]
    public class Role : StorageEntity
    {
        public Role()
        {
            _RoleId = GenerateNewKey();
        }

        private string _RoleId;
        [Key]
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

        private string _Name;
        [Required]
        [DataMember]
        [StringLength(128)]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                SetValue(ref _Name, () => this.Name, value);
            }
        }

        #region Navigation Properties

        private ObservableCollection<RolePermission> _rolePermissions = null;
        [DataMember]
        public ObservableCollection<RolePermission> RolePermissions
        {
            get
            {
                if (_rolePermissions == null)
                {
                    _rolePermissions = new ObservableCollection<RolePermission>();
                }
                return _rolePermissions;
            }
            set { _rolePermissions = value; }
        }

        private ObservableCollection<RoleAssignment> _assignments = null;

        [DataMember]
        public ObservableCollection<RoleAssignment> RoleAssignments
        {
            get
            {
                if (_assignments == null)
                    _assignments = new ObservableCollection<RoleAssignment>();

                return _assignments;
            }
        }

        #endregion

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj == this)
            {
                return true;
            }

            bool retVal = false;
            Role otherRole = obj as Role;
            if (otherRole != null)
            {
                retVal = RoleId == otherRole.RoleId;
            }
            else
            {
                //default impl
                base.Equals(obj);
            }

            return retVal;
        }
    }
}
