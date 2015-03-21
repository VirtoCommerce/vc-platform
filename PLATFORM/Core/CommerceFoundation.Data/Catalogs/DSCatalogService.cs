namespace VirtoCommerce.Foundation.Data.Catalogs
{
    using Microsoft.Practices.ServiceLocation;
    using VirtoCommerce.Foundation.Data.Infrastructure;
    using VirtoCommerce.Foundation.Catalogs.Repositories;

    [JsonSupportBehavior]
	public class DSCatalogService : DServiceBase<EFCatalogRepository>
	{
	}
}
