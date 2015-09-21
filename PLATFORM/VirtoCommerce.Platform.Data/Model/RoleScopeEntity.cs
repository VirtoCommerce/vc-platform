using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Model
{
    public class RoleScopeEntity : Entity
    {
        [Required]
        [StringLength(1024)]
        public string Scope { get; set; }
     
        #region Navigation properties
        public string RoleId { get; set; }
        public RoleEntity Role { get; set; } 
        #endregion
    }
}
