using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Data.Services;
using VirtoCommerce.CatalogModule.Web.ExportImport;
using VirtoCommerce.CoreModule.Data.Repositories;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Commerce.Services;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
namespace VirtoCommerce.CatalogModule.Test
{
    [TestClass]
    public class ImportExportTest
    {
        [TestMethod]
        public void ExportProductsTest()
        {
            var searchService = GetSearchService();
            var categoryService = GetCategoryService();
            var itemService = GetItemService();
            var result = searchService.Search(new SearchCriteria { CatalogId = "Sony", CategoryId = "66b58f4c-fd62-4c17-ab3b-2fb22e82704a", Skip = 0, Take = 10, ResponseGroup = coreModel.SearchResponseGroup.WithProducts });
            var importConfiguration = GetMapConfiguration();

            using (var csvWriter = new CsvWriter(new StreamWriter(@"c:\Projects\VCF\vc-community\PLATFORM\Modules\Catalog\VirtoCommerce.CatalogModule.Test\products.csv")))
            {
                var csvProducts = new List<CsvProduct>();
                foreach (var product in result.Products)
                {
                    var fullLoadedProduct = itemService.GetById(product.Id, ItemResponseGroup.ItemLarge);
                    csvProducts.Add(new CsvProduct(fullLoadedProduct, null, null, null));
                }

                importConfiguration.PropertyCsvColumns = csvProducts.SelectMany(x => x.PropertyValues).Select(x => x.PropertyName).Distinct().ToArray();
                csvWriter.Configuration.Delimiter = ";";
                csvWriter.Configuration.RegisterClassMap(new CsvProductMap(importConfiguration));

                csvWriter.WriteHeader<CsvProduct>();
                foreach (var product in csvProducts)
                {
                    csvWriter.WriteRecord(product);
                }

            }
        }


        [TestMethod]
        public void ImportProductsTest()
        {
            //Auto detect mapping configuration
            var importConfiguration = GetMapConfiguration();


            var csvProducts = new List<CsvProduct>();
            using (var reader = new CsvReader(new StreamReader(@"c:\Projects\VCF\vc-community\PLATFORM\Modules\Catalog\VirtoCommerce.CatalogModule.Test\products.csv")))
            {
                reader.Configuration.Delimiter = ";";
                var initialized = false;
                while (reader.Read())
                {
                    if (!initialized)
                    {
                        importConfiguration.AutoMap(reader.FieldHeaders);
                        reader.Configuration.RegisterClassMap(new CsvProductMap(importConfiguration));
                        initialized = true;
                    }

                    var csvProduct = reader.GetRecord<CsvProduct>();
                    csvProducts.Add(csvProduct);
                }
            };

            var categories = new List<coreModel.Category>();


        }

        private CsvProductMappingConfiguration GetMapConfiguration()
        {
            return new CsvProductMappingConfiguration();
        }

        private ICatalogSearchService GetSearchService()
        {
            return new CatalogSearchServiceImpl(GetRepository, GetItemService(), GetCatalogService(), GetCategoryService());
        }

        private ICategoryService GetCategoryService()
        {
            return new CategoryServiceImpl(() => { return GetRepository(); }, GetCommerceService());
        }

        private ICatalogService GetCatalogService()
        {
            return new CatalogServiceImpl(() => { return GetRepository(); }, GetCommerceService());
        }

        private IItemService GetItemService()
        {
            return new ItemServiceImpl(() => { return GetRepository(); }, GetCommerceService());
        }

        private ICommerceService GetCommerceService()
        {
            return new CommerceServiceImpl(() => new CommerceRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor(null)));
        }
        private ICatalogRepository GetRepository()
        {
            var retVal = new CatalogRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor(null));
            return retVal;
        }
    }
}
