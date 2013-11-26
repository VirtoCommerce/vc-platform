using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Importing.Model
{
	public class RelationImporter : EntityImporterBase
	{
		#region const
		private const string notUniqueTargetError = "Parent item with code - {0} has matches in catalogs: {1}. Specify catalog property.";
		private const string notUniqueSourceError = "Child item with code - {0} has matches in catalogs: {1}. Specify catalog property.";
		#endregion

		private ICatalogRepository _repository;

		public RelationImporter(ICatalogRepository catalogRepository)
		{
			_repository = catalogRepository;
			Name = ImportEntityType.ItemRelation.ToString();
			InitializeSystemProperties();
			_repository.Catalogs.ToList().ForEach(cat =>
				{ 
					SystemProperties.First(prop => prop.Name == "SourceCatalogId").EnumValues.Add(cat.Name);
					SystemProperties.First(prop => prop.Name == "TargetCatalogId").EnumValues.Add(cat.Name);
				});

			SystemProperties.First(prop => prop.Name == "RelationTypeId").EnumValues.Add(ItemRelationType.Sku);
			SystemProperties.First(prop => prop.Name == "RelationTypeId").EnumValues.Add(ItemRelationType.Category);
			SystemProperties.First(prop => prop.Name == "RelationTypeId").EnumValues.Add(ItemRelationType.BundleItem);
			SystemProperties.First(prop => prop.Name == "RelationTypeId").EnumValues.Add(ItemRelationType.PackageItem);
		}

		private void InitializeSystemProperties()
		{
			var action = new ImportProperty { Name = "Action", DisplayName = "Action", IsRequiredProperty = true, IsEntityProperty = false, EntityImporterId = Name, DefaultValue = "Insert", IsEnumValuesProperty = true, EnumValues = { "Insert", "Insert & Update", "Update", "Delete" }, HasDefaultValue = true };
			var relationType = new ImportProperty { Name = "RelationTypeId", DisplayName = "Relation Type", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name, DefaultValue = ItemRelationType.Sku, IsEnumValuesProperty = true, HasDefaultValue = true };
			var relationGroupName = new ImportProperty { Name = "GroupName", DisplayName = "Relation group", IsRequiredProperty = false, IsEntityProperty = false, EntityImporterId = Name };
			var relationParent = new ImportProperty { Name = "ParentId", DisplayName = "Parent item", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			var relationChild = new ImportProperty { Name = "ChildId", DisplayName = "Child item", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			var relationSourceCatalogId = new ImportProperty { Name = "SourceCatalogId", DisplayName = "Source item catalog", IsRequiredProperty = false, IsEntityProperty = false, EntityImporterId = Name, IsEnumValuesProperty = true};
			var relationTargetCatalogId = new ImportProperty { Name = "TargetCatalogId", DisplayName = "Target item catalog", IsRequiredProperty = false, IsEntityProperty = false, EntityImporterId = Name, IsEnumValuesProperty = true };
			var relationQuantity = new ImportProperty { Name = "Quantity", DisplayName = "Quantity", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name };
			
			AddSystemProperties(action, relationType,
				relationGroupName, relationParent, relationChild, relationSourceCatalogId, relationTargetCatalogId, relationQuantity);
		}

		public override string Import(string catalogId, string propertySetId, ImportItem[] systemValues, ImportItem[] customValues, IRepository repository)
		{
			var _error = string.Empty;
			_repository = (ICatalogRepository)repository;

			var action = GetAction(systemValues.First(x => x.Name == "Action").Value);
			
			switch (action)
			{
				case ImportAction.Insert:
					var group = systemValues.First(y => y.Name == "GroupName").Value;
					var child = systemValues.First(y => y.Name == "ChildId").Value;
					var parent = systemValues.First(y => y.Name == "ParentId").Value;
					
					var sourceItems = _repository.Items.Where(x => x.ItemId == parent || x.Code == parent).ToList();
					
					Item sourceItem;

					//aa: below condition checks if more than 1 item found - catalogId should be provided and item of the catalog selected, otherwise return error
					if (sourceItems.Count() > 1)
					{
						var sourceCatName = systemValues.First(y => y.Name == "SourceCatalogId").Value;
						if (!string.IsNullOrEmpty(sourceCatName))
							catalogId = _repository.Catalogs.Where(cat => cat.Name == sourceCatName).First().CatalogId;

						if (!string.IsNullOrEmpty(catalogId))
							sourceItem = sourceItems.FirstOrDefault(x => x.CatalogId == catalogId);
						else
						{
							var catNames = string.Empty;
							sourceItems.ForEach(x => catNames = catNames + x.CatalogId + ", ");
							_error = string.Format(notUniqueSourceError, parent, catNames);
							return _error;
						}
					}
					//aa: if 1 item found set it to sourceItem and go further
					else
					{
						sourceItem = sourceItems.FirstOrDefault();
					}

					var targetItems = _repository.Items.Where(x => x.ItemId == child || x.Code == child).ToList();
					
					Item targetItem;

					//aa: below condition checks if more than 1 item found - catalogId should be provided and item of the catalog selected, otherwise return error
					if (targetItems.Count() > 1)
					{
						var targetCatName = systemValues.First(y => y.Name == "TargetCatalogId").Value;
						if (!string.IsNullOrEmpty(targetCatName))
							catalogId = _repository.Catalogs.Where(cat => cat.Name == targetCatName).First().CatalogId;

						if (!string.IsNullOrEmpty(catalogId))
							targetItem = targetItems.FirstOrDefault(x => x.CatalogId == catalogId);
						else
						{
							var catNames = string.Empty;
							targetItems.ForEach(x => catNames = catNames + x.CatalogId + ", ");
							_error = string.Format(notUniqueTargetError, child, catNames);
							return _error;
						}
					}
					//aa: if 1 item found set it to sourceItem and go further
					else
					{
						targetItem = targetItems.FirstOrDefault();
					}
					
					if (!string.IsNullOrEmpty(group) && targetItem != null && sourceItem != null)
					{
						var addItem = InitializeItem(null, systemValues);
						((ItemRelation)addItem).GroupName = group;
						((ItemRelation)addItem).ParentItemId = sourceItem.ItemId;
						((ItemRelation)addItem).ChildItemId = targetItem.ItemId;
						
						_repository.Add(addItem);
					}
					else
					{
						_error = "Not all required data provided";
					}
					break;
				case ImportAction.InsertAndReplace:
					var itemR = systemValues.FirstOrDefault(y => y.Name == "ItemRelationId");
					if (itemR != null)
					{
						var originalItem = _repository.Associations.Where(x => x.ItemId == itemR.Value).SingleOrDefault();
						if (originalItem != null)
							repository.Remove(originalItem);
					}
					var replaceItem = InitializeItem(null, systemValues);
					repository.Add(replaceItem);
					break;
				case ImportAction.Update:
					var itemU = systemValues.FirstOrDefault(y => y.Name == "ItemRelationId");
					if (itemU != null)
					{
						var origItem = _repository.ItemRelations.Where(x => x.ItemRelationId == itemU.Value).SingleOrDefault();
						if (origItem != null)
						{
							InitializeItem(origItem, systemValues);
							_repository.Update(origItem);
						}
					}
					break;
				case ImportAction.Delete:
					var itemD = systemValues.FirstOrDefault(y => y.Name == "ItemRelationId");
					if (itemD != null)
					{
						var deleteItem = _repository.ItemRelations.Where(x => x.ItemRelationId == itemD.Value).SingleOrDefault();
						if (deleteItem != null)
							_repository.Remove(deleteItem);
					}
					break;
			}
			return _error;
		}
		
		private object InitializeItem(object item, IEnumerable<ImportItem> systemValues)
		{
			if (item == null)
				item = new CatalogEntityFactory().CreateEntityForType(Name);
			var itemProperties = item.GetType().GetProperties();
			systemValues.ToList().ForEach(x => SetPropertyValue(item, itemProperties.FirstOrDefault(y => y.Name == x.Name), x.Value));

			return item;
		}
	}
}
