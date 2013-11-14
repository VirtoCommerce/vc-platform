using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Order.Model.Builders;
using VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Wizard.Implementations
{
	public class ReturnItemViewModel : ViewModelBase, IReturnItemViewModel
	{
		const string SettingName_ReturnReasons = "ReturnReasons";
		private readonly IRepositoryFactory<IAppConfigRepository> _repository;

		public ReturnItemViewModel(IRepositoryFactory<IAppConfigRepository> repository)
		{
			_repository = repository;
			Initialize();
		}

		private async void Initialize()
		{
			QuantityToMove = 1;
			AvailableReasons = await Task.Run(() =>
				{
					string[] result = null;
					using (var repository = _repository.GetRepositoryInstance())
					{
						var setting = repository.Settings.Where(x => x.Name == SettingName_ReturnReasons).ExpandAll().SingleOrDefault();
						if (setting != null)
						{
							result = setting.SettingValues.Select(x => x.ShortTextValue).ToArray();
						}
					}
					return result;
				});

			if (AvailableReasons != null)
			{
				OnPropertyChanged("AvailableReasons");
				SelectedReason = AvailableReasons.FirstOrDefault();
				OnPropertyChanged("SelectedReason");
			}
		}

		public bool IsBulkReturnFalse { get { return !IsBulkReturn; } }

		#region IReturnItemViewModel Members

		public ReturnBuilder.ReturnLineItem ReturnLineItem
		{
			get;
			set;
		}

		public decimal QuantityToMove
		{
			get;
			set;
		}

		public string SelectedReason
		{
			get;
			set;
		}

		public string[] AvailableReasons
		{
			get;
			private set;
		}

		public bool IsBulkReturn { get; set; }

		#endregion

	}
}
