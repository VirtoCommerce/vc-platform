using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Newtonsoft.Json;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.Platform.Core.Security
{
    public class Permission : ValueObject
    {
        private const char _scopeCharSeparator = '|';

        public string Name { get; set; }
        /// <summary>
        /// Id of the module which has registered this permission.
        /// </summary>
        public string ModuleId { get; set; }
        /// <summary>
        /// Display name of the group to which this permission belongs. The '|' character is used to separate Child and parent groups.
        /// </summary>
        public string GroupName { get; set; }

        public IList<PermissionScope> AssignedScopes { get; set; } = new List<PermissionScope>();

        public IList<PermissionScope> AvailableScopes { get; } = new List<PermissionScope>();

        public static Permission TryCreateFromClaim(Claim claim, JsonSerializerSettings jsonSettings)
        {
            Permission result = null;
            if (claim != null && claim.Type.EqualsIgnoreCase(PlatformConstants.Security.Claims.PermissionClaimType))
            {
                result = AbstractTypeFactory<Permission>.TryCreateInstance();
                result.Name = claim.Value;
                if (result.Name.Contains(_scopeCharSeparator))
                {
                    var parts = claim.Value.Split(_scopeCharSeparator);
                    result.Name = parts.First();
                    result.AssignedScopes = JsonConvert.DeserializeObject<PermissionScope[]>(parts.Skip(1).FirstOrDefault(), jsonSettings);
                }
            }
            return result;
        }

        public virtual Claim ToClaim(JsonSerializerSettings jsonSettings)
        {
            var result = Name;
            if (!AssignedScopes.IsNullOrEmpty())
            {
                result += _scopeCharSeparator + JsonConvert.SerializeObject(AssignedScopes, jsonSettings);
            }
            return new Claim(PlatformConstants.Security.Claims.PermissionClaimType, result);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return ModuleId;
            if (!AssignedScopes.IsNullOrEmpty())
            {
                foreach (var scope in AssignedScopes)
                {
                    yield return scope;
                }
            }
        }

        public virtual void Patch(Permission target)
        {
            target.Name = Name;
            target.ModuleId = ModuleId;
            target.GroupName = GroupName;
            if (!AssignedScopes.IsNullOrEmpty())
            {
                AssignedScopes.Patch(target.AssignedScopes, (sourceScope, targetScope) => sourceScope.Patch(targetScope));
            }
            if (!AvailableScopes.IsNullOrEmpty())
            {
                AvailableScopes.Patch(target.AvailableScopes, (sourceScope, targetScope) => sourceScope.Patch(targetScope));
            }
        }

        #region ICloneable members
        public override object Clone()
        {
            var result = (Permission)MemberwiseClone();

            result.AssignedScopes = AssignedScopes?.Select(x => x.Clone()).OfType<PermissionScope>().ToList();

            return result;
        }
        #endregion
    }
}
