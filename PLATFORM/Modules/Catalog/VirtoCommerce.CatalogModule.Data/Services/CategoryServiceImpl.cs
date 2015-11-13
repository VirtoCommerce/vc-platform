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
					//var dbLinks = repository.GetCategoryLinks(categoryId);

					var catalog = dbCatalog.ToCoreModel();
					var properties = dbProperties.Select(x => x.ToCoreModel(catalog, dbCategory.ToCoreModel(catalog)))
												 .ToArray();
					var allParents = repository.GetAllCategoryParents(dbCategory);

					retVal = dbCategory.ToCoreModel(catalog, properties, allParents);
					retVal.SeoInfos = _commerceService.GetObjectsSeo(new string[] { categoryId }).ToList();
				}
            }
            return retVal;
        }

        public coreModel.Category Create(coreModel.Category category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            var dbCategory = category.ToDataModel();
            
            using (var repository = _catalogRepositoryFactory())
            {	
                repository.Add(dbCategory);
                CommitChanges(repository);
            }
			//Need add seo separately
			if (category.SeoInfos != null)
			{
				foreach (var seoInfo in category.SeoInfos)
				{
					seoInfo.ObjectId = dbCategory.Id;
					seoInfo.ObjectType = typeof(coreModel.Category).Name;
					_commerceService.UpsertSeo(seoInfo);
				}
			}
			category.Id = dbCategory.Id;
            return GetById(dbCategory.Id);
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
						foreach(var seoInfo in category.SeoInfos)
						{
							seoInfo.ObjectId = category.Id;
							seoInfo.ObjectType = typeof(coreModel.Category).Name;
						}
						var seoInfos = new ObservableCollection<SeoInfo>(_commerceService.GetObjectsSeo(new string[] { category.Id}));
						seoInfos.ObserveCollection(x => _commerceService.UpsertSeo(x), x => _commerceService.DeleteSeo(new string[] { x.Id }));
						category.SeoInfos.Patch(seoInfos, (source, target) => _commerceService.UpsertSeo(source));
					}
		
					var dbCategoryChanged = category.ToDataModel();
					changeTracker.Attach(dbCategory);

					category.Patch(dbCategory);
				}
				CommitChanges(repository);

			}
        }

        public void Delete(string[] categoryIds)
        {
            using (var repository = _catalogRepositoryFactory())
            {
                var seoInfos = _commerceService.GetObjectsSeo(categoryIds);
                _commerceService.DeleteSeo(seoInfos.Select(x => x.Id).ToArray());

                repository.RemoveCategories(categoryIds);
                CommitChanges(repository);
            }
        }

        #endregion
    }
}
