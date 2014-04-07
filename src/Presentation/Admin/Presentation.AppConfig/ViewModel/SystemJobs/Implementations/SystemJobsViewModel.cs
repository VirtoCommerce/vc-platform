using System.Collections.Generic;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.SystemJobs.Interfaces;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.SystemJobs.Implementations
{
	public class SystemJobsViewModel : HomeSettingsEditableViewModel<SystemJob>, ISystemJobsViewModel
	{

		#region Dependencies

		private readonly IRepositoryFactory<IAppConfigRepository> _repositoryFactory;

		#endregion

		#region ctor

		public SystemJobsViewModel(IRepositoryFactory<IAppConfigRepository> repositoryFactory, IAppConfigEntityFactory entityFactory, IViewModelsFactory<ICreateSystemJobViewModel> wizardVmFactory, IViewModelsFactory<ISystemJobEditViewModel> editVmFactory)
			: base(entityFactory, wizardVmFactory, editVmFactory)
		{
			_repositoryFactory = repositoryFactory;
		}

		#endregion

		#region HomeSettingsViewModel

		protected override object LoadData()
		{
			using (var repository = _repositoryFactory.GetRepositoryInstance())
			{
				if (repository != null)
				{
					var items = repository.SystemJobs.OrderBy(cr => cr.Name).ToList();
					return items;
				}
			}

			return null;
		}

		public override void RefreshItem(object item)
		{
			var itemToUpdate = item as SystemJob;
			if (itemToUpdate != null)
			{
				SystemJob itemFromInnerItem =
					Items.SingleOrDefault(sj => sj.SystemJobId == itemToUpdate.SystemJobId);

				if (itemFromInnerItem != null)
				{
					OnUIThread(() =>
					{
						itemFromInnerItem.InjectFrom<CloneInjection>(itemToUpdate);
						OnPropertyChanged("Items");
					});
				}
			}
		}

		#endregion

		#region HomeSettingsEditableViewModel

		protected override void RaiseItemAddInteractionRequest()
		{
			var item = EntityFactory.CreateEntity<SystemJob>();

			var vm = WizardVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item));

			var confirmation = new ConditionalConfirmation()
			{
				Title = "Create System Job".Localize(),
				Content = vm
			};
			ItemAdd(item, confirmation, _repositoryFactory.GetRepositoryInstance());
		}

		protected override void RaiseItemEditInteractionRequest(SystemJob item)
		{
			var itemVM = EditVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item),
				new KeyValuePair<string, object>("parent", this));

			var openTracking = (IOpenTracking)itemVM;
			openTracking.OpenItemCommand.Execute();
		}

		protected override void RaiseItemDeleteInteractionRequest(SystemJob item)
		{
			var confirmation = new ConditionalConfirmation
			{
				Content = string.Format("Are you sure you want to delete System Job '{0}'?".Localize(), item.Name),
				Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};

			ItemDelete(item, confirmation, _repositoryFactory.GetRepositoryInstance());
		}

		#endregion

	}
}
