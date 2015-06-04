using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Importing.Model
{
	public class AssociationImporter : EntityImporterBase
	{
		#region const
		private const string notUniqueTargetError = "Target item with code - {0} has matches in catalogs: {1}. Specify catalog property.";
		private const string notUniqueSourceError = "Source item with code - {0} has matches in catalogs: {1}. Specify catalog property.";
		#endregion

		private ICatalogRepository _repository;

		public AssociationImporter(ICatalogRepository catalogRepository)
		{
			_repository = catalogRepository;
			Name = ImportEntityType.Association.ToString();
			InitializeSystemProperties();
			_repository.Catalogs.ToList().ForEach(cat =>
				{ 
					SystemProperties.First(prop => prop.Name == "SourceCatalogId").EnumValues.Add(cat.Name);
					SystemProperties.First(prop => prop.Name == "TargetCatalogId").EnumValues.Add(cat.Name);
				});
			
		}

		private void InitializeSystemProperties()
		{
			var action = new ImportProperty { Name = "Action", DisplayName = "Action", IsRequiredProperty = true, IsEntityProperty = false, EntityImporterId = Name, DefaultValue = "Insert", IsEnumValuesProperty = true, EnumValues = { "Insert", "Insert & Update", "Update", "Delete" }, HasDefaultValue = true };
			var associationType = new ImportProperty { Name = "AssociationType", DisplayName = "Association Type", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name, DefaultValue = AssociationTypes.optional.ToString(), IsEnumValuesProperty = true, HasDefaultValue = true };
			var associationPriority = new ImportProperty { Name = "Priority", DisplayName = "Priority", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name, DefaultValue = "1" };
			var associationGroupName = new ImportProperty { Name = "GroupName", DisplayName = "Association group name", IsRequiredProperty = false, IsEntityProperty = false, EntityImporterId = Name };
			var associationSource = new ImportProperty { Name = "ItemId", DisplayName = "Source item", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			var associationTarget = new ImportProperty { Name = "TargetId", DisplayName = "Target item", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			var associationSourceCatalogId = new ImportProperty { Name = "SourceCatalogId", DisplayName = "Source item catalog", IsRequiredProperty = false, IsEntityProperty = false, EntityImporterId = Name, IsEnumValuesProperty = true};
			var associationTargetCatalogId = new ImportProperty { Name = "TargetCatalogId", DisplayName = "Target item catalog", IsRequiredProperty = false, IsEntityProperty = false, EntityImporterId = Name, IsEnumValuesProperty = true };

			foreach (var associatonType in Enum.GetValues(typeof(AssociationTypes)))
			{
				associationType.EnumValues.Add(associatonType.ToString());
			}

			AddSystemProperties(action, associationType,
				associationPriority, associationGroupName,
				associationSource, associationTarget, associationSourceCatalogId, associationTargetCatalogId);
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
					var target = systemValues.First(y => y.Name == "TargetId").Value;
					var source = systemValues.First(y => y.Name == "ItemId").Value;
					
					var sourceItems = _repository.Items.Where(x => x.ItemId == source || x.Code == source).ToList();
					
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
							_error = string.Format(notUniqueSourceError, source, catNames);
							return _error;
						}
					}
					//aa: if 1 item found set it to sourceItem and go further
					else
					{
						sourceItem = sourceItems.FirstOrDefault();
					}

					var targetItems = _repository.Items.Where(x => x.ItemId == target || x.Code == target).ToList();
					
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
							_error = string.Format(notUniqueTargetError, target, catNames);
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
						var associationGroup = _repository.Associations.Where(x => x.AssociationGroup.Name == group && x.ItemId == targetItem.ItemId).SingleOrDefault();
						string groupId;
						if (associationGroup == null)
						{
							var addGroup = new AssociationGroup() { ItemId = targetItem.ItemId, Name = group };
							_repository.Add(addGroup);
							groupId = addGroup.AssociationGroupId;
						}
						else
						{
							groupId = associationGroup.AssociationGroupId;
						}

						var addItem = InitializeItem(null, systemValues);
						((Association)addItem).AssociationGroupId = groupId;
						((Association)addItem).ItemId = sourceItem.ItemId;
						
						_repository.Add(addItem);
					}
					else
					{
						_error = "Not all required data provided";
					}
					break;
				case ImportAction.InsertAndReplace:
					var itemR = systemValues.FirstOrDefault(y => y.Name == "AssociationId");
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
					var itemU = systemValues.FirstOrDefault(y => y.Name == "AssociationId");
					if (itemU != null)
					{
						var origItem = _repository.Associations.Where(x => x.ItemId == itemU.Value).SingleOrDefault();
						if (origItem != null)
						{
							InitializeItem(origItem, systemValues);
							_repository.Update(origItem);
						}
					}
					break;
				case ImportAction.Delete:
					var itemD = systemValues.FirstOrDefault(y => y.Name == "AssociationId");
					if (itemD != null)
					{
						var deleteItem = _repository.Associations.Where(x => x.ItemId == itemD.Value).SingleOrDefault();
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
