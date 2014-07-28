using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Core.Controls;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.ContentPublishing.Interfaces;

namespace VirtoCommerce.ManagementClient.DynamicContent.Model
{
    [Serializable]
    public sealed class LanguageElement : KeyValueDictionaryElement
    {
        public LanguageElement(IExpressionViewModel expressionViewModel)
        {
            InitializeAvailableValues(expressionViewModel);

            DefaultValue = AvailableValues.FirstOrDefault();
            InputDisplayName = "select language".Localize();
        }

        public override void InitializeAvailableValues(IExpressionViewModel expressionViewModel)
        {
            using (var repository = ((IContentPublishingItemViewModel)expressionViewModel).AppConfigRepositoryFactory.GetRepositoryInstance())
            {
                var langSetting = repository.Settings
                    .Where(s => s.Name == "Languages")
                    .ExpandAll()
                    .Single();
                AvailableValues = langSetting.SettingValues.Select(x => new KeyValuePair<string, string>(
                    x.ShortTextValue,
                    CultureInfo.GetCultureInfo(x.ShortTextValue).DisplayName)).ToArray();
            }
        }
    }
}
