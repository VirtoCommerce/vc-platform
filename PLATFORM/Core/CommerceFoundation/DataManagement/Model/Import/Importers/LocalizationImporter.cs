using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Importing.Model
{
	public class LocalizationImporter : EntityImporterBase
	{
		private IAppConfigRepository _repository;

		public LocalizationImporter()
		{
			Name = ImportEntityType.Localization.ToString();
			InitializeSystemProperties();
		}

		private void InitializeSystemProperties()
		{
			var action = new ImportProperty { Name = "Action", DisplayName = "Action", IsRequiredProperty = true, IsEntityProperty = false, EntityImporterId = Name, DefaultValue = "Insert", IsEnumValuesProperty = true, EnumValues = { "Insert", "Insert & Update", "Update", "Delete" } };
			var name = new ImportProperty { Name = "Name", DisplayName = "Name", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			var locale = new ImportProperty { Name = "LanguageCode", DisplayName = "Locale", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			var category = new ImportProperty { Name = "Category", DisplayName = "Category", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name };
			var value = new ImportProperty { Name = "Value", DisplayName = "Value", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			
			AddSystemProperties(
				action, name, locale, category, value);
		}

		public override string Import(string catalogId, string propertySetId, ImportItem[] systemValues, ImportItem[] customValues, IRepository repository)
		{
			var _error = string.Empty;
			_repository = (IAppConfigRepository)repository;

			var action = GetAction(systemValues.First(x => x.Name == "Action").Value);

			switch (action)
			{
				case ImportAction.Insert:
					var addItem = InitializeItem(null, systemValues);
					_repository.Add(addItem);
					
					break;
				case ImportAction.InsertAndReplace:
					var itemR = systemValues.FirstOrDefault(y => y.Name == "LanguageCode");
					var itemRKey = systemValues.FirstOrDefault(y => y.Name == "Name");
					if (itemR != null)
					{
						var originalItem = _repository.Localizations.Where(x => x.LanguageCode == itemR.Value && x.Name == itemRKey.Value).SingleOrDefault();
						if (originalItem != null) 
							_repository.Remove(originalItem);
					}

					var replaceItem = InitializeItem(null, systemValues);
					_repository.Add(replaceItem);
					break;
				case ImportAction.Update:
					var itemU = systemValues.FirstOrDefault(y => y.Name == "LanguageCode");
					var itemUKey = systemValues.FirstOrDefault(y => y.Name == "Name");
					if (itemU != null)
					{
						var origItem = _repository.Localizations.Where(x => x.LanguageCode == itemU.Value && x.Name.Equals(itemUKey.Value, StringComparison.Ordinal)).SingleOrDefault();
						if (origItem != null)
						{
							InitializeItem(origItem, systemValues);							
							_repository.Update(origItem);
						}
					}
					break;
				case ImportAction.Delete:
					var itemD = systemValues.FirstOrDefault(y => y.Name == "LanguageCode");
					if (itemD != null)
					{
						var deleteItem = _repository.Localizations.Where(x => x.LanguageCode == itemD.Value).SingleOrDefault();
						if (deleteItem != null)
							_repository.Remove(deleteItem);
					}
					break;
			}
			return _error;
		}

		private static Localization InitializeItem(Localization item, IEnumerable<ImportItem> systemValues)
		{
			if (item == null)
				item = new AppConfigEntityFactory().CreateEntity<Localization>();
			var itemProperties = item.GetType().GetProperties();
			systemValues.ToList().ForEach(x => SetPropertyValue(item, itemProperties.FirstOrDefault(y => y.Name == x.Name), x.Value));

			return item;
		}
	}
}
