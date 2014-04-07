using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.AppConfig.Interfaces;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.AppConfig.Implementations
{
	public class AppConfigSettingsViewModel : HomeSettingsEditableViewModel<AppConfigSettingsItemViewModel>, IAppConfigSettingsViewModel
	{

		#region Dependencies

		private readonly IRepositoryFactory<IAppConfigRepository> _repositoryFactory;

		#endregion

		#region ctor

		public AppConfigSettingsViewModel(IRepositoryFactory<IAppConfigRepository> repositoryFactory,
			IAppConfigEntityFactory entityFactory, IViewModelsFactory<ICreateAppConfigSettingViewModel> wizardVMFactory,
			IViewModelsFactory<IAppConfigSettingEditViewModel> editVMFactory)
			: base(entityFactory, wizardVMFactory, editVMFactory)
		{
			_repositoryFactory = repositoryFactory;
		}

		#endregion

		#region HomeSettingsViewModel override Members

		protected override object LoadData()
		{
			var retVal = new List<AppConfigSettingsItemViewModel>();
			using (var repository = _repositoryFactory.GetRepositoryInstance())
			{
				if (repository != null)
				{
					retVal = repository.Settings.Expand(s => s.SettingValues)
								  .OrderBy(setting => setting.Created)
								  .Select(CreateItem)
								  .ToList();
				}
			}
			return retVal;
		}

		public override void RefreshItem(object item)
		{
			var itemToUpdate = item as Setting;
			if (itemToUpdate != null)
			{
				var vmItem = Items.SingleOrDefault(svm => svm.Id == itemToUpdate.SettingId);
				if (vmItem != null)
				{
					vmItem.SetProperties(itemToUpdate);
				}
			}
		}

		#endregion

		#region HomeSettingsEditableEditableViewModel override Members

		protected override AppConfigSettingsItemViewModel CreateItem(object itemFromRep)
		{
			var appConfig = new AppConfigSettingsItemViewModel();
			appConfig.SetProperties(itemFromRep as Setting);
			return appConfig;
		}

		protected override void RaiseItemAddInteractionRequest()
		{
			var itemFromRep = EntityFactory.CreateEntity<Setting>();
			var vm = WizardVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", itemFromRep));
			var confirmation = new Confirmation { Title = "Create setting".Localize(), Content = vm };
			ItemAdd(confirmation, _repositoryFactory.GetRepositoryInstance(), itemFromRep);
		}

		protected override void RaiseItemEditInteractionRequest(AppConfigSettingsItemViewModel item)
		{
			var itemFromRep = GetItemById(item.Id) as Setting;
			if (itemFromRep != null)
			{
				var editVM = EditVmFactory.GetViewModelInstance(
					new KeyValuePair<string, object>("item", itemFromRep),
					new KeyValuePair<string, object>("parent", this)
					);
				var openTracking = (IOpenTracking)editVM;
				openTracking.OpenItemCommand.Execute();
			}
		}

		protected override void RaiseItemDeleteInteractionRequest(AppConfigSettingsItemViewModel item)
		{
			ItemDelete(item, string.Format("Are you sure you want to delete setting '{0}'?".Localize(), item.Name),
					   _repositoryFactory.GetRepositoryInstance(), GetItemById(item.Id));
		}



		protected override bool CanRaiseItemDeleteExecute(AppConfigSettingsItemViewModel item)
		{
			return base.CanRaiseItemDeleteExecute(item) && !item.IsSystem;
		}

		protected override object GetItemById(string itemId)
		{
			if (string.IsNullOrEmpty(itemId))
				return null;

			var repository = _repositoryFactory.GetRepositoryInstance();
			return (repository != null)
					   ? repository.Settings.Where(s => s.SettingId == itemId).SingleOrDefault()
					   : null;
		}

		#endregion
	}
}
