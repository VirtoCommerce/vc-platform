using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Importing.Model
{
	public class TaxCategoryImporter : EntityImporterBase
	{
		private ICatalogRepository _repository;

		public TaxCategoryImporter()
		{
			Name = ImportEntityType.TaxCategory.ToString();
			InitializeSystemProperties();
		}

		private void InitializeSystemProperties()
		{
			var action = new ImportProperty { Name = "Action", DisplayName = "Action", IsRequiredProperty = true, IsEntityProperty = false, EntityImporterId = Name, DefaultValue = "Insert", IsEnumValuesProperty = true, EnumValues = { "Insert", "Insert & Update", "Update", "Delete" } };
			var taxCategoryId = new ImportProperty { Name = "TaxCategoryId", DisplayName = "Tax Category Id", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name };
			var categoryName = new ImportProperty { Name = "Name", DisplayName = "Name", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			
			AddSystemProperties(
				action, taxCategoryId, categoryName);
		}

		public override string Import(string catalogId, string propertySetId, ImportItem[] systemValues, ImportItem[] customValues, IRepository repository)
		{
			var _error = string.Empty;
			_repository = (ICatalogRepository)repository;

			var action = GetAction(systemValues.First(x => x.Name == "Action").Value);

			switch (action)
			{
				case ImportAction.Insert:
					var addItem = InitializeItem(null, systemValues);
					_repository.Add(addItem);
					
					break;
				case ImportAction.InsertAndReplace:
					var itemR = systemValues.FirstOrDefault(y => y.Name == "TaxCategoryId");
					if (itemR != null)
					{
						var originalItem = _repository.TaxCategories.Where(x => x.TaxCategoryId == itemR.Value).SingleOrDefault();
						if (originalItem != null)
							_repository.Remove(originalItem);
					}

					var replaceItem = InitializeItem(null, systemValues);
					_repository.Add(replaceItem);
					break;
				case ImportAction.Update:
					var itemU = systemValues.FirstOrDefault(y => y.Name == "TaxCategoryId");
					if (itemU != null)
					{
						var origItem = _repository.TaxCategories.Where(x => x.TaxCategoryId == itemU.Value).SingleOrDefault();
						if (origItem != null)
						{
							InitializeItem(origItem, systemValues);
							_repository.Update(origItem);
						}
					}
					break;
				case ImportAction.Delete:
					var itemD = systemValues.FirstOrDefault(y => y.Name == "TaxCategoryId");
					if (itemD != null)
					{
						var deleteItem = _repository.TaxCategories.Where(x => x.TaxCategoryId == itemD.Value).SingleOrDefault();
						if (deleteItem != null)
							_repository.Remove(deleteItem);
					}
					break;
			}
			return _error;
		}

		private TaxCategory InitializeItem(TaxCategory item, IEnumerable<ImportItem> systemValues)
		{
			if (item == null)
				item = new CatalogEntityFactory().CreateEntity<TaxCategory>();
			var itemProperties = item.GetType().GetProperties();
			if (string.IsNullOrEmpty(systemValues.First(x => x.Name == "TaxCategoryId").Value))
				systemValues.First(x => x.Name == "TaxCategoryId").Value = item.TaxCategoryId;
			systemValues.ToList().ForEach(x => SetPropertyValue(item, itemProperties.FirstOrDefault(y => y.Name == x.Name), x.Value));

			return item;
		}
	}
}
