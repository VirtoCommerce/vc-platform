using VirtoCommerce.Client.Globalization;
#region Usings

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Marketing.Model;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing.Repositories;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.DynamicContent.Interfaces;
using VirtoCommerce.ManagementClient.Localization;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;

#endregion

namespace VirtoCommerce.ManagementClient.DynamicContent.ViewModel.DynamicContent.Implementations
{
	public class DynamicContentItemViewModel : ViewModelDetailBase<DynamicContentItem>, IDynamicContentItemViewModel
	{
		#region Dependencies

		private readonly IViewModelsFactory<IPropertyEditViewModel> _viewModelsFactory;
		private readonly IRepositoryFactory<IDynamicContentRepository> _repositoryFactory;
		private readonly INavigationManager _navManager;

		#endregion

		#region ctor

		public DynamicContentItemViewModel(IRepositoryFactory<IDynamicContentRepository> repositoryFactory,
			IDynamicContentEntityFactory entityFactory,
			DynamicContentItem item,
			INavigationManager navManager,
			IViewModelsFactory<IPropertyEditViewModel> viewModelsFactory)
			: base(entityFactory, item)
		{
			_viewModelsFactory = viewModelsFactory;
			_repositoryFactory = repositoryFactory;
			_navManager = navManager;
			CommandInit();

			ViewTitle = new ViewTitleBase
			{
                Title = "Dynamic Content",
				SubTitle = (item != null && !String.IsNullOrEmpty(item.Name)) ? item.Name.ToUpper(CultureInfo.InvariantCulture) : ""
			};

			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));
		}

		private void CommandInit()
		{
			PropertyValueEditCommand = new DelegateCommand<DynamicContentItemProperty>(RaisePropertyValueEditInteractionRequest, x => x != null);
			PropertyValueDeleteCommand = new DelegateCommand<object>(RaisePropertyValueDeleteInteractionRequest, x => x != null);
		}

		#endregion

		public DelegateCommand<object> PropertyValueDeleteCommand { get; private set; }
		public DelegateCommand<DynamicContentItemProperty> PropertyValueEditCommand
		{
			get;
			private set;
		}

		public bool IsWizardMode
		{
			get { return false; }
		}

		#region ViewModelBase overrides

		public override sealed string DisplayName
		{
			get
			{
				return string.IsNullOrEmpty(OriginalItem.Name) ? "" : OriginalItem.Name;
			}
		}

		public override sealed string IconSource
		{
			get
			{
				return "Icon_Dynamic";
			}
		}

		public override Brush ShellDetailItemMenuBrush
		{
			get
			{
				var result =
					(SolidColorBrush)Application.Current.TryFindResource("DynamicContentDetailItemMenuBrush");

				return result ?? base.ShellDetailItemMenuBrush;
			}
		}

		private NavigationItem _navigationData;
		public override NavigationItem NavigationData
		{
			get
			{
				return _navigationData ??
					   (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.DynamicContentItemId),
														NavigationNames.HomeName, NavigationNames.MenuName, this));
			}
		}

		#endregion

		#region IDynamicContentItemViewModel

		public void Duplicate(IDynamicContentRepository repository)
		{
			var original = repository.Items.Expand(x => x.PropertyValues).ToArray()
				.FirstOrDefault(x => x.DynamicContentItemId == InnerItem.DynamicContentItemId);
			var item = original.DeepClone(EntityFactory as IKnownSerializationTypes);
			item.DynamicContentItemId = item.GenerateNewKey();
			item.Name = item.Name + "_1";

			item.PropertyValues.ToList().ForEach(x =>
			{
				x.DynamicContentItemId = item.DynamicContentItemId;
				x.PropertyValueId = x.GenerateNewKey();
			});

			repository.Add(item);
		}

		#endregion

		#region ViewModelDetailBase
		public override string ExceptionContextIdentity { get { return string.Format("Dynamic content ({0})", DisplayName); } }

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
                Content = string.Format("Save changes to Dynamic Content item '{0}'?".Localize(), InnerItem.Name),
				Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
			};
		}

		protected override void LoadInnerItem()
		{
			var item = (Repository as IDynamicContentRepository).Items.Where(comment => comment.DynamicContentItemId == InnerItem.DynamicContentItemId).Expand(x => x.PropertyValues).SingleOrDefault();
			OnUIThread(() => InnerItem = item);
		}

		protected override void DoSaveChanges()
		{
			if (HasPermission())
			{
				try
				{
					Repository.UnitOfWork.Commit();
					OnUIThread(() => OriginalItem.InjectFrom(InnerItem));
				}
				catch (Exception ex)
				{
					ShowErrorDialog(ex, string.Format("An error occurred when trying to save {0}".Localize(null, LocalizationScope.DefaultCategory), ExceptionContextIdentity));
				}
			}
		}

		protected override void SetSubscriptionUI()
		{
			if (InnerItem.PropertyValues != null)
			{
				InnerItem.PropertyValues.CollectionChanged += ViewModel_PropertyChanged;
				InnerItem.PropertyValues.ToList().ForEach(param => param.PropertyChanged += ViewModel_PropertyChanged);
			}
		}

		protected override void CloseSubscriptionUI()
		{
			if (InnerItem.PropertyValues != null)
			{
				InnerItem.PropertyValues.CollectionChanged -= ViewModel_PropertyChanged;
				InnerItem.PropertyValues.ToList().ForEach(param => param.PropertyChanged -= ViewModel_PropertyChanged);
			}
		}
		#endregion

		#region private members

		//delete selected property value
		private void RaisePropertyValueDeleteInteractionRequest(object originalItemObject)
		{
			var originalItem = (DynamicContentItemProperty)originalItemObject;
			switch ((PropertyValueType)originalItem.ValueType)
			{
				case PropertyValueType.Integer:
					OnUIThread(() => InnerItem.PropertyValues.First(y => y.Name == originalItem.Name).IntegerValue = Int32.MinValue);
					break;
				case PropertyValueType.Decimal:
					OnUIThread(() => InnerItem.PropertyValues.First(y => y.Name == originalItem.Name).DecimalValue = Decimal.MinValue);
					break;
				case PropertyValueType.ShortString:
					InnerItem.PropertyValues.First(x => x.Name == originalItem.Name).ShortTextValue = null;
					break;
				case PropertyValueType.Category:
				case PropertyValueType.Image:
				case PropertyValueType.LongString:
					InnerItem.PropertyValues.First(x => x.Name == originalItem.Name).LongTextValue = null;
					break;
			}
		}

		private void RaisePropertyValueEditInteractionRequest(DynamicContentItemProperty originalItem)
		{
			var item = originalItem.DeepClone(EntityFactory as IKnownSerializationTypes);
			var itemVM = _viewModelsFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item)
				);

            var confirmation = new ConditionalConfirmation { Title = "Enter property value".Localize(), Content = itemVM };

			CommonConfirmRequest.Raise(confirmation, (x) =>
			{
				if (x.Confirmed)
				{
					switch ((PropertyValueType)item.ValueType)
					{
						case PropertyValueType.Integer:
							OnUIThread(() => InnerItem.PropertyValues.First(y => y.Name == item.Name).IntegerValue = item.IntegerValue);
							break;
						case PropertyValueType.Decimal:
							OnUIThread(() => InnerItem.PropertyValues.First(y => y.Name == item.Name).DecimalValue = item.DecimalValue);
							break;
						case PropertyValueType.ShortString:
							OnUIThread(() => InnerItem.PropertyValues.First(y => y.Name == item.Name).ShortTextValue = item.ShortTextValue);
							break;
						case PropertyValueType.Image:
						case PropertyValueType.LongString:
							OnUIThread(() => InnerItem.PropertyValues.First(y => y.Name == item.Name).LongTextValue = item.LongTextValue);
							break;
						case PropertyValueType.Category:
							OnUIThread(() => InnerItem.PropertyValues.First(y => y.Name == item.Name).LongTextValue = item.LongTextValue);
							OnUIThread(() => InnerItem.PropertyValues.First(y => y.Name == item.Name).Alias = item.Alias);
							break;
						case PropertyValueType.Boolean:
							OnUIThread(() => InnerItem.PropertyValues.First(y => y.Name == item.Name).BooleanValue = item.BooleanValue);
							break;
					}
				}
			});
		}

		public void RaiseCanExecuteChanged()
		{
			PropertyValueEditCommand.RaiseCanExecuteChanged();
			PropertyValueDeleteCommand.RaiseCanExecuteChanged();
		}

		#endregion

	}
}
