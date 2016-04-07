using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.CatalogModule.Data.Converters;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CatalogModule.Data.Services
{
    public class OutlineService : IOutlineService
    {
        private readonly Func<ICatalogRepository> _catalogRepositoryFactory;

        public OutlineService(Func<ICatalogRepository> catalogRepositoryFactory)
        {
            _catalogRepositoryFactory = catalogRepositoryFactory;
        }

        public void FillOutlinesForCategories(IEnumerable<Category> categories, string catalogId)
        {
            foreach (var category in categories)
            {
                category.Outlines = GetOutlines(category.Id, catalogId, null, null, null);
            }
        }

        public void FillOutlinesForProducts(IEnumerable<CatalogProduct> products, string catalogId)
        {
            foreach (var product in products)
            {
                product.Outlines = GetOutlines(product.CategoryId, catalogId, product.Links, product.Id, product.SeoObjectType);
            }
        }


        private List<Outline> GetOutlines(string categoryId, string allowedCatalogId, IEnumerable<CategoryLink> additionalLinks, string additionalItemId, string additionalItemType)
        {
            var outlines = new List<Outline>();

            var additionalItem = additionalItemId != null ? new OutlineItem { Id = additionalItemId, SeoObjectType = additionalItemType } : null;
            AddOutlinesForParentAndLinkedCategories(categoryId, false, null, outlines, allowedCatalogId, additionalItem);

            if (additionalLinks != null)
            {
                additionalItem = additionalItemId != null ? new OutlineItem { Id = additionalItemId, SeoObjectType = additionalItemType, IsLinkTarget = true } : null;
                AddOutlinesForLinks(additionalLinks, null, outlines, allowedCatalogId, additionalItem);
            }

            return outlines;
        }

        private void AddOutlinesForParentAndLinkedCategories(string categoryId, bool isLinkTarget, Outline partialOutline, List<Outline> outlines, string allowedCatalogId, OutlineItem additionalItem)
        {
            var category = GetCategory(categoryId);
            var newOutline = CreateOutline(categoryId, category.SeoObjectType, isLinkTarget, partialOutline);

            if (string.IsNullOrEmpty(category.ParentId))
            {
                AddFinalOutline(category.CatalogId, false, newOutline, outlines, allowedCatalogId, additionalItem);
            }
            else
            {
                AddOutlinesForParentAndLinkedCategories(category.ParentId, false, newOutline, outlines, allowedCatalogId, additionalItem);
            }

            AddOutlinesForLinks(category.Links, newOutline, outlines, allowedCatalogId, additionalItem);
        }

        private void AddOutlinesForLinks(IEnumerable<CategoryLink> links, Outline partialOutline, List<Outline> outlines, string allowedCatalogId, OutlineItem additionalItem)
        {
            foreach (var link in links)
            {
                if (string.IsNullOrEmpty(link.CategoryId))
                {
                    AddFinalOutline(link.CatalogId, true, partialOutline, outlines, allowedCatalogId, additionalItem);
                }
                else
                {
                    AddOutlinesForParentAndLinkedCategories(link.CategoryId, true, partialOutline, outlines, allowedCatalogId, additionalItem);
                }
            }
        }

        private static void AddFinalOutline(string actualCatalogId, bool isLinkTarget, Outline partialOutline, List<Outline> outlines, string allowedCatalogId, OutlineItem additionalItem)
        {
            if (allowedCatalogId == null || string.Equals(allowedCatalogId, actualCatalogId, StringComparison.OrdinalIgnoreCase))
            {
                var finalOutline = CreateOutline(actualCatalogId, "Catalog", isLinkTarget, partialOutline);

                if (additionalItem != null)
                {
                    finalOutline.Items.Add(new OutlineItem { Id = additionalItem.Id, SeoObjectType = additionalItem.SeoObjectType, IsLinkTarget = additionalItem.IsLinkTarget });
                }

                outlines.Add(finalOutline);
            }
        }

        private static Outline CreateOutline(string firstItemId, string firstItemType, bool isLinkTarget, Outline partialOutline)
        {
            var result = new Outline
            {
                Items = new List<OutlineItem>
                {
                    new OutlineItem { Id = firstItemId, SeoObjectType = firstItemType }
                }
            };

            if (partialOutline != null && partialOutline.Items != null && partialOutline.Items.Any())
            {
                var partialOutlneItems = partialOutline.Items
                    .Select(i => new OutlineItem { Id = i.Id, SeoObjectType = i.SeoObjectType, IsLinkTarget = i.IsLinkTarget })
                    .ToList();

                partialOutlneItems[0].IsLinkTarget = isLinkTarget;

                result.Items.AddRange(partialOutlneItems);
            }

            return result;
        }

        private Category GetCategory(string categoryId)
        {
            using (var repository = _catalogRepositoryFactory())
            {
                var result = repository.GetCategoriesByIds(new[] { categoryId }, CategoryResponseGroup.WithParents | CategoryResponseGroup.WithLinks)
                    .Select(c => c.ToCoreModel())
                    .FirstOrDefault();
                return result;
            }
        }
    }
}
