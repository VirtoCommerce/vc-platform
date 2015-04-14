namespace VirtoCommerce.Framework.Web.Security
{
    public interface IRoleManagementService
    {
        RoleSearchResponse SearchRoles(RoleSearchRequest request);
        RoleDescriptor GetRole(string roleId);
        void AddOrUpdateRole(RoleDescriptor role);
    }
}
