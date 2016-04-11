using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using VirtoCommerce.CatalogModule.Data.Converters;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Domain.Catalog.Services;
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
                var categories = repository.GetCategoriesByIds(categoryIds, responseGroup).Select(x => x.ToCoreModel()).ToArray();
                retVal.AddRange(categories);
                if ((responseGroup & coreModel.CategoryResponseGroup.WithSeo) == coreModel.CategoryResponseGroup.WithSeo)
                {
                    _commerceService.LoadSeoForObjects(categories);
                }
            
            }
           
            return retVal.ToArray();
        }

        public coreModel.Category GetById(string categoryId, coreModel.CategoryResponseGroup responseGroup)
        {
            return GetByIds(new[] { categoryId }, responseGroup).FirstOrDefault();
        }

        public void Create(coreModel.Category[] categories)
        {
            if (categories == null)
                throw new ArgumentNullException("categories");

            var pkMap = new PrimaryKeyResolvingMap();
            var dbCategories = categories.Select(x => x.ToDataModel(pkMap));

            using (var repository = _catalogRepositoryFactory())
            {
                foreach (var dbCategory in dbCategories)
                {
                    repository.Add(dbCategory);
                }
                CommitChanges(repository);
                pkMap.ResolvePrimaryKeys();
            }
            //Need add seo separately
            _commerceService.UpsertSeoForObjects(categories);         
        }


        public coreModel.Category Create(coreModel.Category category)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            Create(new[] { category });
            return GetById(category.Id, Domain.Catalog.Model.CategoryResponseGroup.Info);
        }

        public void Update(coreModel.Category[] categories)
        {
            var pkMap = new PrimaryKeyResolvingMap();
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
					changeTracker.Attach(dbCategory);

					category.Patch(dbCategory, pkMap);
				}
				CommitChanges(repository);
                pkMap.ResolvePrimaryKeys();
            }
            //Update seo
            _commerceService.UpsertSeoForObjects(categories);
        }

        public void Delete(string[] categoryIds)
        {
            var categories = GetByIds(categoryIds, coreModel.CategoryResponseGroup.WithSeo);
            using (var repository = _catalogRepositoryFactory())
            {
                repository.RemoveCategories(categoryIds);
                CommitChanges(repository);
            }
            foreach (var category in categories)
            {
                _commerceService.DeleteSeoForObject(category);
            }
        }

        #endregion
    }
}
