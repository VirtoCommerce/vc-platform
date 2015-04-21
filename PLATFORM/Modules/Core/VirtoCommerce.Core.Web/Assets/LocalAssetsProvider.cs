using VirtoCommerce.Foundation.Assets.Factories;
using VirtoCommerce.Foundation.Data.Asset;
using VirtoCommerce.Platform.Core.Asset;

namespace VirtoCommerce.CoreModule.Web.Assets
{
    public class LocalAssetsProvider : FileSystemBlobAssetRepository
    {
        public const string ProviderName = "LocalStorage";

        public LocalAssetsProvider(IAssetsConnection connection, IAssetEntityFactory entityFactory)
            : base(connection.Parameters["rootPath"], connection.Parameters["publicUrl"], entityFactory)
        {
        }
    }
}
