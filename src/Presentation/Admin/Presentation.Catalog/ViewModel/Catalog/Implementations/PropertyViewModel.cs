using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Localization;
using catalogModel = VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
    public class PropertyViewModel : ViewModelBase, IPropertyViewModel
    {
        #region Dependencies

        private readonly IViewModelsFactory<IPropertyValueViewModel> _propertyValueVmFactory;
        private readonly IViewModelsFactory<IPropertyAttributeViewModel> _attributeVmFactory;
        private readonly IRepositoryFactory<ICatalogRepository> _repositoryFactory;
        private readonly ICatalogEntityFactory _entityFactory;
        private readonly ObservableCollection<Property> _properties;
        #endregion

        public catalogModel.Catalog ParentCatalog { get; private set; }

        public PropertyViewModel(IViewModelsFactory<IPropertyValueViewModel> propertyValueVmFactory, IViewModelsFactory<IPropertyAttributeViewModel> attributeVmFactory, ICatalogEntityFactory entityFactory, Property item, catalogModel.Catalog parentCatalog, IRepositoryFactory<ICatalogRepository> repositoryFactory, ObservableCollection<Property> properties)
        {
            InnerItem = item;
            _propertyValueVmFactory = propertyValueVmFactory;
            _attributeVmFactory = attributeVmFactory;
            _entityFactory = entityFactory;
            _properties = properties;
            ParentCatalog = parentCatalog;
            // _repositoryFactory = repositoryFactory;

            ValueAddCommand = new DelegateCommand(RaiseValueAddInteractionRequest);
            ValueEditCommand = new DelegateCommand<PropertyValueBase>(RaiseValueEditInteractionRequest, x => x != null);
            ValueDeleteCommand = new DelegateCommand<PropertyValueBase>(RaiseValueDeleteInteractionRequest, x => x != null);

            AttributeAddCommand = new DelegateCommand(RaiseAttributeAddInteractionRequest);
            AttributeEditCommand = new DelegateCommand<PropertyAttribute>(RaiseAttributeEditInteractionRequest, x => x != null);
            AttributeDeleteCommand = new DelegateCommand<PropertyAttribute>(RaiseAttributeDeleteInteractionRequest, x => x != null);


            CommonConfirmRequest = new InteractionRequest<Confirmation>();

            var allValueTypes = (PropertyValueType[])Enum.GetValues(typeof(PropertyValueType));
            PropertyTypes = new List<PropertyValueType>(allValueTypes);
            PropertyTypes.Sort((x, y) => x.ToString().CompareTo(y.ToString()));
        }

        public List<PropertyValueType> PropertyTypes { get; private set; }

        private object _propertyValueType;
        public object PropertyType
        {
            set
            {
                if (value != null && _propertyValueType != value)
                {
                    _propertyValueType = value;
                    ValidatePropertyValueType();
                }
            }
        }

        public DelegateCommand ValueAddCommand { get; private set; }
        public DelegateCommand<PropertyValueBase> ValueEditCommand { get; private set; }
        public DelegateCommand<PropertyValueBase> ValueDeleteCommand { get; private set; }
        public DelegateCommand AttributeAddCommand { get; private set; }
        public DelegateCommand<PropertyAttribute> AttributeEditCommand { get; private set; }
        public DelegateCommand<PropertyAttribute> AttributeDeleteCommand { get; private set; }

        public InteractionRequest<Confirmation> CommonConfirmRequest { get; private set; }

        #region IPropertyViewModel

        public Property InnerItem { get; private set; }

        public bool Validate()
        {
            InnerItem.Validate();

            ValidatePropertyValueType();

            if (InnerItem.IsEnum && InnerItem.PropertyValues.Count == 0)
                InnerItem.SetError("Values", "Dictionary values must be defined".Localize(), true);
            else
                InnerItem.ClearError("Values");

            if (!InnerItem.Errors.Any())
            {
                var isUnique = !_properties.Any(x => x.PropertyId != InnerItem.PropertyId && x.Name == InnerItem.Name);
                // check only on client, not DB
                //if (isUnique)
                //{
                //    var repository = _repositoryFactory.GetRepositoryInstance();
                //    isUnique = repository.Properties.Where(x => x.PropertyId != InnerItem.PropertyId && x.Name == InnerItem.Name && x.CatalogId == ParentCatalog.CatalogId)
                //        .Count() == 0;
                //}

                if (isUnique)
                    InnerItem.ClearError("Name");
                else
                    InnerItem.SetError("Name", "Property with the same name already exists in this catalog.".Localize(), true);
            }

            return !InnerItem.Errors.Any();
        }

        #endregion

        #region private members
        private void ValidatePropertyValueType()
        {
            if (_propertyValueType != null && !(_propertyValueType is PropertyValueType))
                InnerItem.SetError("PropertyValueType", "Property Type must be selected".Localize(), true);
            else
                InnerItem.ClearError("PropertyValueType");
        }

        private void RaiseValueAddInteractionRequest()
        {
            var item = (PropertyValue)_entityFactory.CreateEntityForType("PropertyValue");
            // ValueType is inherited
            item.ValueType = InnerItem.PropertyValueType;
            if (RaisePropertyValueEditInteractionRequest(item, "Create property value".Localize()))
            {
                InnerItem.PropertyValues.Add(item);
            }
        }

        private void RaiseValueEditInteractionRequest(PropertyValueBase originalItem)
        {
            var item = originalItem.DeepClone(_entityFactory as CatalogEntityFactory);
            if (RaisePropertyValueEditInteractionRequest(item, "Edit property value".Localize()))
            {
                // copy all values to original:
                OnUIThread(() => originalItem.InjectFrom<CloneInjection>(item));
                // fake assign for UI triggers to display correct values.
                originalItem.ValueType = item.ValueType;
            }
        }

        private void RaiseValueDeleteInteractionRequest(PropertyValueBase item)
        {
            var confirmation = new ConditionalConfirmation
            {
                Content = string.Format("Are you sure you want to delete dictionary Property value '{0}'?".Localize(), item),
                Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
            };

            CommonConfirmRequest.Raise(confirmation, (x) =>
            {
                if (x.Confirmed)
                {
                    InnerItem.PropertyValues.Remove((PropertyValue)item);
                }
            });
        }

        private bool RaisePropertyValueEditInteractionRequest(StorageEntity item, string title)
        {
            bool result = false;
            var itemVM = _propertyValueVmFactory.GetViewModelInstance(
                new KeyValuePair<string, object>("item", item),
                new KeyValuePair<string, object>("parent", this)
                );
            var confirmation = new ConditionalConfirmation(itemVM.Validate);
            confirmation.Title = title;
            confirmation.Content = itemVM;

            CommonConfirmRequest.Raise(confirmation, (x) =>
            {
                result = x.Confirmed;
            });

            return result;
        }

        private void RaiseAttributeAddInteractionRequest()
        {
            var item = (PropertyAttribute)_entityFactory.CreateEntityForType("PropertyAttribute");
            item.PropertyId = InnerItem.PropertyId;
            if (RaisePropertyAttributeEditInteractionRequest(item, "Create property attribute".Localize()))
            {
                InnerItem.PropertyAttributes.Add(item);
            }
        }

        private void RaiseAttributeEditInteractionRequest(StorageEntity originalItem)
        {
            var item = originalItem.DeepClone(_entityFactory as CatalogEntityFactory);
            if (RaisePropertyAttributeEditInteractionRequest(item, "Edit property attribute".Localize()))
            {
                // copy all values to original:
                OnUIThread(() => originalItem.InjectFrom<CloneInjection>(item));
            }
        }

        private bool RaisePropertyAttributeEditInteractionRequest(StorageEntity item, string title)
        {
            var result = false;
            var itemVM = _attributeVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", item));
            var confirmation = new ConditionalConfirmation(itemVM.Validate) { Title = title, Content = itemVM };

            CommonConfirmRequest.Raise(confirmation, (x) =>
            {
                result = x.Confirmed;
            });

            return result;
        }

        private void RaiseAttributeDeleteInteractionRequest(PropertyAttribute item)
        {
            var confirmation = new ConditionalConfirmation
            {
                Content = string.Format("Are you sure you want to delete Property attribute '{0}({1})'?".Localize(), item.PropertyAttributeName, item.PropertyAttributeValue),
                Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
            };

            CommonConfirmRequest.Raise(confirmation, (x) =>
            {
                if (x.Confirmed)
                {
                    InnerItem.PropertyAttributes.Remove(item);
                }
            });
        }

        #endregion

    }
}
