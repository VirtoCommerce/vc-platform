using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ManagementClient.Localization;
using VirtoCommerce.ManagementClient.Order.Model.Settings;
using VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Wizard.Interfaces;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings.Wizard.Implementations
{
	public class GeneralLanguagesStepViewModel : WizardStepViewModelBase, IGeneralLanguagesStepViewModel
	{
		private readonly IViewModelsFactory<IGeneralLanguageViewModel> _vmFactory;
		private bool _IsLast;

		public GeneralLanguagesStepViewModel(IViewModelsFactory<IGeneralLanguageViewModel> vmFactory, IEnumerable selectedLanguages)
		{
			_vmFactory = vmFactory;
			_IsLast = true;

			InnerItems = new ObservableCollection<GeneralLanguage>();
			AddedItems = new Collection<GeneralLanguage>();
			UpdatedItems = new ObservableCollection<GeneralLanguage>();
			RemovedItems = new Collection<GeneralLanguage>();

			foreach (var x in selectedLanguages)
			{
				InnerItems.Add(new GeneralLanguage(x));
			}

			ItemAddCommand = new DelegateCommand(RaiseLanguageAddInteractionRequest);
			ItemEditCommand = new DelegateCommand<GeneralLanguage>(RaiseLanguageEditInteractionRequest, x => x != null);
			ItemRemoveCommand = new DelegateCommand<GeneralLanguage>(RaiseLanguageRemoveInteractionRequest, x => x != null);

			LanguageConfirmRequest = new InteractionRequest<ConditionalConfirmation>();
			LanguageCommonConfirmRequest = new InteractionRequest<Confirmation>();
		}

		public InteractionRequest<ConditionalConfirmation> LanguageConfirmRequest { get; private set; }
		public InteractionRequest<Confirmation> LanguageCommonConfirmRequest { get; private set; }

		public DelegateCommand ItemAddCommand { get; private set; }
		public DelegateCommand<GeneralLanguage> ItemEditCommand { get; private set; }
		public DelegateCommand<GeneralLanguage> ItemRemoveCommand { get; private set; }

		#region ICollectionChange<GeneralLanguage> Members

		public ObservableCollection<GeneralLanguage> InnerItems { get; set; }
		public ICollection<GeneralLanguage> AddedItems { get; private set; }
		public ObservableCollection<GeneralLanguage> UpdatedItems { get; private set; }
		public ICollection<GeneralLanguage> RemovedItems { get; private set; }

		public NotifyCollectionChangedEventHandler CollectionChanged
		{
			set
			{
				InnerItems.CollectionChanged += value;
				UpdatedItems.CollectionChanged += value;
			}
		}

		#endregion

		#region IWizardStep Members

		public override bool IsValid
		{
			get { return true; }
		}

		public override bool IsLast
		{
			get { return _IsLast; }
		}

		public override string Description
		{
			get { return "Define Languages.".Localize(); }
		}

		#endregion

		#region Commands Implementation

		private void RaiseLanguageAddInteractionRequest()
		{
			var item = new GeneralLanguage();
			if (RaiseLanguageEditInteractionRequest(item, "Create Language".Localize()))
			{
				AddedItems.Add(item);
				InnerItems.Add(item);
			}
		}

		private void RaiseLanguageEditInteractionRequest(GeneralLanguage originalItem)
		{
			var item = new GeneralLanguage(originalItem);
			if (RaiseLanguageEditInteractionRequest(item, "Edit Language".Localize()))
			{
				// copy all values to original:
				OnUIThread(() => originalItem.InjectFrom<CloneInjection>(item));
				if (!UpdatedItems.Contains(originalItem))
					UpdatedItems.Add(originalItem);
			}
		}

		private bool RaiseLanguageEditInteractionRequest(GeneralLanguage item, string title)
		{
			var result = false;

			var parameters = new[]
		        {
					new KeyValuePair<string, object>("item", item),
					new KeyValuePair<string, object>("selectedLanguages",InnerItems)
		        };
			var itemVM = _vmFactory.GetViewModelInstance(parameters);

			var confirmation = new ConditionalConfirmation { Title = title, Content = itemVM };
			LanguageConfirmRequest.Raise(confirmation, x =>
				{
					result = x.Confirmed;
				});

			return result;
		}

		private void RaiseLanguageRemoveInteractionRequest(GeneralLanguage selectedItem)
		{
			var confirmation = new ConditionalConfirmation
			{
				Title = "Remove confirmation".Localize(null, LocalizationScope.DefaultCategory),
				Content = string.Format("Are you sure you want to remove Language '{0}'?".Localize(), selectedItem.LanguageCode)
			};
			LanguageCommonConfirmRequest.Raise(confirmation,
				(x) =>
				{
					if (x.Confirmed)
					{
						if (AddedItems.Contains(selectedItem))
							AddedItems.Remove(selectedItem);
						else
						{
							if (UpdatedItems.Contains(selectedItem))
								UpdatedItems.Remove(selectedItem);
							RemovedItems.Add(selectedItem);
						}
						InnerItems.Remove(selectedItem);
					}
				});
		}

		#endregion

		public bool IsLast_2
		{
			set { _IsLast = value; }
		}
	}
}