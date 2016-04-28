using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Practices.ObjectBuilder2;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Asset;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Data.Common;

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
        private readonly IBlobStorageProvider _blobStorageProvider;

        public CatalogExportImport(ICatalogSearchService catalogSearchService,
            ICatalogService catalogService, ICategoryService categoryService, IItemService itemService,
            IPropertyService propertyService, IBlobStorageProvider blobStorageProvider)
        {
            _blobStorageProvider = blobStorageProvider;
            _catalogSearchService = catalogSearchService;
            _catalogService = catalogService;
            _categoryService = categoryService;
            _itemService = itemService;
            _propertyService = propertyService;
        }


        public void DoExport(Stream backupStream, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
        {
            var backupObject = GetBackupObject(progressCallback, manifest.HandleBinaryData);

            backupObject.SerializeJson(backupStream);
        }

        public void DoImport(Stream backupStream, PlatformExportManifest manifest, Action<ExportImportProgressInfo> progressCallback)
        {
            var progressInfo = new ExportImportProgressInfo();

            var backupObject = backupStream.DeserializeJson<BackupObject>();
            foreach(var category in backupObject.Categories)
            {
                category.Catalog = backupObject.Catalogs.FirstOrDefault(x => x.Id == category.CatalogId);
                if(category.Parents != null)
                {
                    category.Level = category.Parents.Count();
                }
            }
            var originalObject = GetBackupObject(progressCallback, false);

            progressInfo.Description = String.Format("{0} catalogs importing...", backupObject.Catalogs.Count());
            progressCallback(progressInfo);

            UpdateCatalogs(originalObject.Catalogs, backupObject.Catalogs);

            progressInfo.Description = String.Format("{0} categories importing...", backupObject.Categories.Count());
            progressCallback(progressInfo);
          
            backupObject.Products = backupObject.Products.OrderBy(x => x.MainProductId).ToList();
            UpdateCategories(originalObject.Categories, backupObject.Categories);
            UpdateProperties(originalObject.Properties, backupObject.Properties);

            //Binary data
            if (manifest.HandleBinaryData)
            {
                var allBackupImages = backupObject.Products.SelectMany(x => x.Images);
                allBackupImages = allBackupImages.Concat(backupObject.Categories.SelectMany(x => x.Images));
                allBackupImages = allBackupImages.Concat(backupObject.Products.SelectMany(x => x.Variations).SelectMany(x => x.Images));

                var allOrigImages = originalObject.Products.SelectMany(x => x.Images);
                allOrigImages = allOrigImages.Concat(originalObject.Categories.SelectMany(x => x.Images));
                allOrigImages = allOrigImages.Concat(originalObject.Products.SelectMany(x => x.Variations).SelectMany(x => x.Images));
                //Import only new images
                var allNewImages = allBackupImages.Where(x => !allOrigImages.Contains(x));
                var index = 0;
                var progressTemplate = "{0} of " + allNewImages.Count() + " images uploading";
                foreach (var image in allNewImages)
                {
                    progressInfo.Description = String.Format(progressTemplate, index);
                    progressCallback(progressInfo);
                    try
                    {
                        //do not save images with external url
                        if (image.Url != null && !image.Url.IsAbsoluteUrl())
                        {
                            using (var sourceStream = new MemoryStream(image.BinaryData))
                            using (var targetStream = _blobStorageProvider.OpenWrite(image.Url))
                            {
                                sourceStream.CopyTo(targetStream);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        progressInfo.Errors.Add(String.Format("{0}: {1}", "CatalogModule", ex.ExpandExceptionMessage()));
                        progressCallback(progressInfo);
                    }

                    index++;
                }
            }

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
            var toCreate = new List<Category>();
            backup.CompareTo(original, EqualityComparer<Category>.Default, (state, x, y) =>
            {
                switch (state)
                {
                    case EntryState.Modified:
                        toUpdate.Add(x);
                        break;
                    case EntryState.Added:
                        toCreate.Add(x);
                        break;
                }
            });
            _categoryService.Create(toCreate.ToArray());
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
            var toCreate = new List<CatalogProduct>();
            backup.CompareTo(original, EqualityComparer<CatalogProduct>.Default, (state, x, y) =>
            {
                switch (state)
                {
                    case EntryState.Modified:
                        toUpdate.Add(x);
                        break;
                    case EntryState.Added:
                        toCreate.Add(x);
                        break;
                }
            });
            _itemService.Update(toUpdate.ToArray());
            _itemService.Create(toCreate.ToArray());
        }

        private BackupObject GetBackupObject(Action<ExportImportProgressInfo> progressCallback, bool loadBinaryData)
        {
            var progressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(progressInfo);


            const SearchResponseGroup responseGroup = SearchResponseGroup.WithCatalogs | SearchResponseGroup.WithCategories | SearchResponseGroup.WithProducts;
            var searchResponse = _catalogSearchService.Search(new SearchCriteria { WithHidden = true, Take = int.MaxValue, Skip = 0, ResponseGroup = responseGroup });

            var retVal = new BackupObject();

            progressInfo.Description = String.Format("{0} catalogs loading", searchResponse.Catalogs.Count());
            progressCallback(progressInfo);

            //Catalogs
            retVal.Catalogs = searchResponse.Catalogs.Select(x => _catalogService.GetById(x.Id)).ToList();

            progressInfo.Description = String.Format("{0} categories loading", searchResponse.Categories.Count());
            progressCallback(progressInfo);
          
            //Categories
            retVal.Categories = _categoryService.GetByIds(searchResponse.Categories.Select(x=>x.Id).ToArray(), CategoryResponseGroup.Full);
         
            //Products
            for (int i = 0; i < searchResponse.Products.Count(); i += 50)
            {
                var products = _itemService.GetByIds(searchResponse.Products.Skip(i).Take(50).Select(x => x.Id).ToArray(), ItemResponseGroup.ItemLarge);
                retVal.Products.AddRange(products);

                progressInfo.Description = String.Format("{0} of {1} products loaded", Math.Min(searchResponse.ProductsTotalCount, i), searchResponse.ProductsTotalCount);
                progressCallback(progressInfo);
            }
            //Binary data
            if (loadBinaryData)
            {
                var allImages = retVal.Products.SelectMany(x => x.Images);
                allImages = allImages.Concat(retVal.Categories.SelectMany(x => x.Images));
                allImages = allImages.Concat(retVal.Products.SelectMany(x => x.Variations).SelectMany(x => x.Images));

                var index = 0;
                var progressTemplate = "{0} of " + allImages.Count() + " images downloading";
                foreach (var image in allImages)
                {
                    progressInfo.Description = String.Format(progressTemplate, index);
                    progressCallback(progressInfo);
                    try
                    {
                        using (var stream = _blobStorageProvider.OpenRead(image.Url))
                        {
                            image.BinaryData = stream.ReadFully();
                        }
                    }
                    catch (Exception ex)
                    {
                        progressInfo.Errors.Add(ex.ToString());
                        progressCallback(progressInfo);
                    }
                    index++;
                }
            }

            //Properties
            progressInfo.Description = String.Format("Properties loading");
            progressCallback(progressInfo);

            retVal.Properties = _propertyService.GetAllProperties();

            //Reset some props to descrease resulting json size
            foreach (var catalog in retVal.Catalogs)
            {
                catalog.Properties = null;
            }

            foreach (var category in retVal.Categories)
            {
                category.Catalog = null;
                category.Properties = null;
                category.Children = null;
                category.Parents = null;
                foreach (var propvalue in category.PropertyValues)
                {
                    propvalue.Property = null;
                }
            }
            foreach (var product in retVal.Products.Concat(retVal.Products.SelectMany(x=>x.Variations)))
            {
                product.Catalog = null;
                product.Category = null;
                product.Properties = null;
                product.MainProduct = null;
                foreach (var propvalue in product.PropertyValues)
                {
                    propvalue.Property = null;
                }
            }
            return retVal;

        }

    }

}