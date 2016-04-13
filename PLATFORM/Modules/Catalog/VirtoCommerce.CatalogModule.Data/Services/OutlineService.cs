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

        public void FillOutlinesForObjects(IEnumerable<IHasOutlines> objects, string catalogId)
        {
            foreach (var obj in objects)
            {
                var item = ConvertToGenericItem(obj);

                obj.Outlines = new List<Outline>();
                AddOutlines(item, catalogId, obj.Outlines);
            }
        }


        private void AddOutlines(GenericItem item, string allowedCatalogId, ICollection<Outline> outlines)
        {
            // Add physical outline
            if (IsAllowedCatalog(item.CatalogId, allowedCatalogId))
            {
                var outline = CreateOutline(item.CatalogId);
                outline.Items.AddRange(item.Parents.Select(ConvertToOutlineItem));
                outline.Items.Add(ConvertToOutlineItem(item));
                outlines.Add(outline);
            }

            // Add virtual outlines for parent links
            var lastItem = ConvertToOutlineItem(item);
            var parents = new List<OutlineItem>();

            foreach (var parent in item.Parents.Reverse())
            {
                parents = parents.Select(Clone).ToList();
                parents.Insert(0, ConvertToOutlineItem(parent, true));
                AddOutlinesForLinks(parent.Links, parents, lastItem, allowedCatalogId, outlines);
            }

            // Add virtual outlines for item links
            lastItem = ConvertToOutlineItem(item, true);
            AddOutlinesForLinks(item.Links, null, lastItem, allowedCatalogId, outlines);
        }

        private void AddOutlinesForLinks(IEnumerable<CategoryLink> links, List<OutlineItem> parents, OutlineItem lastItem, string allowedCatalogId, ICollection<Outline> outlines)
        {
            foreach (var link in links)
            {
                if (IsAllowedCatalog(link.CatalogId, allowedCatalogId))
                {
                    var outline = CreateOutline(link.CatalogId);

                    if (link.CategoryId != null)
                    {
                        var category = GetCategoryItem(link.CategoryId, true);
                        outline.Items.AddRange(category.Parents.Select(ConvertToOutlineItem));
                        outline.Items.Add(ConvertToOutlineItem(category));
                    }

                    if (parents != null)
                    {
                        outline.Items.AddRange(parents);
                    }

                    outline.Items.Add(lastItem);
                    outlines.Add(outline);
                }
            }
        }

        private static bool IsAllowedCatalog(string actualCatalogId, string allowedCatalogId)
        {
            return allowedCatalogId == null || string.Equals(allowedCatalogId, actualCatalogId, StringComparison.OrdinalIgnoreCase);
        }

        private static Outline CreateOutline(string catalogId)
        {
            return new Outline
            {
                Items = new List<OutlineItem>
                {
                    new OutlineItem { Id = catalogId, SeoObjectType = "Catalog" }
                }
            };
        }

        private static OutlineItem Clone(OutlineItem item)
        {
            return item == null ? null : new OutlineItem
            {
                Id = item.Id,
                SeoObjectType = item.SeoObjectType,
            };
        }

        private static OutlineItem ConvertToOutlineItem(GenericItem item)
        {
            return ConvertToOutlineItem(item, false);
        }

        private static OutlineItem ConvertToOutlineItem(GenericItem item, bool isLinkTarget)
        {
            return new OutlineItem
            {
                Id = item.Id,
                SeoObjectType = item.SeoObjectType,
                IsLinkTarget = isLinkTarget,
            };
        }

        private GenericItem ConvertToGenericItem(IHasOutlines obj, bool convertParents = true)
        {
            GenericItem result = null;

            var category = obj as Category;
            if (category != null)
            {
                result = GetCategoryItem(category.Id, convertParents);
            }

            var product = obj as CatalogProduct;
            if (product != null)
            {
                result = new GenericItem
                {
                    Id = product.Id,
                    SeoObjectType = product.SeoObjectType,
                    CatalogId = product.CatalogId,
                    Parents = new List<GenericItem>(),
                    Links = new List<CategoryLink>(product.Links),
                };

                if (product.CategoryId != null)
                {
                    var productCategory = GetCategoryItem(product.CategoryId, convertParents);
                    result.Parents.AddRange(productCategory.Parents);
                    result.Parents.Add(productCategory);
                }
            }

            return result;
        }

        private GenericItem GetCategoryItem(string categoryId, bool convertParents)
        {
            var category = GetCategory(categoryId);

            var result = new GenericItem
            {
                Id = category.Id,
                SeoObjectType = category.SeoObjectType,
                CatalogId = category.CatalogId,
                Links = category.Links,
            };

            if (convertParents)
            {
                result.Parents = category.Parents
                    .Select(c => ConvertToGenericItem(c, false))
                    .ToList();
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

        private class GenericItem
        {
            public string Id { get; set; }
            public string SeoObjectType { get; set; }
            public string CatalogId { get; set; }
            public ICollection<GenericItem> Parents { get; set; }
            public ICollection<CategoryLink> Links { get; set; }
        }
    }
}
