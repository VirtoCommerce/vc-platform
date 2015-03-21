using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Inventories.Repositories;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Inventories.Factories;
using VirtoCommerce.Foundation.Inventories;
using VirtoCommerce.Foundation.Inventories.Model;
using VirtoCommerce.Foundation.Security.Services;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Data.Inventories
{
	public class DSInventoryClient : DSClientBase, IInventoryRepository
	{
		[InjectionConstructor]
		public DSInventoryClient(IInventoryEntityFactory factory, ISecurityTokenInjector tokenInjector, IServiceConnectionFactory connFactory)
			: base(connFactory.GetConnectionString(InventoryConfiguration.Instance.Connection.DataServiceUri), factory, tokenInjector)
		{
		}

        public DSInventoryClient(Uri serviceUri, IInventoryEntityFactory factory, ISecurityTokenInjector tokenInjector)
			: base(serviceUri, factory, tokenInjector)
		{
		}

		#region IInventoryRepository Members

		public IQueryable<Inventory> Inventories
		{
			get { return GetAsQueryable<Inventory>(); }
		}

		#endregion
	}

}
