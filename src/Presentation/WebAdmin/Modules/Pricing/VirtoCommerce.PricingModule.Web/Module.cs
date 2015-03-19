using Microsoft.Practices.Unity;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.PricingModule.Data.Repositories;
using System;
using VirtoCommerce.PricingModule.Data.Services;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
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
