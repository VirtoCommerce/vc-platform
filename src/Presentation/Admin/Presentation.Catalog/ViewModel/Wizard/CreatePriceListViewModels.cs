using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Pricelists.Implementations;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Wizard
{
	public class CreatePriceListViewModel : WizardContainerStepsViewModel, ICreatePriceListViewModel
    {
        public CreatePriceListViewModel(IViewModelsFactory<IPriceListOverviewStepViewModel> vmFactory, Pricelist item)
        {
            RegisterStep(vmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item)));
        }
    }

    public class PriceListOverviewStepViewModel : PriceListViewModel, IPriceListOverviewStepViewModel
    {
        public PriceListOverviewStepViewModel(IRepositoryFactory<IPricelistRepository> repositoryFactory,
            IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory, 
            ICatalogEntityFactory entityFactory, IAuthenticationContext authContext, Pricelist item)
            : base(repositoryFactory, appConfigRepositoryFactory,  entityFactory, authContext, item)
        {
        }
		
		#region IWizardStep Members

		public override bool IsValid
		{
			get
			{
				bool doNotifyChanges = false;
				InnerItem.Validate(doNotifyChanges);
				var retval = InnerItem.Errors.Count == 0;
				InnerItem.Errors.Clear();
				return retval;
			}
		}

		public override bool IsLast { get { return true; } }

		public override string Description { get { return "Enter price list general information.".Localize(); } }

		#endregion

		#region ViewModelDetailBase

		protected override void InitializePropertiesForViewing()
		{
			InitializeAvailableCurrencies();
		}
		
		#endregion
    }
}
