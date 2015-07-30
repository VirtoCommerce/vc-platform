using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Practices.ObjectBuilder2;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;

namespace VirtoCommerce.CatalogModule.Web.ExportImport
{
	public sealed class BackupObject
	{
		public BackupObject()
		{
			Catalogs = new List<Catalog>();
			Categories = new List<Category>();
			Products = new List<CatalogProduct>();
			Properties = new List<Property>();
		}
		public ICollection<Catalog> Catalogs { get; set; }
		public ICollection<Category> Categories { get; set; }
		public ICollection<CatalogProduct> Products { get; set; }
		public ICollection<Property> Properties { get; set; }
	}

	
	public sealed class CatalogExportImport
	{
		private readonly ICatalogService _catalogService;
		private readonly ICatalogSearchService _catalogSearchService;
		private readonly ICategoryService _categoryService;
		private readonly IItemService _itemService;
		private readonly IPropertyService _propertyService;

		public CatalogExportImport(ICatalogSearchService catalogSearchService,
			ICatalogService catalogService, ICategoryService categoryService, IItemService itemService, IPropertyService propertyService)
		{
			_catalogSearchService = catalogSearchService;
			_catalogService = catalogService;
			_categoryService = categoryService;
			_itemService = itemService;
			_propertyService = propertyService;
		}


		public void DoExport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
		{
			var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
			progressCallback(prodgressInfo);

			var backupObject = GetBackupObject(progressCallback);

			backupObject.SerializeJson(backupStream);
		}

		public void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
		{
			var progressInfo = new ExportImportProgressInfo { Description = "loading data..." };
			progressCallback(progressInfo);

			var backupObject = backupStream.DeserializeJson<BackupObject>();
			var originalObject = GetBackupObject(progressCallback);

			progressInfo.Description = String.Format("{0} catalogs importing...", backupObject.Catalogs.Count());
			progressCallback(progressInfo);

			UpdateCatalogs(originalObject.Catalogs, backupObject.Catalogs);

			progressInfo.Description = String.Format("{0} categories importing...", backupObject.Categories.Count());
			progressCallback(progressInfo);
			//Categories should be sorted right way (because it have a hierarchy structure and links to virtual categories)
			backupObject.Categories = backupObject.Categories.Where(x => x.Links == null || !x.Links.Any())
															.OrderBy(x => x.Parents != null ? x.Parents.Count() : 0)
															.Concat(backupObject.Categories.Where(x => x.Links != null && x.Links.Any()))
															.ToList();
			UpdateCategories(originalObject.Categories, backupObject.Categories);
			UpdateProperties(originalObject.Properties, backupObject.Properties);

			progressInfo.Description = String.Format("{0} products importing...", backupObject.Products.Count());
			progressCallback(progressInfo);
			UpdateCatalogProducts(originalObject.Products, backupObject.Products);
		}

		private void UpdateCatalogs(ICollection<Catalog> original, ICollection<Catalog> backup)
		{
			var toUpdate = new List<Catalog>();

			backup.CompareTo(original, EqualityComparer<Catalog>.Default, (state, x, y) =>
			{
				switch (state)
				{
					case EntryState.Modified:
						toUpdate.Add(x);
						break;
					case EntryState.Added:
						_catalogService.Create(x);
						break;
				}
			});
			_catalogService.Update(toUpdate.ToArray());
		}

		private void UpdateCategories(ICollection<Category> original, ICollection<Category> backup)
		{
			var toUpdate = new List<Category>();

			backup.CompareTo(original, EqualityComparer<Category>.Default, (state, x, y) =>
			{
				switch (state)
				{
					case EntryState.Modified:
						toUpdate.Add(x);
						break;
					case EntryState.Added:
						_categoryService.Create(x);
						break;
				}
			});
			_categoryService.Update(toUpdate.ToArray());
		}

		private void UpdateProperties(ICollection<Property> original, ICollection<Property> backup)
		{
			var toUpdate = new List<Property>();

			backup.CompareTo(original, EqualityComparer<Property>.Default, (state, x, y) =>
			{
				switch (state)
				{
					case EntryState.Modified:
						toUpdate.Add(x);
						break;
					case EntryState.Added:
						_propertyService.Create(x);
						break;
				}
			});
			_propertyService.Update(toUpdate.ToArray());
		}

		private void UpdateCatalogProducts(ICollection<CatalogProduct> original, ICollection<CatalogProduct> backup)
		{
			var toUpdate = new List<CatalogProduct>();

			backup.CompareTo(original, EqualityComparer<CatalogProduct>.Default, (state, x, y) =>
			{
				switch (state)
				{
					case EntryState.Modified:
						toUpdate.Add(x);
						break;
					case EntryState.Added:
						_itemService.Create(x);
						break;
				}
			});
			_itemService.Update(toUpdate.ToArray());
		}

		private BackupObject GetBackupObject(Action<ExportImportProgressInfo> progressCallback)
		{
			const ResponseGroup responseGroup = ResponseGroup.Full;
			var searchResponse = _catalogSearchService.Search(new SearchCriteria { Count = int.MaxValue, GetAllCategories = true, Start = 0, ResponseGroup = responseGroup });
			
			var retVal = new BackupObject();

			var progressInfo = new ExportImportProgressInfo();
			progressInfo.Description = String.Format("{0} catalogs loading", searchResponse.Catalogs.Count());
			progressCallback(progressInfo);

			retVal.Catalogs = searchResponse.Catalogs.Select(x => _catalogService.GetById(x.Id)).ToList();
		
			progressInfo.Description = String.Format("{0} categories loading", searchResponse.Categories.Count());
			progressCallback(progressInfo);

			retVal.Categories = searchResponse.Categories.Select(x => _categoryService.GetById(x.Id)).ToList();
		
			for (int i = 0; i < searchResponse.Products.Count(); i += 50)
			{
				var products = _itemService.GetByIds(searchResponse.Products.Skip(i).Take(50).Select(x => x.Id).ToArray(), ItemResponseGroup.ItemMedium | ItemResponseGroup.Variations | ItemResponseGroup.Seo);
				retVal.Products.AddRange(products);
			
				progressInfo.Description = String.Format("{0} of {1} products loaded", Math.Min(searchResponse.TotalCount, i), searchResponse.TotalCount);
				progressCallback(progressInfo);
			}


			var catalogsPropertiesIds = retVal.Catalogs.SelectMany(x => _propertyService.GetCatalogProperties(x.Id)).Select(x => x.Id).ToArray();
			var categoriesPropertiesIds = retVal.Categories.SelectMany(x => _propertyService.GetCategoryProperties(x.Id)).Select(x => x.Id).ToArray();
			var propertiesIds = catalogsPropertiesIds.Concat(categoriesPropertiesIds).Distinct().ToArray();

			progressInfo.Description = String.Format("{0} properties loading", propertiesIds.Count());
			progressCallback(progressInfo);

			retVal.Properties = propertiesIds.Select(x => _propertyService.GetById(x)).ToList();
			return retVal;

		}

	}

}