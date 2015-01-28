using Microsoft.Practices.Unity;
using System.Collections.Generic;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Foundation.Money;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.OrderModule.Data;
using VirtoCommerce.OrderModule.Data.Repositories;

namespace VirtoCommerce.OrderModule.Web
{
	public class Module : IModule, IDatabaseModule
	{
		private readonly IUnityContainer _container;
		public Module(IUnityContainer container)
		{
			_container = container;
		}

		#region IModule Members

		public void Initialize()
		{
		}

		#endregion

		#region IDatabaseModule Members

		public void SetupDatabase(bool insertSampleData, bool reducedSampleData)
		{
			using (var context = new OrderRepositoryImpl())
			{
				var initializer = new OrderDatabaseInitializer();
				initializer.InitializeDatabase(context);
			}
		}

		#endregion
	}
}
