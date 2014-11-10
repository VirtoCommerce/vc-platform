using Microsoft.Practices.Unity;
using Owin;
using System;
using VirtoCommerce.Framework.Web.Modularity;
using VirtoCommerce.SecurityModule.Web.Data;

namespace VirtoCommerce.SecurityModule.Web
{
    [Module(ModuleName = "SecurityModule", OnDemand = true)]
    public class Module : IModule
    {
        private readonly IUnityContainer _container;
        private readonly IAppBuilder _appBuilder;

        public Module(IUnityContainer container, IAppBuilder appBuilder)
        {
            _container = container;
            _appBuilder = appBuilder;
        }

        #region IModule Members

        public void Initialize()
        {
            OwinConfig.Configure(_appBuilder);

            #region Security

            _container.RegisterType<Func<IFoundationSecurityRepository>>(
                new InjectionFactory(x => new Func<IFoundationSecurityRepository>(() =>
                    new FoundationSecurityRepositoryImpl("VirtoCommerce"))));

            #endregion

            #region Customer

            _container.RegisterType<Func<IFoundationCustomerRepository>>(
                new InjectionFactory(x => new Func<IFoundationCustomerRepository>(() =>
                    new FoundationCustomerRepositoryImpl("VirtoCommerce"))));

            #endregion
        }

        #endregion
    }
}
