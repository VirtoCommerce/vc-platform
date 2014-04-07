using System.Collections.Generic;
using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.EmailTemplates.Interfaces;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.EmailTemplates.Implementations
{
	public class EmailTemplatesViewModel : HomeSettingsEditableViewModel<EmailTemplate>, IEmailTemplatesViewModel
	{

		#region Dependencies

		private readonly IRepositoryFactory<IAppConfigRepository> _repositoryFactory;

		#endregion

		#region Constructor

		public EmailTemplatesViewModel(IRepositoryFactory<IAppConfigRepository> repositoryFactory, IAppConfigEntityFactory entityFactory, IViewModelsFactory<ICreateEmailTemplateViewModel> wizardVmFactory, IViewModelsFactory<IEmailTemplateEditViewModel> editVmFactory)
			: base(entityFactory, wizardVmFactory, editVmFactory)
		{
			_repositoryFactory = repositoryFactory;
		}

		#endregion

		#region HomeSettingsViewModel members

		protected override object LoadData()
		{
			using (var repository = _repositoryFactory.GetRepositoryInstance())
			{
				if (repository != null)
				{
					var items = repository.EmailTemplates.OrderBy(cr => cr.Name).ToList();
					return items;
				}
			}

			return null;
		}

		public override void RefreshItem(object item)
		{
			var itemToUpdate = item as EmailTemplate;
			if (itemToUpdate != null)
			{
				EmailTemplate itemFromInnerItem =
					Items.SingleOrDefault(et => et.EmailTemplateId == itemToUpdate.EmailTemplateId);

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
			var item = EntityFactory.CreateEntity<EmailTemplate>();

			var vm = WizardVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item));

			var confirmation = new ConditionalConfirmation()
			{
				Title = "Create Email Template".Localize(),
				Content = vm
			};
			ItemAdd(item, confirmation, _repositoryFactory.GetRepositoryInstance());
		}

		protected override void RaiseItemEditInteractionRequest(EmailTemplate item)
		{
			var itemVM = EditVmFactory.GetViewModelInstance(
				 new KeyValuePair<string, object>("item", item),
				 new KeyValuePair<string, object>("parent", this));

			var openTracking = (IOpenTracking)itemVM;
			openTracking.OpenItemCommand.Execute();
		}

		protected override void RaiseItemDeleteInteractionRequest(EmailTemplate item)
		{
			var confirmation = new ConditionalConfirmation
			{
				Content = string.Format("Are you sure you want to delete Email Template '{0}'?".Localize(), item.Name),
				Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};

			ItemDelete(item, confirmation, _repositoryFactory.GetRepositoryInstance());
		}

		#endregion

	}
}
