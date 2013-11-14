using System.Collections.Specialized;
using Omu.ValueInjecter;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.ManagementClient.Order.Model.Settings
{
    public class GeneralPropertyValue : NotifyPropertyChanged
    {
        public GeneralPropertyValue(object item = null)
        {
            if (item != null)
            {
                this.InjectFrom(item);

                var propInfo = item.GetType().FindPropertiesWithAttribute(typeof(KeyAttribute)).First();
                Id = propInfo.GetValue(item) as string;
            }
        }

        [Key]
        public string Id { get; set; }

        private string _Name;
        [StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; OnPropertyChanged(); }
        }

        private string _DisplayName;
        public string DisplayName
        {
            get { return _DisplayName; }
            set { _DisplayName = value; OnPropertyChanged(); }
        }

        private int _ValueType;
        public int ValueType
        {
            get { return _ValueType; }
            set { _ValueType = value; OnPropertyChanged(); }
        }

        private string _ShortTextValue;
        [StringLength(512, ErrorMessage = "Only 512 characters allowed.")]
        public string ShortTextValue
        {
            get { return _ShortTextValue; }
            set { _ShortTextValue = value; OnPropertyChanged(); }
        }

        private bool _BooleanValue;
        public bool BooleanValue
        {
            get { return _BooleanValue; }
            set { _BooleanValue = value; OnPropertyChanged(); }
        }

        public StringDictionary DictionaryValues { get; set; }

        /*  TODO: implement other types
        private string _LongTextValue;
        public string LongTextValue
        {
            get { return _LongTextValue; }
            set { _LongTextValue = value; OnPropertyChanged(); }
        }
        private decimal _DecimalValue;
        private int _IntegerValue;
        private DateTime? _DateTimeValue;
        */
    }
}
