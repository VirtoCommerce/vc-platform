using Microsoft.Practices.Unity;
using System.Collections.Generic;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Foundation.Money;
using VirtoCommerce.Framework.Web.Modularity;

namespace VirtoCommerce.OrderModule.Web
{
    public class Module : IModule
    {
        private readonly IUnityContainer _container;
        public Module(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
          
        }
    }
}