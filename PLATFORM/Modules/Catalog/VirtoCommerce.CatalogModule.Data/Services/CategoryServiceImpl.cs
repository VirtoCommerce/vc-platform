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
using System.Collections.Generic;

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
        public coreModel.Category[] GetByIds(string[] categoryIds, coreModel.CategoryResponseGroup responseGroup)
        {
            var retVal = new List<coreModel.Category>();
            using (var repository = _catalogRepositoryFactory())
            {
                var seoInfos = new List<SeoInfo>();
                if ((responseGroup & coreModel.CategoryResponseGroup.WithSeo) == coreModel.CategoryResponseGroup.WithSeo)
                {
                    seoInfos = _commerceService.GetObjectsSeo(categoryIds).ToList();
                }

                var dbCategories = repository.GetCategoriesByIds(categoryIds, responseGroup);
                foreach (var dbCategory in dbCategories)
                {
                    var category = dbCategory.ToCoreModel();
                    retVal.Add(category);
                    category.SeoInfos = seoInfos.Where(x => x.ObjectId == dbCategory.Id).ToList();
                }

            }
            return retVal.ToArray();
        }

        public coreModel.Category GetById(string categoryId, coreModel.CategoryResponseGroup responseGroup)
        {
            return GetByIds(new[] { categoryId }, responseGroup).FirstOrDefault();
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
            return GetById(dbCategory.Id, Domain.Catalog.Model.CategoryResponseGroup.Info);
        }

        public void Update(coreModel.Category[] categories)
        {
            using (var repository = _catalogRepositoryFactory())
			using (var changeTracker = base.GetChangeTracker(repository))
            {
				foreach (var category in categories)
				{
                    var dbCategory = repository.GetCategoriesByIds(new[] { category.Id }, Domain.Catalog.Model.CategoryResponseGroup.Full).FirstOrDefault();
					
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
