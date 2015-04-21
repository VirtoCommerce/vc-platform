using System.Collections.Generic;

namespace VirtoCommerce.Framework.Web.Asset
{
    public interface IAssetsConnection
    {
        string OriginalConnectionString { get; }
        string Provider { get; }
        string ConnectionString { get; }
        IReadOnlyDictionary<string, string> Parameters { get; }
    }
}
