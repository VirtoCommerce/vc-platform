namespace VirtoCommerce.Platform.Core.Security
{
    public interface IRoleManagementService
    {
        RoleSearchResponse SearchRoles(RoleSearchRequest request);
        Role GetRole(string roleId);
        void DeleteRole(string roleId);
        Role AddOrUpdateRole(Role role);
    }
}
