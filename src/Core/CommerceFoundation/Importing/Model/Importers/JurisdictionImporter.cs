using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model.Jurisdiction;
using VirtoCommerce.Foundation.Orders.Repositories;

namespace VirtoCommerce.Foundation.Importing.Model
{
	public class JurisdictionImporter : EntityImporterBase
	{
		private IOrderRepository _repository;

		public JurisdictionImporter()
		{
			Name = ImportEntityType.Jurisdiction.ToString();
			InitializeSystemProperties();
		}

		private void InitializeSystemProperties()
		{
			var action = new ImportProperty { Name = "Action", DisplayName = "Action", IsRequiredProperty = true, IsEntityProperty = false, EntityImporterId = Name, DefaultValue = "Insert", IsEnumValuesProperty = true, EnumValues = { "Insert", "Insert & Update", "Update", "Delete" } };
			var jurisdictionId = new ImportProperty { Name = "JurisdictionId", DisplayName = "Jurisdiction Id", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name };
			var jurisdictionName = new ImportProperty { Name = "DisplayName", DisplayName = "Display Name", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name };
			var stateProvinceCode = new ImportProperty { Name = "StateProvinceCode", DisplayName = "State/province Code", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name };
			var countryCode = new ImportProperty { Name = "CountryCode", DisplayName = "Country", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			var jurisdictionType = new ImportProperty { Name = "JurisdictionType", DisplayName = "Jurisdiction Type", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name, DefaultValue = "All", IsEnumValuesProperty = true, EnumValues = { "All", "Shipping", "Taxes" } };
			var zipPostalCodeStart = new ImportProperty { Name = "ZipPostalCodeStart", DisplayName = "Zip/Postal Code Start", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name };
			var zipPostalCodeEnd = new ImportProperty { Name = "ZipPostalCodeEnd", DisplayName = "Zip/Postal Code End", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name };
			var city = new ImportProperty { Name = "City", DisplayName = "City", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name };
			var county = new ImportProperty { Name = "County", DisplayName = "County", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name };
			var geoCode = new ImportProperty { Name = "GeoCode", DisplayName = "Geographic Unique Code", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name };
			var code = new ImportProperty { Name = "Code", DisplayName = "Jurisdiction Code", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };

			AddSystemProperties(
				action, jurisdictionId, jurisdictionName, stateProvinceCode,
				countryCode, jurisdictionType, zipPostalCodeStart, zipPostalCodeEnd,
				city, county, geoCode, code);
		}

		public override string Import(string catalogId, string propertySetId, ImportItem[] systemValues, ImportItem[] customValues, IRepository repository)
		{
			var _error = string.Empty;
			_repository = (IOrderRepository)repository;

			var action = GetAction(systemValues.First(x => x.Name == "Action").Value);

			switch (action)
			{
				case ImportAction.Insert:
					var addItem = InitializeItem(null, systemValues);
					_repository.Add(addItem);
					
					break;
				case ImportAction.InsertAndReplace:
					var itemR = systemValues.FirstOrDefault(y => y.Name == "Code");
					if (itemR != null)
					{
						var originalItem = _repository.Jurisdictions.Where(x => x.Code == itemR.Value).SingleOrDefault();
						if (originalItem != null)
							_repository.Remove(originalItem);
					}

					var replaceItem = InitializeItem(null, systemValues);
					_repository.Add(replaceItem);
					break;
				case ImportAction.Update:
					var itemU = systemValues.FirstOrDefault(y => y.Name == "Code");
					if (itemU != null)
					{
						var origItem = _repository.Jurisdictions.Where(x => x.Code == itemU.Value).SingleOrDefault();
						if (origItem != null)
						{
							InitializeItem(origItem, systemValues);
							_repository.Update(origItem);
						}
					}
					break;
				case ImportAction.Delete:
					var itemD = systemValues.FirstOrDefault(y => y.Name == "Code");
					if (itemD != null)
					{
						var deleteItem = _repository.Jurisdictions.Where(x => x.Code == itemD.Value).SingleOrDefault();
						if (deleteItem != null)
							_repository.Remove(deleteItem);
					}
					break;
			}
			return _error;
		}

		private static Jurisdiction InitializeItem(Jurisdiction item, IEnumerable<ImportItem> systemValues)
		{
			if (item == null)
				item = new OrderEntityFactory().CreateEntity<Jurisdiction>();
			var itemProperties = item.GetType().GetProperties();
			systemValues.ToList().ForEach(x => SetPropertyValue(item, itemProperties.FirstOrDefault(y => y.Name == x.Name), x.Value));

			return item;
		}
	}
}
