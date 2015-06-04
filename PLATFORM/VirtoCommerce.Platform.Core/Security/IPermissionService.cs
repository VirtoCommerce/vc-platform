namespace VirtoCommerce.Platform.Core.Security
{
    public interface IPermissionService
    {
        bool UserHasAnyPermission(string userName, params string[] permissionIds);
        Permission[] GetAllPermissions();
        string[] GetUserPermissionIds(string userName);
    }
}
