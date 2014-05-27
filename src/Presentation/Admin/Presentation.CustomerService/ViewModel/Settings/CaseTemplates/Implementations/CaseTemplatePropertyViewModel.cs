using System;
using System.Collections.Generic;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseTemplates.Interfaces;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseTemplates.Implementations
{
	public class CaseTemplatePropertyViewModel : ViewModelBase, ICaseTemplatePropertyViewModel
	{
		#region Constructor

		public CaseTemplatePropertyViewModel(CaseTemplateProperty item)
		{
			InnerItem = item;

            ViewTitle = new ViewTitleBase() { Title = "Edit Info Value", SubTitle = "SETTINGS".Localize(null, LocalizationScope.DefaultCategory) };

			InitializePropertiesForViewing();
		}

		#endregion

		public CaseTemplateProperty InnerItem { get; private set; }

		public bool IsValid { get { return InnerItem.Validate(false); } }

		public List<PropertyValueType> PropertyTypes { get; private set; }

		#region private members
		private void InitializePropertiesForViewing()
		{
			InnerItem.PropertyChanged += InnerItem_PropertyChanged;

			var allValueTypes = (PropertyValueType[])Enum.GetValues(typeof(PropertyValueType));
			PropertyTypes = new List<PropertyValueType>(allValueTypes);
			PropertyTypes.Sort((x, y) => x.ToString().CompareTo(y.ToString()));

		}

		private void InnerItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			OnPropertyChanged("IsValid");
		}
		#endregion
	}
}