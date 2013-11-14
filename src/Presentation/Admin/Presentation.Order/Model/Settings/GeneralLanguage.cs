using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.ManagementClient.Order.Model.Settings
{
    public class GeneralLanguage : NotifyPropertyChanged
    {
        private string _displayName;
        private string _languageCode;

        public GeneralLanguage(object item = null)
        {
            if (item != null)
            {
                this.InjectFrom(item);

                var propInfo = item.GetType().FindPropertiesWithAttribute(typeof(KeyAttribute)).First();
                Id = propInfo.GetValue(item) as string ?? Guid.NewGuid().ToString();
            }
        }

        [Key]
        public string Id { get; set; }

        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; OnPropertyChanged(); }
        }

        public string LanguageCode
        {
            get { return _languageCode; }
            set { _languageCode = value; OnPropertyChanged(); }
        }
    }
}
