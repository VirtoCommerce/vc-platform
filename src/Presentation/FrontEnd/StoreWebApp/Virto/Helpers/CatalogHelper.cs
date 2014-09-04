using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Omu.ValueInjecter;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Web.Client.Helpers;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Foundation.Catalogs.Services;

namespace VirtoCommerce.Web.Virto.Helpers
{

    /// <summary>
    /// Class CatalogHelper.
    /// </summary>
    public class CatalogHelper
    {
        /// <summary>
        /// Gets the catalog client.
        /// </summary>
        /// <value>The catalog client.</value>
        public static CatalogClient CatalogClient
        {
            get { return DependencyResolver.Current.GetService<CatalogClient>(); }
        }

        /// <summary>
        /// Gets the price list client.
        /// </summary>
        /// <value>The price list client.</value>
        public static PriceListClient PriceListClient
        {
            get { return DependencyResolver.Current.GetService<PriceListClient>(); }
        }

        /// <summary>
        /// Gets the marketing helper.
        /// </summary>
        /// <value>The marketing helper.</value>
        public static MarketingHelper MarketingHelper
        {
            get { return DependencyResolver.Current.GetService<MarketingHelper>(); }
        }

        public static ICatalogOutlineBuilder OutlineBuilder
        {
            get { return DependencyResolver.Current.GetService<ICatalogOutlineBuilder>(); }
        }

        /// <summary>
        /// Creates the item model.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="propertySet">The property set.</param>
        /// <returns>ItemModel.</returns>
        /// <exception cref="System.ArgumentNullException">item</exception>
        public static ItemModel CreateItemModel(Item item, PropertySet propertySet = null)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            var model = new ItemModel { Item = item };
            model.InjectFrom(item);

            model.ItemAssets = new List<ItemAsset>(item.ItemAssets).ToArray();
            var reviews = item.EditorialReviews;
            model.EditorialReviews = new List<EditorialReview>(reviews.Where(x => string.IsNullOrEmpty(x.Locale) 
                ||  string.Equals(x.Locale, CultureInfo.CurrentUICulture.Name, StringComparison.InvariantCultureIgnoreCase)
                || (string.Equals(x.Locale, StoreHelper.GetDefaultLanguageCode(), StringComparison.InvariantCultureIgnoreCase)
                    && !reviews.Any(val => val.Source == x.Source && string.Equals(val.Locale, CultureInfo.CurrentUICulture.Name, StringComparison.InvariantCultureIgnoreCase))))).ToArray();

            model.AssociationGroups = new List<AssociationGroup>(item.AssociationGroups).ToArray();

            if (propertySet != null && item.ItemPropertyValues != null)
            {
                var values = item.ItemPropertyValues;
                var properties = propertySet.PropertySetProperties.SelectMany(x => values.Where(v => v.Name == x.Property.Name
                    && !x.Property.PropertyAttributes.Any(pa => pa.PropertyAttributeName.Equals("Hidden", StringComparison.OrdinalIgnoreCase)) //skip hidden
                    && (!x.Property.IsLocaleDependant // if not localized ok
                    || string.Equals(v.Locale, CultureInfo.CurrentUICulture.Name, StringComparison.InvariantCultureIgnoreCase) //if current locale match value locale ok
                    || (string.Equals(v.Locale, StoreHelper.GetDefaultLanguageCode(), StringComparison.InvariantCultureIgnoreCase) //if default locale match value locale and values does not contain locale for current property ok
                    && !values.Any(val => val.Name == x.Property.Name && string.Equals(val.Locale, CultureInfo.CurrentUICulture.Name, StringComparison.InvariantCultureIgnoreCase))))),
                    (r, v) => CreatePropertyModel(r.Priority, r.Property, v, item)).ToArray();

                model.Properties = new PropertiesModel(properties);
            }

            //Find item category
            if (item.CategoryItemRelations != null)
            {
                string categoryId = null;
                foreach (var rel in item.CategoryItemRelations)
                {
                    if (rel.CatalogId == UserHelper.CustomerSession.CatalogId)
                    {
                        categoryId = rel.CategoryId;
                        break;
                    }

                    var category = CatalogClient.GetCategoryById(rel.CategoryId);

                    if (category == null)
                        continue;

                    var linkedCategory = category.LinkedCategories.FirstOrDefault(
                        link => link.CatalogId == UserHelper.CustomerSession.CatalogId);

                    if (linkedCategory == null)
                        continue;

                    categoryId = linkedCategory.CategoryId;
                    break;
                }

                if (!string.IsNullOrEmpty(categoryId))
                {
                    var category = CatalogClient.GetCategoryById(categoryId);
                    var cat = category as Category;
                    if (cat != null)
                    {
                        model.CategoryName = cat.Name.Localize();
                    }
                }
            }

            return model;
        }

        /// <summary>
        /// Creates the category model.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">category</exception>
        public static CategoryModel CreateCategoryModel(CategoryBase category)
        {
            if (category == null)
            {
                throw new ArgumentNullException("category");
            }

            var model = new CategoryModel { Category = category };
            model.InjectFrom(category);

            model.LinkedCategories = new List<LinkedCategory>(category.LinkedCategories).ToArray();
            model.CatalogOutline = CatalogClient.BuildCategoryOutline(UserHelper.CustomerSession.CatalogId, category);

            if (category is Category)
            {
                var realCat = category as Category;
                model.CategoryPropertyValues = new List<CategoryPropertyValue>(realCat.CategoryPropertyValues).ToArray();

                if (realCat.PropertySet != null && realCat.CategoryPropertyValues != null)
                {
                    var values = realCat.CategoryPropertyValues;
                    var properties = realCat.PropertySet.PropertySetProperties.SelectMany(x => values.Where(v => v.Name == x.Property.Name
                        && !x.Property.PropertyAttributes.Any(pa => pa.PropertyAttributeName.Equals("Hidden", StringComparison.OrdinalIgnoreCase))
                        && (!x.Property.IsLocaleDependant
                        || string.Equals(v.Locale, CultureInfo.CurrentUICulture.Name, StringComparison.InvariantCultureIgnoreCase))),
                        (r, v) => CreatePropertyModel(r.Priority, r.Property, v, category)).ToArray();

                    model.Properties = new PropertiesModel(properties);
                }
            }
            return model;
        }

