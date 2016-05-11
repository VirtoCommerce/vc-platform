namespace VirtoCommerce.Platform.Core.Asset
{
    public interface IBlobUrlResolver
    {
		string GetAbsoluteUrl(string blobKey);
    }
}
