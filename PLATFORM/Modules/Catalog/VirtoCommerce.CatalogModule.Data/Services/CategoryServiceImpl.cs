using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.CatalogModule.Data.Converters;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Commerce.Model;
using VirtoCommerce.Domain.Commerce.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Data.Infrastructure;
using coreModel = VirtoCommerce.Domain.Catalog.Model;

namespace VirtoCommerce.CatalogModule.Data.Services
{
    public class CategoryServiceImpl : ServiceBase, ICategoryService
    {
        private readonly Func<ICatalogRepository> _catalogRepositoryFactory;
        private readonly ICommerceService _commerceService;
        private readonly IOutlineService _outlineService;

        public CategoryServiceImpl(Func<ICatalogRepository> catalogRepositoryFactory, ICommerceService commerceService, IOutlineService outlineService)
        {
            _catalogRepositoryFactory = catalogRepositoryFactory;
            _commerceService = commerceService;
            _outlineService = outlineService;
        }

        #region ICategoryService Members
        public coreModel.Category[] GetByIds(string[] categoryIds, coreModel.CategoryResponseGroup responseGroup, string catalogId = null)
        {
            coreModel.Category[] result;

            using (var repository = _catalogRepositoryFactory())
            {
                result = repository.GetCategoriesByIds(categoryIds, responseGroup)
                    .Select(c => c.ToCoreModel())
                    .ToArray();
            }

            // Fill outlines for products
            if (responseGroup.HasFlag(coreModel.CategoryResponseGroup.WithOutlines))
            {
                _outlineService.FillOutlinesForObjects(result, catalogId);
            }

            // Fill SEO info
            if ((responseGroup & coreModel.CategoryResponseGroup.WithSeo) == coreModel.CategoryResponseGroup.WithSeo)
            {
                var objectsWithSeo = new List<ISeoSupport>(result);

                var outlineItems = result
                    .Where(c => c.Outlines != null)
                    .SelectMany(c => c.Outlines.SelectMany(o => o.Items));
                objectsWithSeo.AddRange(outlineItems);

                _commerceService.LoadSeoForObjects(objectsWithSeo.ToArray());
            }

            return result;
        }

        public coreModel.Category GetById(string categoryId, coreModel.CategoryResponseGroup responseGroup, string catalogId = null)
        {
            return GetByIds(new[] { categoryId }, responseGroup, catalogId).FirstOrDefault();
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
