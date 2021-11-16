using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Assets
{
    [Obsolete("Deprecated. Use assets from Assets module.")]
    public interface IAssetEntryService
    {
        Task<IEnumerable<AssetEntry>> GetByIdsAsync(IEnumerable<string> ids);
        Task SaveChangesAsync (IEnumerable<AssetEntry> items);
        Task DeleteAsync (IEnumerable<string> ids);
    }
}
