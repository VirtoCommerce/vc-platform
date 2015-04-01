namespace VirtoCommerce.Framework.Web.Asset
{
    public interface IAssetsConnection
    {
        string OriginalConnectionString { get; }
        string Provider { get; }
        string ConnectionString { get; }
    }
}
