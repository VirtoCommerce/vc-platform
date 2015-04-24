using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Data.Model
{
    public class RoleAssignmentEntity : AuditableEntity
    {
        /// <summary>
        /// Organization within which member has a defined role
        /// </summary>
        public string OrganizationId { get; set; }
        public string AccountId { get; set; }
        public string RoleId { get; set; }

        public RoleEntity Role { get; set; }
        public AccountEntity Account { get; set; }
    }
}
