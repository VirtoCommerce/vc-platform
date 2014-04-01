using System.Linq;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Import.Model;
using VirtoCommerce.ManagementClient.Import.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Import.ViewModel.Implementations
{
	public class ColumnMappingViewModel : ViewModelBase, IColumnMappingViewModel
	{
		private const string Default = "Default";
		private const string Custom = "Custom";
		private readonly string SelectValue = "Select value...".Localize();
		public ColumnMappingEntity InnerItem { get; set; }

		public ColumnMappingViewModel(ColumnMappingEntity item)
		{
			InnerItem = item;

			if (InnerItem.CsvColumnsList != null && !InnerItem.CsvColumnsList.Any(x => x.Equals(Custom)))
			{
				var temp = InnerItem.CsvColumnsList.ToList();
				temp.Insert(0, Custom);
				InnerItem.CsvColumnsList = temp.ToArray();
			}

			if (string.IsNullOrEmpty(InnerItem.MappingItem.CsvColumnName))
				InnerItem.MappingItem.CsvColumnName = Custom;

			if (ColumnIsEnum && !InnerItem.ImportProperty.EnumValues.Any(x => x.Equals(Default) || x.Equals(SelectValue)))
				InnerItem.ImportProperty.EnumValues.Insert(0, InnerItem.ImportProperty.HasDefaultValue ? Default : SelectValue);

			if (ColumnIsEnum && string.IsNullOrEmpty(InnerItem.MappingItem.CustomValue))
				InnerItem.MappingItem.CustomValue = InnerItem.ImportProperty.HasDefaultValue ? Default : SelectValue;

			OnPropertyChanged();
        }

		public bool ColumnIsNotEnum
		{
			get
			{
				return !(InnerItem.ImportProperty != null && InnerItem.ImportProperty.IsEnumValuesProperty);
			}
		}

		public bool ColumnIsEnum
		{
			get
			{
				return (InnerItem.ImportProperty != null && InnerItem.ImportProperty.IsEnumValuesProperty);
			}
		}

		public bool IsColumnSelected
		{
			get
			{
				return string.IsNullOrEmpty(InnerItem.MappingItem.CsvColumnName) || InnerItem.MappingItem.CsvColumnName == Custom;
			}
		}

		public bool IsLocaleAvailable
		{
			get
			{
				return !string.IsNullOrEmpty(InnerItem.MappingItem.Locale);
			}
		}
				
		public bool Validate()
		{
			return (InnerItem.MappingItem.CsvColumnName == Custom && !string.IsNullOrEmpty(InnerItem.MappingItem.CustomValue) && !string.Equals(InnerItem.MappingItem.CustomValue, SelectValue)) ||
				InnerItem.MappingItem.CsvColumnName != Custom;
		}

		private void OnPropertyChanged()
		{
			OnPropertyChanged("InnerItem");
			OnPropertyChanged("ColumnIsNotEnum");
			OnPropertyChanged("ColumnIsEnum");
			OnPropertyChanged("IsColumnSelected");
		}

		public void RaiseCanExecuteChanged()
		{
			OnPropertyChanged("IsColumnSelected");
			OnPropertyChanged("ColumnIsNotEnum");
			OnPropertyChanged("ColumnIsEnum");
		}
	}
}
