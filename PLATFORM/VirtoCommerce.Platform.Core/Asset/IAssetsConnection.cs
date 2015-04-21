using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Asset
{
    public interface IAssetsConnection
    {
        string OriginalConnectionString { get; }
        string Provider { get; }
        string ConnectionString { get; }
        IReadOnlyDictionary<string, string> Parameters { get; }
    }
}
