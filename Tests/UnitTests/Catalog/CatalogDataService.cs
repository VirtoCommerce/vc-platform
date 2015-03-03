using System;
using System.Linq;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Data.Catalogs;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using Xunit;

namespace CommerceFoundation.UnitTests.CatalogTests
{
    public class CatalogDataService
    {
        [Fact]
        // fails with "Resource not found for the segment 'CategoryPropertyValues'"
        public void DeleteCategoryPropertyValue_ExistingCategory()
        {
            var ServiceUri = new Uri("http://localhost/store/DataServices/CatalogDataService.svc");
            ICatalogRepository client = new DSCatalogClient(ServiceUri, new CatalogEntityFactory(), null);
            var category1 = client.Categories
                                .OfType<Category>()
                                .Expand("CategoryPropertyValues")
                                .First();

            var category = category1.DeepClone(new CatalogEntityFactory()) as Category;
            client = new DSCatalogClient(ServiceUri, new CatalogEntityFactory(), null);
            client.Attach(category);

            /*
            var category = category1.DeepClone(new CatalogEntityFactory()) as Category;
            client = new DSCatalogClient(ServiceUri, new CatalogEntityFactory());
            client.Attach(category);

            //// adding: works
            var item = new CategoryPropertyValue { IntegerValue = 1, Name = "unit tst", ValueType = PropertyValueType.Integer.GetHashCode() };
            item.CategoryId = category.CategoryId;
            client.Add(item);
            category.CategoryPropertyValues.Add(item);
            client.UnitOfWork.Commit();
            */

            // deleting: fails
            var item = category.CategoryPropertyValues[0] as CategoryPropertyValue;
            //client.Attach(item);
            client.Remove(item);
            category.CategoryPropertyValues.Remove((CategoryPropertyValue)item);
            client.UnitOfWork.Commit();
        }
    }
}
