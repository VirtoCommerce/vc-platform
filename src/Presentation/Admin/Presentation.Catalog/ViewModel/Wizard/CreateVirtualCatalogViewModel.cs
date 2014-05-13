using System.Collections.Generic;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Wizard
{
	public class CreateVirtualCatalogViewModel : WizardContainerStepsViewModel, ICreateVirtualCatalogViewModel
	{
		public CreateVirtualCatalogViewModel(IViewModelsFactory<IVirtualCatalogOverviewStepViewModel> vmFactory, VirtualCatalog item)
		{
			var itemParameter = new KeyValuePair<string, object>("item", item);
			RegisterStep(vmFactory.GetViewModelInstance(itemParameter));
		}
	}

	public class VirtualCatalogOverviewStepViewModel : VirtualCatalogViewModel, IVirtualCatalogOverviewStepViewModel
	{
		public VirtualCatalogOverviewStepViewModel(
			VirtualCatalog item, 
			IRepositoryFactory<ICatalogRepository> repositoryFactory, 
			IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory)
			: base(item, repositoryFactory, appConfigRepositoryFactory)
		{
		}

		#region IWizardStep Members

		public override bool IsValid
		{
			get
			{
				var doNotifyChanges = false;
				InnerItem.Validate(doNotifyChanges);
				var retval = InnerItem.Errors.Count == 0;
				InnerItem.Errors.Clear();
				return retval;
			}
		}

		public override bool IsLast
		{
			get
			{
				return true;
			}
		}

		public override string Comment
		{
			get
			{
				return "Virtual Catalog can contain categories and products from real Catalogs".Localize();
			}
		}

		public override string Description
		{
			get
			{
				return "Enter Virtual Catalog details".Localize();
			}
		}

		#endregion
	}
}
