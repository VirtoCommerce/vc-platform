using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.Bootstrapper.Main.Infrastructure
{
	public class UnityViewModelsFactory<T> : IViewModelsFactory<T> where T : IViewModel
	{
		private readonly IUnityContainer _container;

		public UnityViewModelsFactory(IUnityContainer container)
		{
			_container = container;
		}

		public T GetViewModelInstance(params KeyValuePair<string, object>[] parameters)
		{
			using (var retVal = parameters != null ? _container.Resolve<T>(parameters.Select(param => new ParameterOverride(param.Key, param.Value)).ToArray()) : _container.Resolve<T>())
			{
				return retVal;
			}
		}
	}
}
