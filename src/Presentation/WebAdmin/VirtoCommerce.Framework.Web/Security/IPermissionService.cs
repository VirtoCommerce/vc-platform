namespace VirtoCommerce.Framework.Web.Security
{
    public interface IPermissionService
    {
        bool UserHasAnyPermission(string userName, params string[] permissionIds);
        PermissionDescriptor[] GetAllPermissions();
        string[] GetUserPermissionIds(string userName);
    }
}
