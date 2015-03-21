using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.Foundation.Importing.Model
{
	public class CategoryImporter : EntityImporterBase
	{
		private ICatalogRepository _repository;

		public CategoryImporter()
		{
			Name = ImportEntityType.Category.ToString();
			InitializeSystemProperties();
		}

		private void InitializeSystemProperties()
		{
			var action = new ImportProperty { Name = "Action", DisplayName = "Action", IsRequiredProperty = true, IsEntityProperty = false, EntityImporterId = Name, DefaultValue = "Insert", IsEnumValuesProperty = true, EnumValues = { "Insert", "Insert & Update", "Update", "Delete" } };
			var categoryCode = new ImportProperty { Name = "Code", DisplayName = "Category Code", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			var categoryIsActive = new ImportProperty { Name = "IsActive", DisplayName = "Is Active", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name, DefaultValue = "True", IsEnumValuesProperty = true, EnumValues = { "True", "False" } };
			var categoryPriority = new ImportProperty { Name = "Priority", DisplayName = "Priority", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name, DefaultValue = "1" };
            var categoryParentId = new ImportProperty { Name = "ParentCategoryId", DisplayName = "Parent category code", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name };
			var categoryName = new ImportProperty { Name = "Name", DisplayName = "Category name", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			var categoryStartDate = new ImportProperty { Name = "StartDate", DisplayName = "Start date", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name, DefaultValue = DateTime.Today.ToShortDateString() };
			var categoryEndDate = new ImportProperty { Name = "EndDate", DisplayName = "End date", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name };

			AddSystemProperties(
				action, categoryCode,
				categoryIsActive, categoryPriority, categoryParentId,
				categoryName, categoryStartDate, categoryEndDate);
		}

		public override string Import(string catalogId, string propertySetId, ImportItem[] systemValues, ImportItem[] customValues, IRepository repository)
		{
			var _error = string.Empty;
			_repository = (ICatalogRepository)repository;
			var action = GetAction(systemValues.First(x => x.Name == "Action").Value);
			var itemCode = systemValues.SingleOrDefault(y => y.Name == "Code") != null ? systemValues.Single(y => y.Name == "Code").Value : null;
			var parentCategoryCode = systemValues.SingleOrDefault(val => val.Name == "ParentCategoryId") != null ? systemValues.Single(val => val.Name == "ParentCategoryId").Value : null;
			var parentCategory = _repository.Categories.Where(cat => cat.CatalogId == catalogId && cat.Code == parentCategoryCode).FirstOrDefault();

			//if action is not delete and parent category code provided is not null and there is no category with the code in repository return error
			if (action != ImportAction.Delete && !string.IsNullOrEmpty(parentCategoryCode) && parentCategory == null)
			{
				_error = string.Format("Parent category with the code {0} does not exist", parentCategoryCode);
			}
			else
			{
				switch (action)
				{
					case ImportAction.Insert:
						//if there is already category with the same code - return error
						if (_repository.Categories.Where(cat => cat.CatalogId == catalogId && cat.Code == itemCode).FirstOrDefault() != null)
						{
							_error = string.Format("Category with the code {0} already exist", itemCode);
						}
						else
						{
							var addItem = InitializeItem(null, systemValues);
							addItem.ParentCategoryId = !string.IsNullOrEmpty(parentCategoryCode) ? parentCategory.CategoryId : null;
							addItem.CatalogId = catalogId;
							addItem.PropertySetId = propertySetId;
							_repository.Add(addItem);
							var propSet =
								_repository.PropertySets.Expand("PropertySetProperties/Property/PropertyValues")
								           .Where(x => x.PropertySetId == propertySetId)
								           .SingleOrDefault();
							if (propSet != null)
							{
								var resProps = InitializeProperties(propSet.PropertySetProperties.ToArray(), customValues, addItem.CategoryId);
								resProps.ForEach(_repository.Add);
							}
						}
						break;
					case ImportAction.InsertAndReplace:
						if (itemCode != null)
						{
							var originalItem =
								_repository.Categories.Where(cat => cat.CatalogId == catalogId && cat.Code == itemCode).FirstOrDefault();
							if (originalItem != null)
							{
								InitializeItem((Category)originalItem, systemValues);
								originalItem.ParentCategoryId = !string.IsNullOrEmpty(parentCategoryCode) ? parentCategory.CategoryId : null;
								_repository.Update(originalItem);
							}
							else
							{
								var addItem = InitializeItem(null, systemValues);
								addItem.ParentCategoryId = !string.IsNullOrEmpty(parentCategoryCode) ? parentCategory.CategoryId : null;
								addItem.CatalogId = catalogId;
								addItem.PropertySetId = propertySetId;
								_repository.Add(addItem);
								var propSet =
									_repository.PropertySets.Expand("PropertySetProperties/Property/PropertyValues")
											   .Where(x => x.PropertySetId == propertySetId)
											   .SingleOrDefault();
								if (propSet != null)
								{
									var resProps = InitializeProperties(propSet.PropertySetProperties.ToArray(), customValues, addItem.CategoryId);
									resProps.ForEach(_repository.Add);
								}
							}
						}
						break;
					case ImportAction.Update:
						var itemU = systemValues.FirstOrDefault(y => y.Name == "Code");
						if (itemU != null)
						{
							var origItem = _repository.Categories.Where(x => x.CategoryId == itemU.Value).SingleOrDefault();
							if (origItem != null)
							{
								InitializeItem((Category) origItem, systemValues);
								_repository.Update(origItem);
							}
						}
						break;
					case ImportAction.Delete:
						var deleteItem = _repository.Categories.Where(cat => cat.CatalogId == catalogId && cat.Code == itemCode).SingleOrDefault();
						if (deleteItem != null)
							_repository.Remove(deleteItem);
						break;
				}
			}
			return _error;
		}

		private static Category InitializeItem(Category item, IEnumerable<ImportItem> systemValues)
		{
			if (item == null)
				item = new CatalogEntityFactory().CreateEntity<Category>();
			var itemProperties = item.GetType().GetProperties();
			systemValues.ToList().ForEach(x => SetPropertyValue(item, itemProperties.FirstOrDefault(y => y.Name == x.Name), x.Value));

			return item;
		}

		private static List<CategoryPropertyValue> InitializeProperties(PropertySetProperty[] props, IEnumerable<ImportItem> customValues, string categoryId)
		{
			var retVal = new List<CategoryPropertyValue>();
			foreach (var item in customValues)
			{
				var val = new CatalogEntityFactory().CreateEntity<CategoryPropertyValue>();
				var propSetProp = props.FirstOrDefault(x => x.Property.Name == item.Name);
				if (propSetProp != null)
				{
					val.CategoryId = categoryId;
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
								val.IntegerValue = Convert.ToInt32(item.Value);
								break;
							case PropertyValueType.Decimal:
								val.DecimalValue = Convert.ToDecimal(item.Value);
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
			return retVal;
		}

	}
}
