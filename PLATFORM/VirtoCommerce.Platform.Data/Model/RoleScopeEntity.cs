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
        [StringLength(128)]
        public string Scope { get; set; }
        [Required]
        [StringLength(128)]
        public string Type { get; set; }

        #region Navigation properties
        public string RoleAssignmentId { get; set; }
        public RoleAssignmentEntity RoleAssignment { get; set; } 
        #endregion
    }
}
