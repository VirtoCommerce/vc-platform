using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Marketing.Factories;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Wizard.Interfaces;
using VirtoCommerce.ManagementClient.Localization;
using coreModel = VirtoCommerce.Foundation.Marketing.Model;
using localModel = VirtoCommerce.ManagementClient.DynamicContent.Model;

namespace VirtoCommerce.ManagementClient.DynamicContent.ViewModel.Wizard.Implementations
{
	public class CreateDynamicContentItemViewModel : WizardViewModelBase, ICreateDynamicContentItemViewModel
	{
		public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }
		private readonly CreateDynamicContentItemViewModel _parentVM;
		private readonly IDynamicContentEntityFactory _entityFactory;
		private readonly IViewModelsFactory<IPropertyEditViewModel> _propertyEditVmFactory;

		public CreateDynamicContentItemViewModel(
			IViewModelsFactory<IPropertyEditViewModel> propertyEditVmFactory,
			IViewModelsFactory<IDynamicContentItemOverviewStepViewModel> overviewVmFactory,
			IViewModelsFactory<IDynamicContentItemPropertiesStepViewModel> propertiesVmFactory,
			IDynamicContentEntityFactory entityFactory, DynamicContentItem item)
		{
			_parentVM = this;
			_entityFactory = entityFactory;
			_propertyEditVmFactory = propertyEditVmFactory;

			var itemParameter = new KeyValuePair<string, object>("item", item);
			var parentVMParameter = new KeyValuePair<string, object>("parentVM", this);
			var entityFactoryParameter = new KeyValuePair<string, object>("entityFactory", _entityFactory);
			var propertyEditVMParameter = new KeyValuePair<string, object>("propertyEditVmFactory", propertyEditVmFactory);
			RegisterStep(overviewVmFactory.GetViewModelInstance(itemParameter, parentVMParameter, entityFactoryParameter));
			RegisterStep(propertiesVmFactory.GetViewModelInstance(itemParameter, parentVMParameter, entityFactoryParameter, propertyEditVMParameter));

		}

		protected CreateDynamicContentItemViewModel(
			DynamicContentItem item,
			CreateDynamicContentItemViewModel _parentViewModel,
			IDynamicContentEntityFactory entityFactory,
			IViewModelsFactory<IPropertyEditViewModel> propertyEditVmFactory)
		{
			_parentVM = _parentViewModel;

			InnerItem = item;
			InnerItem.PropertyChanged += InnerItem_PropertyChanged;
			_propertyEditVmFactory = propertyEditVmFactory;


			if (this is IDynamicContentItemPropertiesStepViewModel)
			{
				_entityFactory = entityFactory;
				PropertyValueEditCommand = new DelegateCommand<DynamicContentItemProperty>(RaisePropertyValueEditInteractionRequest, x => x != null);
				PropertyValueDeleteCommand = new DelegateCommand<DynamicContentItemProperty>(RaisePropertyValueDeleteInteractionRequest, x => x != null);
				CommonConfirmRequest = new InteractionRequest<Confirmation>();
			}
		}

		public DelegateCommand<DynamicContentItemProperty> PropertyValueEditCommand
		{
			get;
			private set;
		}

		public DelegateCommand<DynamicContentItemProperty> PropertyValueDeleteCommand
		{
			get;
			private set;
		}

		public DynamicContentItem InnerItem
		{
			get;
			private set;
		}

		public bool IsWizardMode
		{
			get { return true; }
		}

		#region IWizardStep Members

		private bool _isValid;
		public override bool IsValid
		{
			get { return _isValid; }
		}

		public override bool IsLast
		{
			get
			{
				return this is IDynamicContentItemPropertiesStepViewModel;
			}
		}

		public override string Comment
		{
			get
			{
				return string.Empty;
			}
		}

		public override string Description
		{
			get
			{
				var result = string.Empty;
				if (this is IDynamicContentItemOverviewStepViewModel)
				{
					result = string.Format("Enter Dynamic Content Item details".Localize());
				}
				else if (this is IDynamicContentItemPropertiesStepViewModel)
					result = "Enter properties values".Localize();

				return result;
			}
		}

		#endregion

		#region WizardViewModelBase overrides
		protected override void OnIsValidChanged()
		{
			InnerItem.Validate(false);

			_isValid = InnerItem.Errors.Count == 0;

			base.OnIsValidChanged();
		}
		#endregion

		#region private members

