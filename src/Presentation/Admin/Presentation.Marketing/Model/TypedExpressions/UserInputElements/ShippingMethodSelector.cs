using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Marketing.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Marketing.Model
{
	[Serializable]
	public sealed class ShippingMethodSelector : KeyValueDictionaryElement
	{
		public ShippingMethodSelector(IExpressionViewModel expressionViewModel)
		{
			InitializeAvailableValues(expressionViewModel);
			DefaultValue = AvailableValues.FirstOrDefault();
			InputDisplayName = "select Shipping".Localize();
		}

		public override void InitializeAvailableValues(IExpressionViewModel expressionViewModel)
		{
			var names = new List<KeyValuePair<string, string>>();

			using (var repository = ((IPromotionViewModel)expressionViewModel).ShippingRepositoryFactory.GetRepositoryInstance())
			{
				var allShippingMethods = repository.ShippingOptions.Expand(x => x.ShippingMethods)
					.ToList()
					.SelectMany(x => x.ShippingMethods)
					.Distinct()
					.ToList();

				allShippingMethods.ForEach(x => names.Add(new KeyValuePair<string, string>(x.ShippingMethodId, x.DisplayName)));
			}

			AvailableValues = names.ToArray();
		}
	}
}