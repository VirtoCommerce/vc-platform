using System.Collections.Generic;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using catalogModel = VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Wizard
{
	public class CreateCatalogViewModel : WizardContainerStepsViewModel, ICreateCatalogViewModel
	{
		public CreateCatalogViewModel(IViewModelsFactory<ICatalogOverviewStepViewModel> vmFactory, catalogModel.Catalog item)
		{
			var itemParameter = new KeyValuePair<string, object>("item", item);
			RegisterStep(vmFactory.GetViewModelInstance(itemParameter));
		}
	}

	public class CatalogOverviewStepViewModel : CatalogViewModel, ICatalogOverviewStepViewModel
	{
		public CatalogOverviewStepViewModel(IRepositoryFactory<ICatalogRepository> repositoryFactory, IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory, ICatalogEntityFactory entityFactory, catalogModel.Catalog item)
			: base(repositoryFactory, appConfigRepositoryFactory, entityFactory, item)
		{
		}

		public string ItemDefaultLanguage
		{
			set
			{
				ValidateDefaultLanguage(true);
			}
		}

		#region IWizardStep Members

		public override bool IsValid
		{
			get
			{
				var doNotifyChanges = false;
				InnerItem.Validate(doNotifyChanges);
				ValidateLanguages(doNotifyChanges);
				ValidateDefaultLanguage(doNotifyChanges);
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
				return "Catalog can contain Categories and Products and can be assigned to a Store".Localize();
			}
		}

		public override string Description
		{
			get
			{
				return "Enter Catalog details".Localize();
			}
		}

		#endregion

		#region private members
		private void ValidateDefaultLanguage(bool doNotifyChanges)
		{
			if (string.IsNullOrEmpty(InnerItem.DefaultLanguage) || InnerItem.DefaultLanguage.StartsWith("Select"))
				InnerItem.SetError("DefaultLanguage", "Field 'Default Language' is required.".Localize(), doNotifyChanges);
			else
				InnerItem.ClearError("DefaultLanguage");
		}

		private void ValidateLanguages(bool doNotifyChanges)
		{
			if (InnerItem.CatalogLanguages.Count == 0)
				InnerItem.SetError("Languages", "external validation error".Localize(), doNotifyChanges);
			else
				InnerItem.ClearError("Languages");
		}

		#endregion
	}
}
