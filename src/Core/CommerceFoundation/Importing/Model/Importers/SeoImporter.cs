using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Importing.Model
{
	public class SeoImporter : EntityImporterBase
	{
		private IAppConfigRepository _repository;

		public SeoImporter()
		{
			Name = ImportEntityType.Seo.ToString();
			InitializeSystemProperties();
		}

		private void InitializeSystemProperties()
		{
			var action = new ImportProperty { Name = "Action", DisplayName = "Action", IsRequiredProperty = true, IsEntityProperty = false, EntityImporterId = Name, DefaultValue = "Insert", IsEnumValuesProperty = true, EnumValues = { "Insert", "Insert & Update", "Update", "Delete" } };
			var type = new ImportProperty { Name = "KeywordType", DisplayName = "Type", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name, DefaultValue = SeoUrlKeywordTypes.Item.ToString(), IsEnumValuesProperty = true, EnumValues = new System.Collections.ObjectModel.ObservableCollection<string> (Enum.GetNames(typeof(SeoUrlKeywordTypes))) };
			var keyword = new ImportProperty { Name = "Keyword", DisplayName = "URL Keyword", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			var keywordValue = new ImportProperty { Name = "KeywordValue", DisplayName = "Target item", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			var locale = new ImportProperty { Name = "Language", DisplayName = "Locale", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			var metaDescription = new ImportProperty { Name = "MetaDescription", DisplayName = "Meta description", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name };
			var pageTitle = new ImportProperty { Name = "Title", DisplayName = "Page title", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name };
			var imageAltDescription = new ImportProperty { Name = "ImageAltDescription", DisplayName = "Image Alternative Description", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name };
			
			AddSystemProperties(
				action, type, locale, keywordValue, keyword, pageTitle, metaDescription, imageAltDescription);
		}

		public override string Import(string catalogId, string propertySetId, ImportItem[] systemValues, ImportItem[] customValues, IRepository repository)
		{
			var _error = string.Empty;
			_repository = (IAppConfigRepository)repository;

			var action = GetAction(systemValues.First(x => x.Name == "Action").Value);
			var type = systemValues.First(y => y.Name == "KeywordType");
			type.Value = ((int)Enum.Parse(typeof(SeoUrlKeywordTypes), type.Value)).ToString(CultureInfo.InvariantCulture);

			switch (action)
			{
				case ImportAction.Insert:
					
					var addItem = InitializeItem(null, systemValues);
					_repository.Add(addItem);
					break;
				case ImportAction.InsertAndReplace:
					var itemR = systemValues.FirstOrDefault(y => y.Name == "Language");
					var itemRKey = systemValues.FirstOrDefault(y => y.Name == "KeywordValue");
					var itemRType = Int32.Parse(systemValues.First(y => y.Name == "KeywordType").Value);
					if (itemR != null && itemRKey != null)
					{
						var originalItem = _repository.SeoUrlKeywords.Where(x => x.Language == itemR.Value && x.KeywordValue == itemRKey.Value && x.KeywordType == itemRType).SingleOrDefault();
						if (originalItem != null)
							_repository.Remove(originalItem);
					}

					var replaceItem = InitializeItem(null, systemValues);
					_repository.Add(replaceItem);
					break;
				case ImportAction.Update:
					var itemU = systemValues.FirstOrDefault(y => y.Name == "Language");
					var itemUKey = systemValues.FirstOrDefault(y => y.Name == "KeywordValue");
					var itemUType = Int32.Parse(systemValues.First(y => y.Name == "KeywordType").Value);
					if (itemU != null)
					{
						var origItem = _repository.SeoUrlKeywords.Where(x => x.Language == itemU.Value && x.KeywordValue.Equals(itemUKey.Value, StringComparison.Ordinal) && x.KeywordType == itemUType).SingleOrDefault();
						if (origItem != null)
						{
							InitializeItem(origItem, systemValues);							
							_repository.Update(origItem);
						}
					}
					break;
				case ImportAction.Delete:
					var itemD = systemValues.First(y => y.Name == "Language");
					var itemDKey = systemValues.First(y => y.Name == "KeywordValue");
					var itemDType = Int32.Parse(systemValues.First(y => y.Name == "KeywordType").Value);
					if (itemD != null)
					{
						var deleteItem = _repository.SeoUrlKeywords.Where(x => x.Language == itemD.Value && x.KeywordValue == itemDKey.Value && x.KeywordType == itemDType).SingleOrDefault();
						if (deleteItem != null)
							_repository.Remove(deleteItem);
					}
					break;
			}
			return _error;
		}

		private static SeoUrlKeyword InitializeItem(SeoUrlKeyword item, IEnumerable<ImportItem> systemValues)
		{
			if (item == null)
				item = new AppConfigEntityFactory().CreateEntity<SeoUrlKeyword>();
			var itemProperties = item.GetType().GetProperties();
			systemValues.ToList().ForEach(x => SetPropertyValue(item, itemProperties.FirstOrDefault(y => y.Name == x.Name), x.Value));
			item.IsActive = true;
			return item;
		}
	}
}