        /// <summary>
        /// Creates the property model.
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        /// <param name="item">The item.</param>
        /// <returns>PropertyModel.</returns>
        /// <exception cref="System.ArgumentNullException">property</exception>
        public static PropertyModel CreatePropertyModel(int priority, Property property, PropertyValueBase value, StorageEntity item)
        {
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }

            var model = new PropertyModel { Priority = priority };
            model.InjectFrom<CloneInjection>(property);
            model.Values = new[] { CreatePropertyValueModel(value, property) };
            model.CatalogItem = item;
            return model;
        }

        /// <summary>
        /// Creates the property value model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="property">The property.</param>
        /// <returns>PropertyValueModel.</returns>
        public static PropertyValueModel CreatePropertyValueModel(PropertyValueBase value, Property property)
        {
            if (property.IsEnum && !string.IsNullOrEmpty(value.KeyValue))
            {
                return CreatePropertyValueModel(property.PropertyValues.First(p => p.PropertyValueId == value.KeyValue));
            }
            return CreatePropertyValueModel(value);
        }

        /// <summary>
        /// Creates the property value model.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>PropertyValueModel.</returns>
        /// <exception cref="System.ArgumentNullException">value</exception>
        public static PropertyValueModel CreatePropertyValueModel(PropertyValueBase value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            var model = new PropertyValueModel();
            model.InjectFrom<CloneInjection>(value);
            return model;
        }

        /// <summary>
        /// Creates the catalog model.
        /// </summary>
        /// <param name="itemId">The item identifier.</param>
        /// <param name="parentItemId">The parent item identifier.</param>
        /// <param name="associationType">Type of the association.</param>
        /// <param name="forcedActive">if set to <c>true</c> [forced active].</param>
        /// <param name="responseGroups">The response groups.</param>
        /// <param name="display">The display.</param>
        /// <param name="byItemCode">if set to <c>true</c> gets item by code.</param>
        /// <returns>
        /// CatalogItemWithPriceModel.
        /// </returns>
        public static CatalogItemWithPriceModel CreateCatalogModel(string itemId,
            string parentItemId = null,
            string associationType = null,
            bool forcedActive = false,
            ItemResponseGroups responseGroups = ItemResponseGroups.ItemLarge,
            ItemDisplayOptions display = ItemDisplayOptions.ItemLarge,
            bool byItemCode = false)
        {

            var dbItem = CatalogClient.GetItem(itemId, responseGroups,
                                              UserHelper.CustomerSession.CatalogId, bycode: byItemCode);
            if (dbItem != null)
            {

                if (dbItem.IsActive || forcedActive)
                {
                    PriceModel priceModel = null;
                    PropertySet propertySet = null;
                    //ItemRelation[] variations = null;
                    ItemAvailabilityModel itemAvaiability = null;

                    if (display.HasFlag(ItemDisplayOptions.ItemPropertySets))
                    {
                        propertySet = CatalogClient.GetPropertySet(dbItem.PropertySetId);
                        //variations = CatalogClient.GetItemRelations(itemId);
                    }

                    var itemModel = CreateItemModel(dbItem, propertySet);

                    if (display.HasFlag(ItemDisplayOptions.ItemAvailability))
                    {
                        var fulfillmentCenter = UserHelper.StoreClient.GetCurrentStore().FulfillmentCenterId;
                        var availability = CatalogClient.GetItemAvailability(dbItem.ItemId, fulfillmentCenter);
                        itemAvaiability = new ItemAvailabilityModel(availability);
                    }

                    if (display.HasFlag(ItemDisplayOptions.ItemPrice))
                    {
                        var lowestPrice = PriceListClient.GetLowestPrice(dbItem.ItemId, itemAvaiability != null ? itemAvaiability.MinQuantity : 1);
                        var outlines = OutlineBuilder.BuildCategoryOutline(CatalogClient.CustomerSession.CatalogId, dbItem.ItemId);
                        var tags = new Hashtable
							{
								{
									"Outline",
                                    outlines.ToString()
                                }
							};
                        priceModel = MarketingHelper.GetItemPriceModel(dbItem, lowestPrice, tags);
                        itemModel.CatalogOutlines = outlines;

                        // get the category name
                        if (outlines.Count > 0)
                        {
                            var outline = outlines[0];
                            if (outline.Categories.Count > 0)
                            {
                                var category = outline.Categories.OfType<Category>().Reverse().FirstOrDefault();
                                if (category != null)
                                {
                                    itemModel.CategoryName = category.Name;
                                }
                            }
                        }
                    }

                    itemModel.ParentItemId = parentItemId;

                    return string.IsNullOrEmpty(associationType)
                               ? new CatalogItemWithPriceModel(itemModel, priceModel, itemAvaiability)
                               : new AssociatedCatalogItemWithPriceModel(itemModel, priceModel, itemAvaiability, associationType);
                }
            }

            return null;
        }

        public static AssociationGroup Association(ItemModel item, string groupName)
        {
            return item.AssociationGroups.FirstOrDefault(ag => ag.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase));
        }


    }
}