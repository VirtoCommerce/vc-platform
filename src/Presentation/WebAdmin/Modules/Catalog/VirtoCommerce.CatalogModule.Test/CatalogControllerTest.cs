using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using foundation = VirtoCommerce.Foundation.Catalogs.Model;
using module = VirtoCommerce.CatalogModule.Model;
using VirtoCommerce.CatalogModule.Data.Converters;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.CatalogModule.Repositories;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Data.Services;
using VirtoCommerce.CatalogModule.Services;
using VirtoCommerce.CatalogModule.Model;
using System.Collections.Generic;
using VirtoCommerce.CatalogModule.Web.Controllers.Api;
using moduleModel = VirtoCommerce.CatalogModule.Model;
using webModel = VirtoCommerce.CatalogModule.Web.Model;
using System.Web.Http.Results;

namespace VirtoCommerce.CatalogModule.Test
{
	[TestClass]
	public class CatalogControllerTest
	{
		[TestMethod]
		public void VirtualCatalogWorkingTest()
		{
			
			var catalogController = new CatalogsController(GetCatalogService(), GetSearchService(), null);
			var categoryController = new CategoriesController(GetCategoryService(), GetPropertyService());
            var listEntryController = new ListEntryController(GetSearchService(), GetCategoryService());
			
			//Create virtual catalog
			var catalogResult = catalogController.GetNewVirtualCatalog() as OkNegotiatedContentResult<webModel.Catalog>;
			var vCatalog = catalogResult.Content;
			vCatalog.Name = "vCatalog1";
			catalogController.Post(vCatalog);

			Assert.IsTrue(vCatalog.Virtual);

			//Create virtual category
			var categoryResult = categoryController.GetNewCategory(vCatalog.Id) as OkNegotiatedContentResult<webModel.Category>;
			var vCategory = categoryResult.Content;
			vCatalog.Name = "vCategory";
			categoryController.Post(vCategory);

			Assert.IsTrue(vCategory.Virtual);

			//Link category to virtual category
			var link = new webModel.CategoryLink { SourceCategoryId = "40773cd0-f2de-462f-9041-da742a274c38", CatalogId = vCatalog.Id, CategoryId = vCategory.Id };
            listEntryController.CreateLinks(new webModel.CategoryLink[] { link });
		

			//Check result
			var serachResult = listEntryController.ListItemsSearch(new webModel.SearchCriteria { CatalogId = vCatalog.Id, 
																							 CategoryId = vCategory.Id, 
																							 ResponseGroup = webModel.ResponseGroup.WithCategories })
																							 as  OkNegotiatedContentResult<webModel.ListEntrySearchResult>;
			var listResult = serachResult.Content;
		
			Assert.IsTrue(listResult.ListEntries.Any());
			var category = listResult.ListEntries.First();
			Assert.IsTrue(category.Id == "40773cd0-f2de-462f-9041-da742a274c38");

			//Remove link
            listEntryController.DeleteLinks(new webModel.CategoryLink[] { link });

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
			return new CategoryServiceImpl(() => { return GetRepository(); }, null);
		}

		private ICatalogService GetCatalogService()
		{
			return new CatalogServiceImpl(() => { return GetRepository(); }, null);
		}

		private IItemService GetItemService()
		{
			return new ItemServiceImpl(() => { return GetRepository(); }, null);
		}


		private IFoundationCatalogRepository GetRepository()
		{
			var retVal = new FoundationCatalogRepositoryImpl("VirtoCommerce");
			return retVal;
		}
	}
}
