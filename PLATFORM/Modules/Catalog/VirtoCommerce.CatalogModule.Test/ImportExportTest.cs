using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Data.Services;
using VirtoCommerce.CatalogModule.Web.ExportImport;
using VirtoCommerce.CoreModule.Data.Repositories;
using VirtoCommerce.CoreModule.Data.Services;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Commerce.Services;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using Xunit;

namespace VirtoCommerce.CatalogModule.Test
{
    public class ImportExportTest
    {
        [Fact]
        public void ExportProductsTest()
        {
            var searchService = GetSearchService();
            var itemService = GetItemService();
            var searchResult = searchService.Search(new SearchCriteria { CatalogId = "Sony", CategoryId = "66b58f4c-fd62-4c17-ab3b-2fb22e82704a", Skip = 0, Take = 10, ResponseGroup = SearchResponseGroup.WithProducts });

            var csvProducts = searchResult.Products
                .Select(p => itemService.GetById(p.Id, ItemResponseGroup.ItemLarge))
                .Select(p => new CsvProduct(p, null, null, null))
                .ToList();

            var importConfiguration = GetMappingConfiguration();
            importConfiguration.PropertyCsvColumns = csvProducts
                .SelectMany(x => x.PropertyValues)
                .Select(x => x.PropertyName)
                .Distinct()
                .ToArray();

            using (var csvWriter = new CsvWriter(new StreamWriter(@"c:\Projects\VCF\vc-community\PLATFORM\Modules\Catalog\VirtoCommerce.CatalogModule.Test\products.csv")))
            {
                csvWriter.Configuration.Delimiter = ";";
                csvWriter.Configuration.RegisterClassMap(new CsvProductMap(importConfiguration));

                csvWriter.WriteHeader<CsvProduct>();

                foreach (var product in csvProducts)
                {
                    csvWriter.WriteRecord(product);
                }
            }
        }

        [Fact]
        public void ImportProductsTest()
        {
            //Auto detect mapping configuration
            var importConfiguration = GetMappingConfiguration();

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
            }
        }

        private static CsvProductMappingConfiguration GetMappingConfiguration()
        {
            return new CsvProductMappingConfiguration();
        }

        private static ICatalogSearchService GetSearchService()
        {
            return new CatalogSearchServiceImpl(GetCatalogRepository, GetItemService(), GetCatalogService(), GetCategoryService());
        }

        private static IOutlineService GetOutlineService()
        {
            return new OutlineService(GetCatalogRepository);
        }

        private static ICategoryService GetCategoryService()
        {
            return new CategoryServiceImpl(GetCatalogRepository, GetCommerceService(), GetOutlineService());
        }

        private static ICatalogService GetCatalogService()
        {
            return new CatalogServiceImpl(GetCatalogRepository, GetCommerceService());
        }

        private static IItemService GetItemService()
        {
            return new ItemServiceImpl(GetCatalogRepository, GetCommerceService(), GetOutlineService());
        }

        private static ICommerceService GetCommerceService()
        {
            return new CommerceServiceImpl(() => new CommerceRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor(null)));
        }

        private static ICatalogRepository GetCatalogRepository()
        {
            var retVal = new CatalogRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor(null));
            return retVal;
        }
    }
}
