using System.ComponentModel;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
	public class CatalogDeleteViewModel : ViewModelBase, ICatalogDeleteViewModel, IDataErrorInfo
	{
		private readonly string confirmationPassword;
		public string ContentText { get; private set; }

		public CatalogDeleteViewModel(CatalogBase item, string contentText)
		{
			ContentText = contentText;
			confirmationPassword = (item.Name.Length > 5 ? item.Name.Substring(0, 5) : item.Name);
		}

		#region ICatalogDeleteViewModel Members

		public bool Validate()
		{
			if (!_confirmationTextChanged)
			{
				_confirmationTextChanged = true;
				OnPropertyChanged("ConfirmationText");
			}

			return ConfirmationText == confirmationPassword;
		}

		#endregion

		private string _confirmationText;
		bool _confirmationTextChanged;
		public string ConfirmationText
		{
			get { return _confirmationText; }
			set
			{
				_confirmationText = value;
				OnPropertyChanged();
				_confirmationTextChanged = true;
			}
		}

		#region IDataErrorInfo Members

		public string Error
		{
			get { return string.Empty; }
		}

		public string this[string columnName]
		{
			get
			{
				if (columnName == "ConfirmationText" && _confirmationTextChanged && !Validate())
				{
					return string.Format("Name begins with '{0}'.".Localize(), confirmationPassword);
				}
				return string.Empty;
			}
		}

		#endregion
	}
}
