using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Model.Taxes;
using VirtoCommerce.Foundation.Orders.Repositories;

namespace VirtoCommerce.Foundation.Importing.Model
{
	public class TaxValueImporter : EntityImporterBase
	{
		private ITaxRepository _repository;

		public TaxValueImporter()
		{
			Name = ImportEntityType.TaxValue.ToString();
			InitializeSystemProperties();
		}

		private void InitializeSystemProperties()
		{
			var action = new ImportProperty { Name = "Action", DisplayName = "Action", IsRequiredProperty = true, IsEntityProperty = false, EntityImporterId = Name, DefaultValue = "Insert", IsEnumValuesProperty = true, EnumValues = { "Insert", "Insert & Update", "Update", "Delete" } };
			var taxId = new ImportProperty { Name = "TaxId", DisplayName = "Tax Id", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name };
			var taxName = new ImportProperty { Name = "TaxName", DisplayName = "Tax Name", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			var taxType = new ImportProperty { Name = "TaxType", DisplayName = "Tax type", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			var taxSortOrder = new ImportProperty { Name = "SortOrder", DisplayName = "Sort order", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name };
			var taxJurisdictionGroupId = new ImportProperty { Name = "JurisdictionGroupId", DisplayName = "Jurisdiction group", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			var taxPercentage = new ImportProperty { Name = "Percentage", DisplayName = "Percentage", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			var taxTaxCategory = new ImportProperty { Name = "TaxCategory", DisplayName = "Category", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			var taxAffectiveDate = new ImportProperty { Name = "AffectiveDate", DisplayName = "Affective date", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name };

			AddSystemProperties(
				action, taxId, taxName, taxType,
				taxSortOrder, taxJurisdictionGroupId, taxPercentage,
				taxTaxCategory, taxAffectiveDate);
		}

		public override string Import(string catalogId, string propertySetId, ImportItem[] systemValues, ImportItem[] customValues, IRepository repository)
		{
			var _error = string.Empty;
			_repository = (ITaxRepository)repository;

			var action = GetAction(systemValues.First(x => x.Name == "Action").Value);

			switch (action)
			{
				case ImportAction.Insert:
					var itemI = systemValues.FirstOrDefault(y => y.Name == "TaxId");
					if (itemI != null)
					{
						var originalItem = _repository.Taxes.Where(x => x.TaxId == itemI.Value);
						if (originalItem.Count() == 0)
						{
							var name = systemValues.FirstOrDefault(y => y.Name == "TaxName");
							var type = systemValues.FirstOrDefault(y => y.Name == "TaxType");
							var addItem = new Tax { TaxId = itemI.Value, Name = name.Value, TaxType = Int32.Parse(type.Value) };
							_repository.Add(addItem);
							_repository.UnitOfWork.Commit();
						}
					}
					
					var tax = _repository.Taxes.Where(x => x.TaxId == itemI.Value);
					if (tax.Count() > 0)
					{
						var t = tax.First();
						var value = InitializeItem(null, systemValues);
						t.TaxValues.Add(value);
					}
					break;
				case ImportAction.InsertAndReplace:
					var itemR = systemValues.FirstOrDefault(y => y.Name == "TaxId");
					if (itemR != null)
					{
						var originalItem = _repository.Taxes.Where(x => x.TaxId == itemR.Value).SingleOrDefault();
						if (originalItem != null)
							_repository.Remove(originalItem);
					}

					var replaceItem = InitializeItem(null, systemValues);
					_repository.Add(replaceItem);
					break;
				case ImportAction.Update:
					//var itemU = systemValues.FirstOrDefault(y => y.Name == "PriceId");
					//if (itemU != null)
					//{
					//	var origItem = _repository.Taxes.Where(x => x.TaxId == itemU.Value).SingleOrDefault();
					//	if (origItem != null && origItem is TaxValue)
					//	{
					//		InitializeItem((TaxValue)origItem, systemValues);
					//		_repository.Update(origItem);
					//	}
					//}
					break;
				case ImportAction.Delete:
					var itemD = systemValues.FirstOrDefault(y => y.Name == "TaxId");
					if (itemD != null)
					{
						var deleteItem = _repository.Taxes.Where(x => x.TaxId == itemD.Value).SingleOrDefault();
						if (deleteItem != null)
							_repository.Remove(deleteItem);
					}
					break;
			}
			return _error;
		}

		private static TaxValue InitializeItem(TaxValue item, IEnumerable<ImportItem> systemValues)
		{
			if (item == null)
				item = new TaxValue();
			var itemProperties = item.GetType().GetProperties();
			systemValues.ToList().ForEach(x => SetPropertyValue(item, itemProperties.FirstOrDefault(y => y.Name == x.Name), x.Value));

			return item;
		}
	}
}
