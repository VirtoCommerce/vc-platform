using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.AppConfig.Infrastructure.Enumerations;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.EmailTemplates.Interfaces;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.EmailTemplates.Implementations
{
	public class EmailTemplateEditViewModel : ViewModelDetailAndWizardBase<EmailTemplate>, IEmailTemplateEditViewModel
	{

		#region Dependencies

		private readonly IHomeSettingsViewModel _parent;
		private readonly IViewModelsFactory<IEmailTemplateAddLanguageViewModel> _vmFactory;
		private readonly INavigationManager _navManager;
		private readonly IRepositoryFactory<IAppConfigRepository> _repositoryFactory;

		#endregion

		#region ctor

		public EmailTemplateEditViewModel(IRepositoryFactory<IAppConfigRepository> repositoryFactory, IAppConfigEntityFactory entityFactory,
			IViewModelsFactory<IEmailTemplateAddLanguageViewModel> vmFactory, INavigationManager navManager, IHomeSettingsViewModel parent, EmailTemplate item)
			: base(entityFactory, item, false)
		{
			_repositoryFactory = repositoryFactory;
			_parent = parent;
			_navManager = navManager;
			_vmFactory = vmFactory;
            ViewTitle = new ViewTitleBase() { SubTitle = "SETTINGS".Localize(null, LocalizationScope.DefaultCategory), Title = "Email Template" };
			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));
			CommandInit();
		}

		protected EmailTemplateEditViewModel(IRepositoryFactory<IAppConfigRepository> repositoryFactory, IAppConfigEntityFactory entityFactory, IViewModelsFactory<IEmailTemplateAddLanguageViewModel> vmFactory, EmailTemplate item)
			: base(entityFactory, item, true)
		{
			_repositoryFactory = repositoryFactory;
			_vmFactory = vmFactory;
			CommandInit();
		}


		private void CommandInit()
		{
			AddEmailTemplateLanguageCommand = new DelegateCommand(AddEmailTemplateLanguage);
			EditEmailTemplateLanguageCommand = new DelegateCommand<EmailTemplateLanguage>(EditEmailTemplateLanguage,
																						  (x) =>
																						  SelectedEmailTemplateLanguage !=
																						  null);
			RemoveEmailTemplateLanguageCommand = new DelegateCommand<EmailTemplateLanguage>(
				RemoveEmailTemplateLanguage, (x) => SelectedEmailTemplateLanguage != null);

			AddEmailTemplateLanguageRequest = new InteractionRequest<ConditionalConfirmation>();
			RemoveEmailTemplateLanguageRequest = new InteractionRequest<ConditionalConfirmation>();
		}


		#endregion

		#region ViewModelBase Members

		public override string DisplayName
		{
			get
			{
				return InnerItem.Name;
			}
		}

		public override Brush ShellDetailItemMenuBrush
		{
			get
			{
				var result =
				  (SolidColorBrush)Application.Current.TryFindResource("SettingsDetailItemMenuBrush");

				return result ?? base.ShellDetailItemMenuBrush;
			}
		}

		private NavigationItem _navigationData;
		public override NavigationItem NavigationData
		{
			get
			{
				return _navigationData ??
					   (_navigationData = new NavigationItem(OriginalItem.EmailTemplateId,
															Configuration.NavigationNames.HomeName,
															NavigationNames.MenuName, this));
			}
		}

		#endregion

		#region ViewModelDetailAndWizardBase Members

		public override string ExceptionContextIdentity { get { return string.Format("Email template ({0})", DisplayName); } }

		protected override void GetRepository()
		{
			Repository = _repositoryFactory.GetRepositoryInstance();
		}

		protected override bool IsValidForSave()
		{
			bool result = false;

			result = InnerItem.Validate() && !string.IsNullOrEmpty(InnerItem.Body) &&
					 !string.IsNullOrEmpty(InnerItem.DefaultLanguageCode);

			return result;
		}

		/// <summary>
		/// Return RefusedConfirmation for Cancel Confirm dialog
		/// </summary>
		protected override RefusedConfirmation CancelConfirm()
		{
			return new RefusedConfirmation
			{
				Content = string.Format("Save changes to Email template '{0}'?".Localize(), InnerItem.Subject),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}

		protected override void LoadInnerItem()
		{
			var item = (Repository as IAppConfigRepository).EmailTemplates.Expand(et => et.EmailTemplateLanguages)
				.Where(et => et.EmailTemplateId == OriginalItem.EmailTemplateId)
				.SingleOrDefault();

			OnUIThread(() => { InnerItem = item; });
		}

		protected override void InitializePropertiesForViewing()
		{
			if (LanguagesCodes == null)
			{
				var repository = Repository as IAppConfigRepository;

				var langSetting =
					repository.Settings.Expand(s => s.SettingValues).Where(s => s.Name.Contains("Lang")).SingleOrDefault();

				OnUIThread(() =>
					{
						LanguagesCodes = langSetting.SettingValues.Select(sv => sv.ShortTextValue).ToList();
						OnPropertyChanged("LanguagesCodes");
					});
			}

			OnUIThread(() =>
				{
					SelectedEmailTemplateType = string.IsNullOrEmpty(InnerItem.Type) ?
						SelectedEmailTemplateType = EmailTemplateTypes.Xsl :
						(EmailTemplateTypes)Enum.Parse(typeof(EmailTemplateTypes), InnerItem.Type);
					OnPropertyChanged("SelectedEmailTemplateType");
				});

		}

		protected override void AfterSaveChangesUI()
		{
			if (_parent != null)
			{
				OriginalItem.InjectFrom<CloneInjection>(InnerItem);
				_parent.RefreshItemListCommand.Execute();
			}
		}

		protected override void SetSubscriptionUI()
		{
			if (InnerItem.EmailTemplateLanguages != null)
			{
				InnerItem.EmailTemplateLanguages.CollectionChanged += ViewModel_PropertyChanged;
			}
		}

		protected override void CloseSubscriptionUI()
		{
			if (InnerItem.EmailTemplateLanguages != null)
			{
				InnerItem.EmailTemplateLanguages.CollectionChanged -= ViewModel_PropertyChanged;
			}
		}

		#endregion

		#region IWizardStep Members

		public override bool IsValid
		{
			get
			{
				bool result = false;
				if (this is IEmailTemplateOverviewStepViewModel && InnerItem != null)
				{
					result = InnerItem.Validate() && !string.IsNullOrEmpty(InnerItem.DefaultLanguageCode) &&
							 !string.IsNullOrEmpty(InnerItem.Body) && !string.IsNullOrEmpty(InnerItem.Name)
							 && !string.IsNullOrEmpty(InnerItem.Type);
				}
				else
					if (this is IEmailTemplateLanguagesStepViewModel)
					{
						result = true;
					}

				return result;

			}
		}

		public override bool IsLast
		{
			get { return true; }
		}

		public override string Description
		{
			get { return "Email Template Details".Localize(); }
		}

		#endregion

		#region IEmailTemplateEditViewModel

		private List<string> _languagesCodes;
		public List<string> LanguagesCodes
		{
			get { return _languagesCodes; }
			set
			{
				_languagesCodes = value;
				OnPropertyChanged();
			}
		}


		private EmailTemplateLanguage _selectedEmailTemplateLanguage;
		public EmailTemplateLanguage SelectedEmailTemplateLanguage
		{
			get { return _selectedEmailTemplateLanguage; }
			set
			{
				_selectedEmailTemplateLanguage = value;
				OnPropertyChanged();
				EditEmailTemplateLanguageCommand.RaiseCanExecuteChanged();
				RemoveEmailTemplateLanguageCommand.RaiseCanExecuteChanged();
			}
		}


		private EmailTemplateTypes _selectedEmailTemplateType;
		public EmailTemplateTypes SelectedEmailTemplateType
		{
			get { return _selectedEmailTemplateType; }
			set
			{
				_selectedEmailTemplateType = value;
				OnPropertyChanged();
				if (InnerItem.Type != value.ToString())
				{
					InnerItem.Type = value.ToString();
				}
			}
		}

		public DelegateCommand AddEmailTemplateLanguageCommand { get; private set; }
		public DelegateCommand<EmailTemplateLanguage> EditEmailTemplateLanguageCommand { get; private set; }
		public DelegateCommand<EmailTemplateLanguage> RemoveEmailTemplateLanguageCommand { get; private set; }

		public InteractionRequest<ConditionalConfirmation> AddEmailTemplateLanguageRequest { get; private set; }
		public InteractionRequest<ConditionalConfirmation> RemoveEmailTemplateLanguageRequest { get; private set; }

		#endregion

		#region CommandImplementation

		private void AddEmailTemplateLanguage()
		{
			if (AddEmailTemplateLanguageRequest != null)
			{
				RaiseAddEditEmailTemplateLanguageRequest(new EmailTemplateLanguage(), OperationType.Add);
			}

		}

		private void EditEmailTemplateLanguage(EmailTemplateLanguage selectedItem)
		{
			var itemToUpdate = selectedItem.DeepClone(EntityFactory as IKnownSerializationTypes);

			if (AddEmailTemplateLanguageRequest != null)
			{
				RaiseAddEditEmailTemplateLanguageRequest(itemToUpdate, OperationType.Edit);
			}

		}


		private void RaiseAddEditEmailTemplateLanguageRequest(EmailTemplateLanguage selectedItem, OperationType operType)
		{
			var selectedLanguageCodes = InnerItem.EmailTemplateLanguages.Select(etl => etl.LanguageCode).ToArray();

			var parameters = new List<KeyValuePair<string, object>>();
			parameters.Add(new KeyValuePair<string, object>("item", selectedItem));
			parameters.Add(new KeyValuePair<string, object>("languageList", LanguagesCodes.ToArray()));
			parameters.Add(new KeyValuePair<string, object>("selectedLanguageList", selectedLanguageCodes));
			parameters.Add((operType == OperationType.Add) ?
				new KeyValuePair<string, object>("operationType", OperationType.Add) :
				new KeyValuePair<string, object>("operationType", OperationType.Edit));

			var itemVm = _vmFactory.GetViewModelInstance(parameters.ToArray());

			var confirmation = new ConditionalConfirmation();
			confirmation.Title = "Enter language details".Localize();
			confirmation.Content = itemVm;


			AddEmailTemplateLanguageRequest.Raise(confirmation,
				(x) =>
				{
					if (x.Confirmed)
					{

						if (operType == OperationType.Add)
						{
							var itemToAdd = (x.Content as IEmailTemplateAddLanguageViewModel).InnerItem;

							InnerItem.EmailTemplateLanguages.Add(itemToAdd);
						}
						else
						{
							var itemFromDialog = (x.Content as IEmailTemplateAddLanguageViewModel).InnerItem;

							var sameItemFromInnerItem =
								InnerItem.EmailTemplateLanguages.SingleOrDefault(
									etl => etl.EmailTemplateLanguageId == itemFromDialog.EmailTemplateLanguageId);
							if (sameItemFromInnerItem != null)
							{
								OnUIThread(() => sameItemFromInnerItem.InjectFrom<CloneInjection>(itemFromDialog));
								IsModified = true;
							}
						}
					}
				});

		}

		private void RemoveEmailTemplateLanguage(EmailTemplateLanguage selectedItem)
		{
			ConditionalConfirmation confirmation = new ConditionalConfirmation();
			confirmation.Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory);
			confirmation.Content = string.Format("Are you sure you want to delete language '{0}'?".Localize(), selectedItem.LanguageCode);

			if (RemoveEmailTemplateLanguageRequest != null)
			{
				RemoveEmailTemplateLanguageRequest.Raise(confirmation,
														 (x) =>
														 {
															 if (x.Confirmed)
															 {
																 if (selectedItem != null)
																 {
																	 InnerItem.EmailTemplateLanguages.Remove(
																		 selectedItem);
																 }
															 }
														 });
			}
		}
		#endregion

	}
}
