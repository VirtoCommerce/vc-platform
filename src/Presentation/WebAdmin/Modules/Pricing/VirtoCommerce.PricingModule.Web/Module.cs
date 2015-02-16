using Microsoft.Practices.Unity;
using System.Collections.Generic;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Foundation.Money;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.PricingModule.Data;
using VirtoCommerce.PricingModule.Data.Repositories;
using System;
using VirtoCommerce.PricingModule.Data.Services;
using VirtoCommerce.Domain.Inventory.Services;
using VirtoCommerce.Domain.Cart.Services;
using VirtoCommerce.Domain.Order.Services;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Foundation.Frameworks.Workflow.Services;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Domain.Pricing.Services;

namespace VirtoCommerce.PricingModule.Web
{
	public class Module : IModule
	{
		private readonly IUnityContainer _container;
		public Module(IUnityContainer container)
		{
			_container = container;
		}

		#region IModule Members

		public void Initialize()
		{
			Func<IFoundationPricingRepository> pricingRepositoryFactory = () =>
			{
				return new FoundationPricingRepositoryImpl("VirtoCommerce", new AuditChangeInterceptor());
			};
			_container.RegisterType<Func<IFoundationPricingRepository>>(new InjectionFactory(x => pricingRepositoryFactory));

			_container.RegisterType<IPricingService, PricingServiceImpl>();
			
		}

		#endregion

		
	}
}
