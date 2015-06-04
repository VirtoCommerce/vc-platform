using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Security.Services;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Data.Catalogs
{
	public class DSCatalogClient : DSClientBase, ICatalogRepository, IPricelistRepository
	{
		[InjectionConstructor]
		public DSCatalogClient(ICatalogEntityFactory catalogEntityFactory, ISecurityTokenInjector tokenInjector, IServiceConnectionFactory connFactory)
			: base(connFactory.GetConnectionString(CatalogConfiguration.Instance.Connection.DataServiceUri), catalogEntityFactory, tokenInjector)
		{
		}

        public DSCatalogClient(Uri serviceUri, ICatalogEntityFactory entityFactory, ISecurityTokenInjector tokenInjector)
			: base(serviceUri, entityFactory, tokenInjector)
		{
		}

		#region ICatalogRepository Members
		public IQueryable<CategoryBase> Categories
		{
			get { return GetAsQueryable<CategoryBase>(); }
		}

		public IQueryable<CatalogBase> Catalogs
		{
			get { return GetAsQueryable<CatalogBase>(); }
		}

		public IQueryable<Item> Items
		{
			get { return GetAsQueryable<Item>(); }
		}

		public IQueryable<Property> Properties
		{
			get { return GetAsQueryable<Property>(); }
		}

		public IQueryable<PropertySet> PropertySets
		{
			get { return GetAsQueryable<PropertySet>(); }
		}

		public IQueryable<ItemRelation> ItemRelations
		{
			get { return GetAsQueryable<ItemRelation>(); }
		}
		public IQueryable<CategoryItemRelation> CategoryItemRelations
		{
			get { return GetAsQueryable<CategoryItemRelation>(); }
		}

		public IQueryable<Packaging> Packagings
		{
			get { return GetAsQueryable<Packaging>(); }
		}

		public IQueryable<TaxCategory> TaxCategories
		{
			get { return GetAsQueryable<TaxCategory>(); }
		}

		public IQueryable<Association> Associations
		{
			get { return GetAsQueryable<Association>(); }
		}
		#endregion

		#region IPriceListRepository Members

		public IQueryable<Pricelist> Pricelists
		{
			get { return GetAsQueryable<Pricelist>(); }
		}

		public IQueryable<Price> Prices
		{
			get { return GetAsQueryable<Price>(); }
		}

		public IQueryable<PricelistAssignment> PricelistAssignments
		{
			get { return GetAsQueryable<PricelistAssignment>(); }
		}

		#endregion

	}
}
