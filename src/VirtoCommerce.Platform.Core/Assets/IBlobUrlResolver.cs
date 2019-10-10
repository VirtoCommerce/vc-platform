namespace VirtoCommerce.Platform.Core.Assets
{
    public interface IBlobUrlResolver
    {
        string GetAbsoluteUrl(string blobKey);
    }
}
