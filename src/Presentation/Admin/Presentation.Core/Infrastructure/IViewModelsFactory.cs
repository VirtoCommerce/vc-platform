using System;
using System.Collections.Generic;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
	public interface IViewModelsFactory<out T> where T : IViewModel
	{
		T GetViewModelInstance(params KeyValuePair<string, object>[] parameters);
	}
}
