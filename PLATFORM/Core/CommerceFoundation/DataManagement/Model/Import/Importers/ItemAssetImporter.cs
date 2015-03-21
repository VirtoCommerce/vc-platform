using System.IO;
using System.Linq;
using System.Collections.Generic;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.Foundation.Importing.Model
{
	public class ItemAssetImporter : EntityImporterBase
	{
		private static readonly string[] imageTypes = new[] {"jpg", "jpeg", "png", "bmp", "gif"};
		private const string imageType = "image";

		private ICatalogRepository _repository;

		public ItemAssetImporter()
		{
			Name = ImportEntityType.ItemAsset.ToString();
			InitializeSystemProperties();
		}

		private void InitializeSystemProperties()
		{
			var action = new ImportProperty { Name = "Action", DisplayName = "Action", IsRequiredProperty = true, IsEntityProperty = false, EntityImporterId = Name, DefaultValue = "Insert", IsEnumValuesProperty = true, EnumValues = { "Insert", "Insert & Update", "Update", "Delete" } };
			var assetType = new ImportProperty { Name = "AssetType", DisplayName = "Asset type", IsRequiredProperty = false, IsEntityProperty = true, EntityImporterId = Name };
			var itemId = new ImportProperty { Name = "ItemId", DisplayName = "Item id", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			var assetId = new ImportProperty { Name = "AssetId", DisplayName = "Asset id (path)", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			var groupName = new ImportProperty { Name = "GroupName", DisplayName = "Group name", IsRequiredProperty = true, IsEntityProperty = true, EntityImporterId = Name };
			
			AddSystemProperties(
				action, assetType, itemId, assetId, groupName);
		}

		public override string Import(string catalogId, string propertySetId, ImportItem[] systemValues, ImportItem[] customValues, IRepository repository)
		{
			var _error = string.Empty;
			_repository = (ICatalogRepository)repository;

			var action = GetAction(systemValues.First(x => x.Name == "Action").Value);

			var itemId = systemValues.First(y => y.Name == "ItemId").Value;
			var itemAssetId = systemValues.First(y => y.Name == "AssetId").Value;
			var assetType = systemValues.First(y => y.Name == "AssetType").Value;

			var originalItem = _repository.Items.Where(x => x.ItemId == itemId || x.Code == itemId).Expand(it => it.ItemAssets).FirstOrDefault();
			if (originalItem != null)
			{
				switch (action)
				{
					case ImportAction.Insert:

						if (originalItem.ItemAssets.All(x => x.AssetId != itemAssetId))
						{
							var addItem = InitializeItem(null, systemValues);

							addItem.AssetType = !string.IsNullOrEmpty(assetType)
								                    ? addItem.AssetType
								                    : imageTypes.Any(x => x.Equals(Path.GetExtension(addItem.AssetId).Replace(".", string.Empty)))
									                      ? imageType
														  : Path.GetExtension(addItem.AssetId).Replace(".", string.Empty);

							originalItem.ItemAssets.Add(addItem);
							_repository.Update(originalItem);
						}
						break;
					case ImportAction.Update:
						if (originalItem.ItemAssets.Any(x => x.AssetId == itemAssetId))
						{
							var asset = originalItem.ItemAssets.FirstOrDefault(x => x.AssetId == itemAssetId);
							if (asset != null)
							{
								InitializeItem(asset, systemValues);
								_repository.Update(originalItem);
							}
						}
						break;
					case ImportAction.Delete:
						if (originalItem.ItemAssets.Any(x => x.AssetId == itemAssetId))
						{
							var assetD = originalItem.ItemAssets.FirstOrDefault(x => x.AssetId == itemAssetId);
							originalItem.ItemAssets.Remove(assetD);
							_repository.Update(originalItem);
						}
						break;
				}
			}
			return _error;
		}

		private static ItemAsset InitializeItem(ItemAsset item, IEnumerable<ImportItem> systemValues)
		{
			if (item == null)
				item = new ItemAsset();
			var itemProperties = item.GetType().GetProperties();
			systemValues.ToList().ForEach(x => SetPropertyValue(item, itemProperties.FirstOrDefault(y => y.Name == x.Name), x.Value));

			return item;
		}
	}
}
