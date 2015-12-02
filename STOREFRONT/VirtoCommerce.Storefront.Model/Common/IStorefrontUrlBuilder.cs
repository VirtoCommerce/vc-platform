namespace VirtoCommerce.Storefront.Model.Common
{
    public interface IStorefrontUrlBuilder
    {
        string ToAppAbsolute(string virtualPath);
        string ToAppAbsolute(string virtualPath, Store store, Language language);
        string ToAppRelative(string virtualPath);
        string ToAppRelative(string virtualPath, Store store, Language language);
        string ToLocalPath(string virtualPath);
    }
}
