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

            var backupObject = GetBackupObject();
            backupObject.Catalogs.ForEach(x=>x.DefaultLanguage.Catalog=null);
            backupObject.SerializeJson(backupStream);
        }

        public void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(prodgressInfo);

            var backupObject = backupStream.DeserializeJson<BackupObject>();
            var originalObject = GetBackupObject();

            UpdateCatalogs(originalObject.Catalogs, backupObject.Catalogs);
            UpdateCategories(originalObject.Categories, backupObject.Categories);
            UpdateProperties(originalObject.Properties, backupObject.Properties);
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

        private BackupObject GetBackupObject()
        {
            const ResponseGroup respounceGroup = ResponseGroup.WithProducts | ResponseGroup.WithCatalogs | ResponseGroup.WithCategories
                | ResponseGroup.WithProperties | ResponseGroup.WithVariations;
            var respounce = _catalogSearchService.Search(new SearchCriteria { Count = int.MaxValue, GetAllCategories = true, Start = 0, ResponseGroup = respounceGroup });
            return new BackupObject
            {
                Catalogs = respounce.Catalogs.Select(x => _catalogService.GetById(x.Id)).ToArray(),
                Categories = respounce.Categories.Select(x => _categoryService.GetById(x.Id)).ToArray(),
                Products = null,//respounce.Products.Select(x => _itemService.GetById(x.Id, ItemResponseGroup.ItemMedium | ItemResponseGroup.Variations | ItemResponseGroup.Seo)).ToArray(),
                Properties = respounce.PropertyValues.Select(x => _propertyService.GetById(x.Id)).ToArray(),
            };
        }

    
    }
}