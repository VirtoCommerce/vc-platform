using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Orders.Factories;
using VirtoCommerce.Foundation.Orders.Model.Jurisdiction;
using VirtoCommerce.Foundation.Orders.Repositories;

namespace VirtoCommerce.Foundation.Importing.Model
{
	public class JurisdictionGroupImporter : EntityImporterBase
	{
		private IOrderRepository _repository;

		public JurisdictionGroupImporter()
		{
			Name = ImportEntityType.JurisdictionGroup.ToString();
			InitializeSystemProperties();
		}

		private void InitializeSystemProperties()
		{
			var action = new ImportProperty { Name = "Action", DisplayName = "Action", IsRequiredProperty = true, IsEntityProperty = false, EntityImporterId = Name, DefaultValue = "Insert", IsEnumValuesProperty = true, EnumValues = { "Insert", "Insert & Update", "Update", "Delete" } };
			var jurisdictionGroupId = new ImportProperty { Name = "JurisdictionGroupId", DisplayName = "Jurisdiction group Id", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name };			
			var jurisdictionGroupName = new ImportProperty { Name = "DisplayName", DisplayName = "Display Name", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			var jurisdictionGroupType = new ImportProperty { Name = "JurisdictionType", DisplayName = "Jurisdiction group Type", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name, DefaultValue = "All", IsEnumValuesProperty = true, EnumValues = { "All", "Shipping", "Taxes" } };
			var code = new ImportProperty { Name = "Code", DisplayName = "Jurisdiction Group Code", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			var jurisdictionCode = new ImportProperty { Name = "JurisdictionCode", DisplayName = "Jurisdiction Code", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };

			AddSystemProperties(
				action, jurisdictionGroupId, jurisdictionGroupName, 
				jurisdictionGroupType, code, jurisdictionCode);
		}

		public override string Import(string catalogId, string propertySetId, ImportItem[] systemValues, ImportItem[] customValues, IRepository repository)
		{
			var _error = string.Empty;
			_repository = (IOrderRepository)repository;

			var action = GetAction(systemValues.First(x => x.Name == "Action").Value);

			switch (action)
			{
				case ImportAction.Insert:
					var itemI = systemValues.FirstOrDefault(y => y.Name == "Code");

					if (itemI != null)
					{
						var originalItem = _repository.JurisdictionGroups.Where(x => x.Code == itemI.Value).FirstOrDefault();
						if (originalItem == null)
						{
							var addItem = InitializeItem(null, systemValues);							
							_repository.Add(addItem);
							originalItem = addItem;
						}

						var jurisdictionCode = systemValues.First(x => x.Name == "JurisdictionCode").Value;

						if (jurisdictionCode != null)
						{
							var jItem = _repository.Jurisdictions.Where(x => x.Code == jurisdictionCode).SingleOrDefault();
							if (jItem != null)
							{
								var relation = new JurisdictionRelation() { JurisdictionId = jItem.JurisdictionId, JurisdictionGroupId = originalItem.JurisdictionGroupId };
								_repository.Add(relation);
							}
						}
					}

					break;
				case ImportAction.InsertAndReplace:
					var itemR = systemValues.FirstOrDefault(y => y.Name == "Code");
					if (itemR != null)
					{
						var originalItem = _repository.JurisdictionGroups.Where(x => x.Code == itemR.Value).SingleOrDefault();
						if (originalItem != null)
							_repository.Remove(originalItem);
					}

					var replaceItem = InitializeItem(null, systemValues);
					_repository.Add(replaceItem);
					break;
				case ImportAction.Update:
					var itemU = systemValues.FirstOrDefault(y => y.Name == "JurisdictionCode");
					if (itemU != null)
					{
						var origItem = _repository.JurisdictionGroups.Where(x => x.Code == itemU.Value).SingleOrDefault();
						if (origItem != null)
						{
							InitializeItem(origItem, systemValues);
							_repository.Update(origItem);
						}
					}
					break;
				case ImportAction.Delete:
					var itemD = systemValues.FirstOrDefault(y => y.Name == "JurisdictionCode");
					if (itemD != null)
					{
						var deleteItem = _repository.JurisdictionGroups.Where(x => x.Code == itemD.Value).SingleOrDefault();
						if (deleteItem != null)
							_repository.Remove(deleteItem);
					}
					break;
			}
			return _error;
		}

		private JurisdictionGroup GetOriginalItem(string itemId)
		{
			return null;
		}

		private static JurisdictionGroup InitializeItem(JurisdictionGroup item, IEnumerable<ImportItem> systemValues)
		{
			if (item == null)
				item = new OrderEntityFactory().CreateEntity<JurisdictionGroup>();
			var itemProperties = item.GetType().GetProperties();
			systemValues.ToList().ForEach(x => SetPropertyValue(item, itemProperties.FirstOrDefault(y => y.Name == x.Name), x.Value));

			return item;
		}
	}
}
