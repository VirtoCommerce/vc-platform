using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.ContentPublishing.Interfaces;

namespace VirtoCommerce.ManagementClient.DynamicContent.Model
{
    [Serializable]
    public sealed class StoreElement : KeyValueDictionaryElement
    {
        public StoreElement(IExpressionViewModel expressionViewModel)
        {
			InitializeAvailableValues(expressionViewModel);

            DefaultValue = AvailableValues.FirstOrDefault();
            InputDisplayName = "select Store".Localize();
        }

		public override void InitializeAvailableValues(IExpressionViewModel expressionViewModel)
        {
			using (var repository = ((IContentPublishingItemViewModel) expressionViewModel).StoreRepositoryFactory.GetRepositoryInstance())
            {
                var names = new List<KeyValuePair<string, string>>();
                repository.Stores.ToList().ForEach(x => names.Add(new KeyValuePair<string, string>(x.StoreId, x.Name)));
                AvailableValues = names.ToArray();
            }
        }
    }
}
