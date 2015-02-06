using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Web.Http.Results;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Data.Services;
using VirtoCommerce.CatalogModule.Web.Controllers.Api;
using webModel = VirtoCommerce.CatalogModule.Web.Model;
using VirtoCommerce.Domain.Catalog.Services;

namespace VirtoCommerce.CatalogModule.Test
{
    [TestClass]
    public class CatalogControllerTest
    {
        [TestMethod]
        public void VirtualCatalogWorkingTest()
        {

            var catalogController = new CatalogsController(GetCatalogService(), GetSearchService(), null);
            var categoryController = new CategoriesController(GetSearchService(), GetCategoryService(), GetPropertyService(), GetCatalogService());
            var listEntryController = new ListEntryController(GetSearchService(), GetCategoryService(), GetItemService(), null);

            //Create virtual catalog
            var catalogResult = catalogController.GetNewVirtualCatalog() as OkNegotiatedContentResult<webModel.Catalog>;
            var vCatalog = catalogResult.Content;
            vCatalog.Name = "vCatalog1";
            catalogController.Update(vCatalog);

            Assert.IsTrue(vCatalog.Virtual);

            //Create virtual category
            var categoryResult = categoryController.GetNewCategory(vCatalog.Id) as OkNegotiatedContentResult<webModel.Category>;
            var vCategory = categoryResult.Content;
            vCatalog.Name = "vCategory";
            categoryController.Post(vCategory);

            Assert.IsTrue(vCategory.Virtual);

            //Link category to virtual category
            var link = new webModel.ListEntryLink { ListEntryId = "40773cd0-f2de-462f-9041-da742a274c38", ListEntryType = "Category", CatalogId = vCatalog.Id, CategoryId = vCategory.Id };
            listEntryController.CreateLinks(new webModel.ListEntryLink[] { link });


            //Check result
            var serachResult = listEntryController.ListItemsSearch(new webModel.ListEntrySearchCriteria
            {
                CatalogId = vCatalog.Id,
                CategoryId = vCategory.Id,
                ResponseGroup = webModel.ResponseGroup.WithCategories
            })
                as OkNegotiatedContentResult<webModel.ListEntrySearchResult>;
            var listResult = serachResult.Content;

            Assert.IsTrue(listResult.ListEntries.Any());
            var category = listResult.ListEntries.First();
            Assert.IsTrue(category.Id == "40773cd0-f2de-462f-9041-da742a274c38");

            //Remove link
            //listEntryController.DeleteLinks(new webModel.ListEntryLink[] { link });

        }

        [TestMethod]
        public void AssociationTest()
        {
            //Get all product associations
            var productController = new ProductsController(GetItemService(), GetPropertyService(), null);
            var productResult = productController.Get("v-b004y45rxi") as OkNegotiatedContentResult<webModel.Product>;
            var product = productResult.Content;
            Assert.IsFalse(product.Associations.Any());


            //Add association
            var association = new webModel.ProductAssociation
            {
                ProductId = "v-b0007zl6ds",
                Name = "Related Items"
            };
            product.Associations.Add(association);
            productController.Update(product);


            //Remove
            product.Associations.Remove(product.Associations.Last());
            productController.Update(product);

        }

        private ICatalogSearchService GetSearchService()
        {
            return new CatalogSearchServiceImpl(GetRepository, GetItemService(), GetCatalogService(), GetCategoryService());
        }

        private IPropertyService GetPropertyService()
        {
            return new PropertyServiceImpl(() => { return GetRepository(); }, null);
        }

        private ICategoryService GetCategoryService()
        {
            return new CategoryServiceImpl(() => { return GetRepository(); }, () => { return GetAppConfigRepository(); }, null);
        }

        private ICatalogService GetCatalogService()
        {
            return new CatalogServiceImpl(() => { return GetRepository(); }, null);
        }

        private IItemService GetItemService()
        {
            return new ItemServiceImpl(() => { return GetRepository(); }, () => { return GetAppConfigRepository(); }, null);
        }


        private IFoundationCatalogRepository GetRepository()
        {
            var retVal = new FoundationCatalogRepositoryImpl("VirtoCommerce");
            return retVal;
        }

        private IFoundationAppConfigRepository GetAppConfigRepository()
        {
            var retVal = new FoundationAppConfigRepositoryImpl("VirtoCommerce");
            return retVal;
        }
    }
}
