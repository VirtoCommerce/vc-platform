namespace VirtoCommerce.Foundation.Assets.Services
{
    public interface IAssetUrlResolver
    {
        string GetAbsoluteUrl(string assetId, bool thumb = false);
        string GetRelativeUrl(string absoluteUrl);
    }
}
