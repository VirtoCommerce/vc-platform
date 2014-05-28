using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Customers.Factories;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CasePropertySets.Interfaces;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CasePropertySets.Implementations
{
	public class CasePropertySetViewModel : ViewModelDetailBase<CasePropertySet>, ICasePropertySetViewModel
	{
		#region Dependencies

		private readonly INavigationManager _navManager;
		private readonly IHomeSettingsViewModel _parent;
		private readonly IRepositoryFactory<ICustomerRepository> _repositoryFactory;
		private readonly IViewModelsFactory<ICasePropertyViewModel> _casePropertyVmFactory;

		#endregion

		#region Constructor

		public CasePropertySetViewModel(IViewModelsFactory<ICasePropertyViewModel> casePropertyVmFactory, IRepositoryFactory<ICustomerRepository> repositoryFactory, ICustomerEntityFactory entityFactory, IHomeSettingsViewModel parent,
			INavigationManager navManager, CasePropertySet item)
			: base(entityFactory, item)
		{
			ViewTitle = new ViewTitleBase()
				{
                    Title = "INFO",
                    SubTitle = (item != null && !string.IsNullOrEmpty(item.Name)) ? item.Name : ""
				};
			_casePropertyVmFactory = casePropertyVmFactory;
			_repositoryFactory = repositoryFactory;
			_navManager = navManager;
			_parent = parent;

			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));


			ItemAddCommand = new DelegateCommand(RaiseItemAddInteractionRequest);
			ItemEditCommand = new DelegateCommand<CaseProperty>(RaiseItemEditInteractionRequest, x => x != null);
			ItemDeleteCommand = new DelegateCommand<CaseProperty>(RaiseItemDeleteInteractionRequest, x => x != null);

			CommonWizardDialogRequest = new InteractionRequest<Confirmation>();
		}

		#endregion

		#region ViewModelBase members

		public override string DisplayName { get { return InnerItem == null ? string.Empty : InnerItem.Name; } }

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
					   (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.CasePropertySetId),
															Configuration.NavigationNames.HomeName, NavigationNames.MenuName,
															this));
			}
		}

		#endregion

		#region ViewModelDetailBase

		public override string ExceptionContextIdentity { get { return string.Format("Case property ({0})", DisplayName); } }

		protected override void GetRepository()
		{
			Repository = _repositoryFactory.GetRepositoryInstance();
		}

		protected override bool IsValidForSave()
		{
			return InnerItem.Validate();
		}

		protected override RefusedConfirmation CancelConfirm()
		{
			return new RefusedConfirmation
			{
				Content = string.Format("Save changes to Case property set '{0}'?".Localize(), DisplayName),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}

		protected override void LoadInnerItem()
		{
			var item =
				(Repository as ICustomerRepository).CasePropertySets.Where(
					x => x.CasePropertySetId == OriginalItem.CasePropertySetId)
					.ExpandAll()
					.SingleOrDefault();

			OnUIThread(() => InnerItem = item);
		}

		protected override void AfterSaveChangesUI()
		{
			if (_parent != null)
			{
				OriginalItem.InjectFrom<CloneInjection>(InnerItem);
				_parent.RefreshItem(OriginalItem);
			}
		}

		#endregion

		#region ICasePropertySetViewModel members

		public InteractionRequest<Confirmation> CommonWizardDialogRequest { get; private set; }

		public DelegateCommand ItemAddCommand { get; private set; }
		public DelegateCommand<CaseProperty> ItemEditCommand { get; private set; }
		public DelegateCommand<CaseProperty> ItemDeleteCommand { get; private set; }

		#endregion

		#region Command Implementation

		private void RaiseItemAddInteractionRequest()
		{
			var item = new CaseProperty();
			if (RaiseItemEditInteractionRequest(item, "Create Info Value".Localize()))
			{
				InnerItem.CaseProperties.Add(item);
				IsModified = true;
			}
		}

		private void RaiseItemEditInteractionRequest(CaseProperty originalItem)
		{
			var item = originalItem.DeepClone(EntityFactory as IKnownSerializationTypes);
			if (RaiseItemEditInteractionRequest(item, "Edit Info Value".Localize()))
			{
				// copy all values to original:
				OnUIThread(() => originalItem.InjectFrom<CloneInjection>(item));
				IsModified = true;
			}
		}

		private bool RaiseItemEditInteractionRequest(CaseProperty item, string title)
		{
			var result = false;

			var itemVM = _casePropertyVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item));
			var confirmation = new ConditionalConfirmation(item.Validate) { Title = title, Content = itemVM };

			CommonConfirmRequest.Raise(confirmation, (x) =>
			{
				result = x.Confirmed;
			});

			return result;
		}

		private void RaiseItemDeleteInteractionRequest(CaseProperty item)
		{
			var confirmation = new ConditionalConfirmation
			{
				Content = string.Format("Are you sure you want to delete Info Value '{0}'?".Localize(), item.Name),
				Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};

			CommonConfirmRequest.Raise(confirmation, (x) =>
			{
				if (x.Confirmed)
				{
					InnerItem.CaseProperties.Remove(item);
					IsModified = true;
				}
			});
		}

		#endregion
	}
}
