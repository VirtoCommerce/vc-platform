using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Omu.ValueInjecter;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Catalogs;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Customers;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Web.Models;

namespace VirtoCommerce.Web.Virto.Helpers
{
    using VirtoCommerce.Foundation.Catalogs.Services;

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
			model.EditorialReviews = new List<EditorialReview>(item.EditorialReviews).ToArray();
			model.AssociationGroups = new List<AssociationGroup>(item.AssociationGroups).ToArray();

			if (propertySet != null && item.ItemPropertyValues != null)
			{
				var values = item.ItemPropertyValues;
				var properties = propertySet.PropertySetProperties.SelectMany(x => values.Where(v => v.Name == x.Property.Name
					&& !x.Property.PropertyAttributes.Any(pa=> pa.PropertyAttributeName.Equals("Hidden", StringComparison.OrdinalIgnoreCase))
					&& (!x.Property.IsLocaleDependant
					|| string.Equals(v.Locale, CultureInfo.CurrentUICulture.Name, StringComparison.InvariantCultureIgnoreCase))),
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
		/// Creates the property model.
		/// </summary>
		/// <param name="priority">The priority.</param>
		/// <param name="property">The property.</param>
		/// <param name="value">The value.</param>
		/// <param name="item">The item.</param>
		/// <returns>PropertyModel.</returns>
		/// <exception cref="System.ArgumentNullException">property</exception>
		public static PropertyModel CreatePropertyModel(int priority, Property property, ItemPropertyValue value, Item item)
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
		/// <returns>CatalogItemWithPriceModel.</returns>
		public static CatalogItemWithPriceModel CreateCatalogModel(string itemId, string parentItemId = null, string associationType = null, bool forcedActive = false, ItemResponseGroups responseGroups = ItemResponseGroups.ItemLarge, ItemDisplayOptions display = ItemDisplayOptions.ItemLarge)
		{
			//1. find item id by url
			//2. find item in search by id if catalog is virtual
			//3. retrieve item from master category by id and category name

			var item = CatalogClient.GetItem(itemId, responseGroups,
											  UserHelper.CustomerSession.CatalogId);

			if (item != null)
			{
				if (item.IsActive || forcedActive)
				{
					PriceModel priceModel = null;
					PropertySet propertySet = null;
					//ItemRelation[] variations = null;
					ItemAvailabilityModel itemAvaiability = null;


					if (display.HasFlag(ItemDisplayOptions.ItemAvailability))
					{
						var fulfillmentCenter = UserHelper.StoreClient.GetCurrentStore().FulfillmentCenterId;
						var availability = CatalogClient.GetItemAvailability(itemId, fulfillmentCenter);
						itemAvaiability = new ItemAvailabilityModel(availability);
					}

					if (display.HasFlag(ItemDisplayOptions.ItemPrice))
					{
                        var lowestPrice = PriceListClient.GetLowestPrice(itemId, itemAvaiability !=null ? itemAvaiability.MinQuantity : 1);
						var tags = new Hashtable
							{
								{
									"Outline",
                                    OutlineBuilder.BuildCategoryOutline(CatalogClient.CustomerSession.CatalogId, item).ToString()
                                }
							};
						priceModel = MarketingHelper.GetItemPriceModel(item, lowestPrice, tags);
					}


					if (display.HasFlag(ItemDisplayOptions.ItemPropertySets))
					{
						propertySet = CatalogClient.GetPropertySet(item.PropertySetId);
						//variations = CatalogClient.GetItemRelations(itemId);
					}

					var itemModel = CreateItemModel(item, propertySet);
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