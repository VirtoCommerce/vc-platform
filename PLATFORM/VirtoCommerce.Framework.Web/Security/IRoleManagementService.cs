namespace VirtoCommerce.Framework.Web.Security
{
    public interface IRoleManagementService
    {
        RoleDescriptor[] GetAllRoles();
        RoleDescriptor GetRole(string roleId);
        void AddOrUpdateRole(RoleDescriptor role);
    }
}
