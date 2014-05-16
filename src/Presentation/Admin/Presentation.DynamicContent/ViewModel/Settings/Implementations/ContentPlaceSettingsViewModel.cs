using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Settings.Interfaces;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Wizard;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using System.Collections.Generic;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Settings.Implementations
{
	public class ContentPlaceSettingsViewModel : HomeSettingsEditableViewModel<DynamicContentPlace>, IContentPlaceSettingsViewModel 
	{

        #region Dependencies

        private readonly IRepositoryFactory<IDynamicContentRepository> _repositoryFactory;

        #endregion

        public ContentPlaceSettingsViewModel(IRepositoryFactory<IDynamicContentRepository> repositoryFactory, IDynamicContentEntityFactory entityFactory, IViewModelsFactory<ICreateContentPlaceViewModel> wizardVmFactory, IViewModelsFactory<IContentPlaceViewModel> editVmFactory)
            : base(entityFactory, wizardVmFactory, editVmFactory)
		{
            _repositoryFactory = repositoryFactory;
        }

		#region HomeSettingsViewModel members

		protected override object LoadData()
		{
			using (var repository = _repositoryFactory.GetRepositoryInstance())
			{
				if (repository != null)
				{
					var items = repository.Places.OrderBy(dcp => dcp.Name).ToList();
					return items;
				}
			}
			return null;
		}


	    public override void RefreshItem(object item)
	    {
            var itemToUpdate = item as DynamicContentPlace;
            if (itemToUpdate != null)
            {
                DynamicContentPlace itemFromInnerItem =
                    Items.SingleOrDefault(dcp => dcp.DynamicContentPlaceId == itemToUpdate.DynamicContentPlaceId);

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

		#region HomeSettingsEditableViewModel members

		protected override void RaiseItemAddInteractionRequest()
		{
			var item = EntityFactory.CreateEntity<DynamicContentPlace>();

			var vm = WizardVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item));
			var confirmation = new ConditionalConfirmation()
				{
                    Title = "Create content place".Localize(),
					Content = vm
				};
			ItemAdd(item, confirmation, _repositoryFactory.GetRepositoryInstance());
		}

		protected override void RaiseItemEditInteractionRequest(DynamicContentPlace item)
		{
			var itemVM = EditVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item),
				new KeyValuePair<string, object>("parent", this)
				);

			var openTracking = (IOpenTracking)itemVM;
			openTracking.OpenItemCommand.Execute();
		}

		protected override void RaiseItemDeleteInteractionRequest(DynamicContentPlace item)
		{
			var confirmation = new ConditionalConfirmation
			{
                Content = string.Format("Are you sure you want to delete Content place '{0}'?".Localize(), item.Name),
				Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};

			ItemDelete(item, confirmation, _repositoryFactory.GetRepositoryInstance());
		}

		#endregion


	}
}