		void InnerItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (InnerItem.PropertyValues != null)
				InnerItem.PropertyValues.Clear();
			if (InnerItem.ContentTypeId != DynamicContentType.Undefined.ToString())
			{
				var l =
			localModel.PropertySets.GetPropertySetByItemType(
				(DynamicContentType)
					Enum.Parse(typeof(DynamicContentType), InnerItem.ContentTypeId));
				InnerItem.PropertyValues.Add(l);
			}

			OnIsValidChanged();
		}

		public void RaiseCanExecuteChanged()
		{

		}

		private void RaisePropertyValueDeleteInteractionRequest(DynamicContentItemProperty originalItem)
		{
			if (!string.IsNullOrEmpty(originalItem.LongTextValue))
			{
				CommonConfirmRequest.Raise(
					new ConditionalConfirmation()
					{
						Content = string.Format("Are you sure you want to clear value for property '{0}'?".Localize(), originalItem.Name),
						Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
					},
					(x) =>
					{
						if (x.Confirmed)
						{
							switch ((coreModel.PropertyValueType)originalItem.ValueType)
							{
								case coreModel.PropertyValueType.ShortString:
									InnerItem.PropertyValues.First(y => y.Name == originalItem.Name).ShortTextValue = string.Empty;
									break;
								case coreModel.PropertyValueType.Category:
								case coreModel.PropertyValueType.Image:
								case coreModel.PropertyValueType.LongString:
									InnerItem.PropertyValues.First(y => y.Name == originalItem.Name).LongTextValue = string.Empty;
									break;
								case coreModel.PropertyValueType.Decimal:
									InnerItem.PropertyValues.First(y => y.Name == originalItem.Name).DecimalValue = 0;
									break;
								case coreModel.PropertyValueType.Integer:
									InnerItem.PropertyValues.First(y => y.Name == originalItem.Name).IntegerValue = 0;
									break;
							}

							OnPropertyChanged("InnerItem");
						}
					});
			}
		}

		private void RaisePropertyValueEditInteractionRequest(DynamicContentItemProperty originalItem)
		{
			var item = originalItem.DeepClone(_entityFactory as IKnownSerializationTypes);
			var itemVM = _propertyEditVmFactory.GetViewModelInstance(
				new KeyValuePair<string, object>("item", item)
				);
			var confirmation = new ConditionalConfirmation { Title = "Enter property value".Localize(), Content = itemVM };

			CommonConfirmRequest.Raise(confirmation, (x) =>
			{
				if (x.Confirmed)
				{
					switch ((coreModel.PropertyValueType)item.ValueType)
					{
						case coreModel.PropertyValueType.ShortString:
							InnerItem.PropertyValues.First(y => y.Name == item.Name).ShortTextValue = item.ShortTextValue;
							break;
						case coreModel.PropertyValueType.Category:
						case coreModel.PropertyValueType.Image:
						case coreModel.PropertyValueType.LongString:
							InnerItem.PropertyValues.First(y => y.Name == item.Name).LongTextValue = item.LongTextValue;
							break;
						case coreModel.PropertyValueType.Decimal:
							InnerItem.PropertyValues.First(y => y.Name == item.Name).DecimalValue = item.DecimalValue;
							break;
						case coreModel.PropertyValueType.Integer:
							InnerItem.PropertyValues.First(y => y.Name == item.Name).IntegerValue = item.IntegerValue;
							break;
						case coreModel.PropertyValueType.Boolean:
							InnerItem.PropertyValues.First(y => y.Name == item.Name).BooleanValue = item.BooleanValue;
							break;
					}

					OnPropertyChanged("InnerItem");
				}
			});
		}

		#endregion
	}

	public class DynamicContentItemOverviewStepViewModel : CreateDynamicContentItemViewModel, IDynamicContentItemOverviewStepViewModel
	{
		public DynamicContentItemOverviewStepViewModel(DynamicContentItem item, CreateDynamicContentItemViewModel parentVM, IDynamicContentEntityFactory entityFactory)
			: base(item, parentVM, entityFactory, null)
		{

		}
	}

	public class DynamicContentItemPropertiesStepViewModel : CreateDynamicContentItemViewModel, IDynamicContentItemPropertiesStepViewModel
	{
		public DynamicContentItemPropertiesStepViewModel(
			DynamicContentItem item,
			CreateDynamicContentItemViewModel parentVM,
			IDynamicContentEntityFactory entityFactory,
			IViewModelsFactory<IPropertyEditViewModel> propertyEditVmFactory)
			: base(item, parentVM, entityFactory, propertyEditVmFactory)
		{

		}
	}
}
