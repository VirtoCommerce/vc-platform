using VirtoCommerce.Foundation.Assets.Factories;
using VirtoCommerce.Foundation.Data.Azure.Asset;
using VirtoCommerce.Framework.Web.Asset;

namespace VirtoCommerce.CoreModule.Web.Assets
{
    public class AzureAssetsProvider : AzureBlobAssetRepository
    {
        public const string ProviderName = "AzureBlobStorage";

        public AzureAssetsProvider(IAssetsConnection connection, IAssetEntityFactory entityFactory)
            : base(connection.ConnectionString, entityFactory)
        {
        }
    }
}
