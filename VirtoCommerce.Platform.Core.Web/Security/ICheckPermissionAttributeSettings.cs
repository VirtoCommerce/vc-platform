namespace VirtoCommerce.Platform.Core.Web.Security
{
    public interface ICheckPermissionAttributeSettings
    {
        string RegularCookieAuthenticationType { get; }

        string LimitedCookieAuthenticationType { get; }

        string[] LimitedCookiePermissions { get; }
    }
}
