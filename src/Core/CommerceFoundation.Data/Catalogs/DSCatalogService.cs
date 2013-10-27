namespace VirtoCommerce.Foundation.Data.Catalogs
{
    using Microsoft.Practices.ServiceLocation;
    using VirtoCommerce.Foundation.Data.Infrastructure;
    using VirtoCommerce.Foundation.Catalogs.Repositories;

    [JsonSupportBehavior]
	public class DSCatalogService : DServiceBase<EFCatalogRepository>
	{
        /// <summary>
        /// Creates the data source.
        /// </summary>
        /// <returns>catalog repository instance</returns>
		protected override EFCatalogRepository CreateDataSource()
		{
            return ServiceLocator.Current.GetInstance<ICatalogRepository>() as EFCatalogRepository;
			//return new EFCatalogRepository(new CatalogEntityFactory());
		}
	}
}
