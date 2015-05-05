using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using VirtoCommerce.CatalogModule.Data.Converters;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Data.Infrastructure;
using dataModel = VirtoCommerce.CatalogModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Domain.Commerce.Services;
using VirtoCommerce.Domain.Commerce.Model;

namespace VirtoCommerce.CatalogModule.Data.Services
{
	public class CategoryServiceImpl : ServiceBase, ICategoryService
    {
        private readonly Func<ICatalogRepository> _catalogRepositoryFactory;
		private readonly ICommerceService _commerceService;
        public CategoryServiceImpl(Func<ICatalogRepository> catalogRepositoryFactory, ICommerceService commerceService)
        {
            _catalogRepositoryFactory = catalogRepositoryFactory;
			_commerceService = commerceService;
        }

        #region ICategoryService Members

        public coreModel.Category GetById(string categoryId)
        {
			coreModel.Category retVal = null;
            using (var repository = _catalogRepositoryFactory())
	        {
                var dbCategory = repository.GetCategoryById(categoryId);
				if (dbCategory != null)
				{
					var dbCatalog = repository.GetCatalogById(dbCategory.CatalogId);
					var dbProperties = repository.GetAllCategoryProperties(dbCategory);
					var dbLinks = repository.GetCategoryLinks(categoryId);
					var seoInfos = _commerceService.GetSeoKeywordsForEntity(categoryId).ToArray();

					var catalog = dbCatalog.ToCoreModel();
					var properties = dbProperties.Select(x => x.ToCoreModel(catalog, dbCategory.ToCoreModel(catalog, null, dbLinks)))
												 .ToArray();
					var allParents = repository.GetAllCategoryParents(dbCategory);

					retVal = dbCategory.ToCoreModel(catalog, properties, dbLinks, seoInfos, allParents);
				}
            }
            return retVal;
        }

        public coreModel.Category Create(coreModel.Category category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            var dbCategory = category.ToDataModel();
            coreModel.Category retVal = null;
            using (var repository = _catalogRepositoryFactory())
            {
				//Need add seo separately
				if (category.SeoInfos != null)
				{
					foreach(var seoInfo in category.SeoInfos)
					{
						var dbSeoInfo = seoInfo.ToCoreModel(dbCategory);
						repository.Add(dbSeoInfo);
					}
				}

                repository.Add(dbCategory);

                CommitChanges(repository);
            }
            retVal = GetById(dbCategory.Id);
            return retVal;
        }

        public void Update(coreModel.Category[] categories)
        {
            using (var repository = _catalogRepositoryFactory())
			using (var changeTracker = base.GetChangeTracker(repository))
            {
				foreach (var category in categories)
				{
					var dbCategory = repository.GetCategoryById(category.Id) as dataModel.Category;
					
					if (dbCategory == null)
					{
						throw new NullReferenceException("dbCategory");
					}

					//Patch SeoInfo  separately
					if (category.SeoInfos != null)
					{
						var dbSeoInfos = new ObservableCollection<SeoUrlKeyword>(_commerceService.GetSeoKeywordsForEntity(category.Id));
						var changedSeoInfos = category.SeoInfos.Select(x => x.ToCoreModel(dbCategory)).ToList();
						dbSeoInfos.ObserveCollection(x => _commerceService.UpsertSeoKeyword(x), x => _commerceService.DeleteSeoKeywords(new string[] { x.Id }));

						changedSeoInfos.Patch(dbSeoInfos, (source, target) => _commerceService.UpsertSeoKeyword(source));

					}
			

					//Patch  Links  separately
					if (category.Links != null)
					{
						var dbLinks = new ObservableCollection<dataModel.LinkedCategory>(repository.GetCategoryLinks(category.Id));
						var changedLinks = category.Links.Select(x => x.ToDataModel(category)).ToList();
						dbLinks.ObserveCollection(x => repository.Add(x), x => { repository.Attach(x); repository.Remove(x); });
						changedLinks.Patch(dbLinks, new LinkedCategoryComparer(), (source, target) => source.Patch(target));
					}

					var dbCategoryChanged = category.ToDataModel();
					changeTracker.Attach(dbCategory);
				
					dbCategoryChanged.Patch(dbCategory);
				}
				CommitChanges(repository);

			}
        }

        public void Delete(string[] categoryIds)
        {
            using (var repository = _catalogRepositoryFactory())
            {
                foreach (var categoryId in categoryIds)
                {
                    var dbCategory = repository.GetCategoryById(categoryId);
                    repository.Remove(dbCategory);
                }
                CommitChanges(repository);
            }
        }

        #endregion
    }
}
