using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Services.Common;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Attributes;

namespace VirtoCommerce.Foundation.Security.Model
{
    [DataContract]
    [EntitySet("RolePermissions")]
    [DataServiceKey("RolePermissionId")]
    public class RolePermission : StorageEntity 
    {
        public RolePermission()
        {
            _RolePermissionId = GenerateNewKey();
        }


        private string _RolePermissionId;
        [Key]
        [DataMember]
        [StringLength(64)]
        public string RolePermissionId
        {
            get { return _RolePermissionId; }
            set
            {
                SetValue(ref _RolePermissionId,()=>this.RolePermissionId,value);
            }
        }



        #region NavigationProperties

        private string _RoleId;
        [DataMember]
        [StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
        public string RoleId
        {
            get { return _RoleId; }
            set
            {
                SetValue(ref _RoleId,()=>this.RoleId,value);
            }
        }

        [DataMember]
		[ForeignKey("RoleId")]
		[Parent]
        public virtual Role Role { get; set; }


        private string _PermissionId;
        [DataMember]
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string PermissionId
        {
            get { return _PermissionId; }
            set
            {
                SetValue(ref _PermissionId, ()=>PermissionId,value);
            }
        }

        [DataMember]
		[ForeignKey("PermissionId")]
		[Parent]
        public virtual Permission Permission { get; set; }

        #endregion

    }
}
