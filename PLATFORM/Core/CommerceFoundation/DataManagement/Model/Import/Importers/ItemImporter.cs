using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.Foundation.Importing.Model
{
    public class ItemImporter : EntityImporterBase
    {
        private const string noCategoryError = "Category with id/code {0} not found for item with code {1}";
        private ICatalogRepository _repository;

        public ItemImporter()
            : this(ImportEntityType.Product.ToString())
        {
        }

        public ItemImporter(string itemTypeName)
        {
            Name = itemTypeName;
            InitializeSystemProperties();
        }

        private void InitializeSystemProperties()
        {
            var action = new ImportProperty { Name = "Action", DisplayName = "Action", IsRequiredProperty = true, IsEntityProperty = false, EntityImporterId = Name, DefaultValue = "Insert", IsEnumValuesProperty = true, EnumValues = { "Insert", "Insert & Update", "Update", "Delete" } };
            var itemName = new ImportProperty { Name = "Name", DisplayName = "Name", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
            var itemStartDate = new ImportProperty { Name = "StartDate", DisplayName = "Start Date", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name, DefaultValue = DateTime.Today.ToShortDateString() };
            var itemEndDate = new ImportProperty { Name = "EndDate", DisplayName = "End Date", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name };
            var itemIsActive = new ImportProperty { Name = "IsActive", DisplayName = "Is Active", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name, IsEnumValuesProperty = true, EnumValues = { "True", "False" }, DefaultValue = "True" };
            var itemIsBuyable = new ImportProperty { Name = "IsBuyable", DisplayName = "Is Buyable", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name, IsEnumValuesProperty = true, EnumValues = { "True", "False" }, DefaultValue = "True" };
            var itemAvailabilityRule = new ImportProperty { Name = "AvailabilityRule", DisplayName = "Availability Rule", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name, IsEnumValuesProperty = true, DefaultValue = AvailabilityRule.Always.ToString() }; //Always available by default
            var itemMinQuantity = new ImportProperty { Name = "MinQuantity", DisplayName = "Minimum quantity", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name, DefaultValue = "1" };
            var itemMaxQuantity = new ImportProperty { Name = "MaxQuantity", DisplayName = "Maximum quantity", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name, DefaultValue = "1" };
            var itemTrackInventory = new ImportProperty { Name = "TrackInventory", DisplayName = "Track Inventory", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name, IsEnumValuesProperty = true, EnumValues = { "True", "False" }, DefaultValue = "False" };
            var itemWeight = new ImportProperty { Name = "Weight", DisplayName = "Weight", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name };
            var itemPackageType = new ImportProperty { Name = "PackageType", DisplayName = "Package Type", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name };
            var itemTaxCategory = new ImportProperty { Name = "TaxCategory", DisplayName = "Tax Category", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name };
            var itemCode = new ImportProperty { Name = "Code", DisplayName = "Code", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
            var itemCategory = new ImportProperty { Name = "CategoryId", DisplayName = "Category", IsRequiredProperty = false, IsEntityProperty = false, EntityImporterId = Name };
            var itemAsset = new ImportProperty { Name = "ItemAsset", DisplayName = "Asset", IsRequiredProperty = false, IsEntityProperty = false, EntityImporterId = Name };
            var itemEditorialReview = new ImportProperty { Name = "EditorialReview", DisplayName = "Editorial review", IsRequiredProperty = false, IsEntityProperty = false, EntityImporterId = Name };

            foreach (var availabilityRule in Enum.GetValues(typeof(AvailabilityRule)))
            {
                itemAvailabilityRule.EnumValues.Add(availabilityRule.ToString());
            }

            if (Name == ImportEntityType.Product.ToString() || Name == ImportEntityType.Bundle.ToString())
            {
                itemIsBuyable.DefaultValue = "False";
                AddSystemProperties(
                    action, itemName,
                    itemStartDate, itemEndDate, itemIsActive,
                    itemIsBuyable, itemAvailabilityRule, itemMinQuantity,
                    itemMaxQuantity, itemTrackInventory, itemWeight,
                    itemPackageType, itemCode, itemCategory, itemAsset, itemEditorialReview);
            }
            else
            {
                itemIsActive.DefaultValue = "False";
                AddSystemProperties(
                    action, itemName,
                    itemStartDate, itemEndDate, itemIsActive,
                    itemIsBuyable, itemAvailabilityRule, itemMinQuantity,
                    itemMaxQuantity, itemTrackInventory, itemWeight,
                    itemPackageType, itemTaxCategory, itemCode, itemCategory, itemAsset, itemEditorialReview);
            }
        }

        public override string Import(string containerId, string propertySetId, ImportItem[] systemValues, ImportItem[] customValues, IRepository repository)
        {
            var _error = string.Empty;
            _repository = (ICatalogRepository)repository;

            var action = GetAction(systemValues.First(x => x.Name == "Action").Value);

            var taxCategory = systemValues.SingleOrDefault(x => x.Name == "TaxCategory") != null ? systemValues.Single(x => x.Name == "TaxCategory").Value : null;
            var categoryId = systemValues.SingleOrDefault(x => x.Name == "CategoryId") != null ? systemValues.Single(x => x.Name == "CategoryId").Value : null;
            var itemCode = systemValues.SingleOrDefault(x => x.Name == "Code") != null ? systemValues.Single(x => x.Name == "Code").Value : null;
            var availability = systemValues.SingleOrDefault(x => x.Name == "AvailabilityRule") != null ? systemValues.Single(x => x.Name == "AvailabilityRule").Value : null;
            if (availability != null)
            {
                var number = (int)((AvailabilityRule)Enum.Parse(typeof(AvailabilityRule), availability));
                systemValues.SingleOrDefault(x => x.Name == "AvailabilityRule").Value = number.ToString();
            }


            switch (action)
            {
                case ImportAction.Insert:
                    if (_repository.Items.Where(item => item.CatalogId == containerId && item.Code == itemCode).FirstOrDefault() != null)
                    {
                        _error = string.Format("Item with the code {0} already exist", itemCode);
                    }
                    else
                    {
                        var addItem = SetupItem(null, containerId, propertySetId, systemValues, customValues, _repository, taxCategory);
                        _repository.Add(addItem);

                        _error = SetupCategoryRelation(categoryId, containerId, _repository, addItem);
                    }
                    break;
                case ImportAction.InsertAndReplace:
                    if (itemCode != null)
                    {
                        var originalItem = _repository.Items.Where(i => i.CatalogId == containerId && i.Code == itemCode).Expand(x => x.CategoryItemRelations).FirstOrDefault();
                        if (originalItem != null)
                        {
                            originalItem = SetupItem(originalItem, containerId, propertySetId, systemValues, customValues, _repository, taxCategory);
                            _repository.Update(originalItem);
                            if (originalItem.CategoryItemRelations.All(rel => rel.CategoryId != categoryId))
                                _error = SetupCategoryRelation(categoryId, containerId, _repository, originalItem);
                        }
                        else
                        {
                            var newItem = SetupItem(null, containerId, propertySetId, systemValues, customValues, _repository, taxCategory);
                            _repository.Add(newItem);
                            _error = SetupCategoryRelation(categoryId, containerId, _repository, newItem);
                        }

                    }
                    break;
                case ImportAction.Update:
                    if (itemCode != null)
                    {
                        var origItem = _repository.Items.FirstOrDefault(i => i.CatalogId == containerId && i.Code == itemCode);
                        if (origItem != null)
                        {
                            SetupItem(origItem, containerId, propertySetId, systemValues, customValues, _repository, taxCategory);
                            _repository.Update(origItem);
                        }
                    }
                    break;
                case ImportAction.Delete:
                    if (itemCode != null)
                    {
                        var deleteItem = _repository.Items.Where(i => i.CatalogId == containerId && i.Code == itemCode).SingleOrDefault();
                        if (deleteItem != null)
                            _repository.Remove(deleteItem);
                    }
                    break;
            }
            return _error;
        }

        private static string SetupCategoryRelation(string categoryId, string catalogId, ICatalogRepository repository, Item item)
        {
            var retVal = string.Empty;

            if (categoryId != null)
            {
                var category =
                    repository.Categories.Where(
                        cat => cat.CatalogId == catalogId && (cat.CategoryId == categoryId || cat.Code == categoryId))
                               .FirstOrDefault();
                if (category != null)
                {
                    var relation = new CatalogEntityFactory().CreateEntity<CategoryItemRelation>();
                    relation.CatalogId = catalogId;
                    relation.ItemId = item.ItemId;
                    relation.CategoryId = category.CategoryId;
                    repository.Add(relation);
                }
                else
                {
                    retVal = string.Format(noCategoryError, categoryId, item.Code);
                }
            }

            return retVal;
        }

        private Item SetupItem(Item item, string containerId, string propertySetId, ImportItem[] systemValues, IEnumerable<ImportItem> customValues, ICatalogRepository repository, string taxCategory)
        {
            var retVal = (Item)InitializeItem(item, systemValues);
            retVal.CatalogId = containerId;
            if (!string.IsNullOrEmpty(propertySetId))
            {
                var propSet = repository.PropertySets.Expand("PropertySetProperties/Property/PropertyValues").Where(x => x.PropertySetId == propertySetId).FirstOrDefault();
                var resProps = InitializeProperties(propSet.PropertySetProperties.ToArray(), customValues, retVal.ItemId);
                retVal.ItemPropertyValues.Add(resProps);
                retVal.PropertySetId = propSet.PropertySetId;
            }

            if (!string.IsNullOrEmpty(taxCategory))
            {
                var cat =
                    repository.TaxCategories.Where(x => x.TaxCategoryId == taxCategory || x.Name == taxCategory).FirstOrDefault();
                retVal.TaxCategory = cat != null ? cat.TaxCategoryId : null;
            }

            if (!string.IsNullOrEmpty(systemValues.First(x => x.Name == "ItemAsset").Value))
            {
                var itemAsset = new ItemAsset
                {
                    AssetType = "image",
                    ItemId = retVal.ItemId,
                    AssetId = systemValues.First(x => x.Name == "ItemAsset").Value,
                    GroupName = "primaryimage"
                };
                retVal.ItemAssets.Add(itemAsset);
            }

            if (!string.IsNullOrEmpty(systemValues.First(x => x.Name == "EditorialReview").Value))
            {
                var editorial = new CatalogEntityFactory().CreateEntity<EditorialReview>();
                editorial.ReviewState = (int)ReviewState.Active;
                editorial.Content = systemValues.First(x => x.Name == "EditorialReview").Value;
                editorial.ItemId = retVal.ItemId;
                retVal.EditorialReviews.Add(editorial);
            }

            return retVal;
        }

        private object InitializeItem(object item, IEnumerable<ImportItem> systemValues)
        {
            if (item == null)
                item = new CatalogEntityFactory().CreateEntityForType(Name);
            var itemProperties = item.GetType().GetProperties();
            systemValues.ToList().ForEach(x => SetPropertyValue(item, itemProperties.FirstOrDefault(y => y.Name == x.Name), x.Value));

            return item;
        }

        private static IEnumerable<ItemPropertyValue> InitializeProperties(PropertySetProperty[] props, IEnumerable<ImportItem> customValues, string itemId)
        {
            var retVal = new List<ItemPropertyValue>();
            foreach (var item in customValues)
            {
                if (item.Value != null)
                {
                    var val = new CatalogEntityFactory().CreateEntity<ItemPropertyValue>();
                    var propSetProp = props.FirstOrDefault(x => x.Property.Name == item.Name);
                    val.ItemId = itemId;
                    val.ValueType = propSetProp.Property.PropertyValueType;
                    val.Name = item.Name;
                    val.Locale = item.Locale;

                    if (propSetProp.Property.IsEnum)
                    {
                        var propVal = propSetProp.Property.PropertyValues.FirstOrDefault(value => value.ToString() == item.Value);
                        val.KeyValue = propVal != null ? propVal.PropertyValueId : null;
                    }
                    else
                    {
                        switch ((PropertyValueType)val.ValueType)
                        {
                            case PropertyValueType.DateTime:
                                val.DateTimeValue = DateTime.Parse(item.Value);
                                break;
                            case PropertyValueType.Integer:
                                val.IntegerValue = Convert.ToInt32(item.Value.Replace(',', '.'), CultureInfo.InvariantCulture.NumberFormat);
                                break;
                            case PropertyValueType.Decimal:
                                val.DecimalValue = Convert.ToDecimal(item.Value.Replace(',', '.'), CultureInfo.InvariantCulture.NumberFormat);
                                break;
                            case PropertyValueType.ShortString:
                                val.ShortTextValue = item.Value;
                                break;
                            case PropertyValueType.LongString:
                                val.LongTextValue = item.Value;
                                break;
                            case PropertyValueType.Boolean:
                                val.BooleanValue = Convert.ToBoolean(item.Value);
                                break;
                        }
                    }
                    retVal.Add(val);
                }
            }
            return retVal.ToArray();
        }
    }
}
