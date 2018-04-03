using System.Collections.Generic;

namespace VirtoCommerce.Platform.Core.Assets
{
    public interface IAssetEntryService
    {
        IEnumerable<AssetEntry> GetByIds(IEnumerable<string> ids);
        void SaveChanges(IEnumerable<AssetEntry> items);
        void Delete(IEnumerable<string> ids);
    }
}
