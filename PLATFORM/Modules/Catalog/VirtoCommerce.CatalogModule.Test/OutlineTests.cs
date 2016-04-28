using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Moq;
using VirtoCommerce.CatalogModule.Data.Model;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Data.Services;
using VirtoCommerce.CoreModule.Data.Model;
using VirtoCommerce.CoreModule.Data.Repositories;
using VirtoCommerce.CoreModule.Data.Services;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Commerce.Services;
using Xunit;
using Catalog = VirtoCommerce.CatalogModule.Data.Model.Catalog;
using Category = VirtoCommerce.CatalogModule.Data.Model.Category;

namespace VirtoCommerce.CatalogModule.Test
{
    [Trait("Category", "CI")]
    public class OutlineTests
    {
        #region Mocks

        private static readonly List<Catalog> _catalogs = new List<Catalog>
        {
            new Catalog { Id = "c" },
            new Catalog { Id = "v", Virtual = true },
        };

        private static readonly List<Category> _categories = new List<Category>
        {
            new Category { CatalogId = "c", Id = "c0", },
            new Category { CatalogId = "c", Id = "c1", },
            new Category { CatalogId = "c", Id = "c2", ParentCategoryId = "c1", },
            new Category { CatalogId = "c", Id = "c3", ParentCategoryId = "c2", },

            new Category { CatalogId = "v", Id = "v1", },
            new Category { CatalogId = "v", Id = "v2", ParentCategoryId = "v1", },
        };

        private static readonly List<Item> _items = new List<Item>
        {
            new Item { CatalogId = "c", Id = "p0", },
            new Item { CatalogId = "c", Id = "p1", CategoryId = "c3", },
        };

        private static readonly List<CategoryRelation> _categoryRelations = new List<CategoryRelation>
        {
            new CategoryRelation { SourceCategoryId = "c1", TargetCatalogId  = "v", TargetCategoryId = "v2" },
            new CategoryRelation { SourceCategoryId = "c2", TargetCatalogId  = "v", TargetCategoryId = "v2" },
            new CategoryRelation { SourceCategoryId = "c3", TargetCatalogId  = "v" },
            new CategoryRelation { SourceCategoryId = "c3", TargetCatalogId  = "v", TargetCategoryId = "v2" },
        };

        private static readonly List<CategoryItemRelation> _categoryItemRelations = new List<CategoryItemRelation>
        {
            new CategoryItemRelation { ItemId = "p1", CatalogId  = "v" },
            new CategoryItemRelation { ItemId = "p1", CatalogId  = "v", CategoryId = "v2" },
        };

        private static readonly List<SeoUrlKeyword> _seo = new List<SeoUrlKeyword>
        {
            new SeoUrlKeyword { ObjectType = "Category", ObjectId = "v1" },
            new SeoUrlKeyword { ObjectType = "Category", ObjectId = "v2" },
            new SeoUrlKeyword { ObjectType = "Category", ObjectId = "c0" },
            new SeoUrlKeyword { ObjectType = "Category", ObjectId = "c1" },
            new SeoUrlKeyword { ObjectType = "Category", ObjectId = "c2" },
            new SeoUrlKeyword { ObjectType = "Category", ObjectId = "c3" },
            new SeoUrlKeyword { ObjectType = "CatalogProduct", ObjectId = "p0" },
            new SeoUrlKeyword { ObjectType = "CatalogProduct", ObjectId = "p1" },
        };

        #endregion

        [Fact]
        public void GetCategories_When_NoParentsAndNoLinks_Expect_SinglePhysicalOutline()
        {
            var service = GetCategoryService();
            var result = service.GetByIds(new[] { "c0" }, CategoryResponseGroup.WithOutlines | CategoryResponseGroup.WithSeo);

            Assert.NotNull(result);
            Assert.Equal(1, result.Length);

            var category = result.First();
            Assert.NotNull(category);
            Assert.Equal("c0", category.Id);
            Assert.NotNull(category.Outlines);
            Assert.Equal(1, category.Outlines.Count);

            var outline = category.Outlines.First();
            Assert.NotNull(outline.Items);
            Assert.Equal(2, outline.Items.Count);

            var outlineString = outline.ToString();
            Assert.Equal("c/c0", outlineString);

            var allOutlineItems = category.Outlines.SelectMany(o => o.Items.Where(i => i.SeoObjectType != "Catalog")).ToList();
            Assert.True(allOutlineItems.All(i => i.SeoInfos != null && i.SeoInfos.Any()));
        }

        [Fact]
        public void GetCategories_When_PhysicalCatalog_Expect_SinglePhysicalOutline()
        {
            var service = GetCategoryService();
            var result = service.GetByIds(new[] { "c3" }, CategoryResponseGroup.WithOutlines | CategoryResponseGroup.WithSeo, "c");

            Assert.NotNull(result);
            Assert.Equal(1, result.Length);

            var category = result.First();
            Assert.NotNull(category);
            Assert.Equal("c3", category.Id);
            Assert.NotNull(category.Outlines);
            Assert.Equal(1, category.Outlines.Count);

            var outline = category.Outlines.First();
            Assert.NotNull(outline.Items);
            Assert.Equal(4, outline.Items.Count);

            var outlineString = outline.ToString();
            Assert.Equal("c/c1/c2/c3", outlineString);

            var allOutlineItems = category.Outlines.SelectMany(o => o.Items.Where(i => i.SeoObjectType != "Catalog")).ToList();
            Assert.True(allOutlineItems.All(i => i.SeoInfos != null && i.SeoInfos.Any()));
        }

        [Fact]
        public void GetCategories_When_VirtualCatalog_Expect_MultipleVirtualOutlines()
        {
            var service = GetCategoryService();
            var result = service.GetByIds(new[] { "c3" }, CategoryResponseGroup.WithOutlines | CategoryResponseGroup.WithSeo, "v");

            Assert.NotNull(result);
            Assert.Equal(1, result.Length);

            var category = result.First();
            Assert.NotNull(category);
            Assert.Equal("c3", category.Id);
            Assert.NotNull(category.Outlines);
            Assert.Equal(4, category.Outlines.Count);

            var outlineStrings = category.Outlines.Select(o => o.ToString()).ToList();
            Assert.True(outlineStrings.Contains("v/v1/v2/*c1/c2/c3"));
            Assert.True(outlineStrings.Contains("v/v1/v2/*c2/c3"));
            Assert.True(outlineStrings.Contains("v/v1/v2/*c3"));
            Assert.True(outlineStrings.Contains("v/*c3"));

            var allOutlineItems = category.Outlines.SelectMany(o => o.Items.Where(i => i.SeoObjectType != "Catalog")).ToList();
            Assert.True(allOutlineItems.All(i => i.SeoInfos != null && i.SeoInfos.Any()));
        }

        [Fact]
        public void GetCategories_When_NoCatalog_Expect_PhysicalAndVirtualOutlines()
        {
            var service = GetCategoryService();
            var result = service.GetByIds(new[] { "c3" }, CategoryResponseGroup.WithOutlines | CategoryResponseGroup.WithSeo);

            Assert.NotNull(result);
            Assert.Equal(1, result.Length);

            var category = result.First();
            Assert.NotNull(category);
            Assert.Equal("c3", category.Id);
            Assert.NotNull(category.Outlines);
            Assert.Equal(5, category.Outlines.Count);

            var outlineStrings = category.Outlines.Select(o => o.ToString()).ToList();
            Assert.True(outlineStrings.Contains("c/c1/c2/c3"));
            Assert.True(outlineStrings.Contains("v/v1/v2/*c1/c2/c3"));
            Assert.True(outlineStrings.Contains("v/v1/v2/*c2/c3"));
            Assert.True(outlineStrings.Contains("v/v1/v2/*c3"));
            Assert.True(outlineStrings.Contains("v/*c3"));

            var allOutlineItems = category.Outlines.SelectMany(o => o.Items.Where(i => i.SeoObjectType != "Catalog")).ToList();
            Assert.True(allOutlineItems.All(i => i.SeoInfos != null && i.SeoInfos.Any()));
        }

        [Fact]
        public void GetProducts_When_NoCategoriesAndNoLinks_Expect_SinglePhysicalOutline()
        {
            var service = GetItemService();
            var result = service.GetByIds(new[] { "p0" }, ItemResponseGroup.Outlines | ItemResponseGroup.Seo);

            Assert.NotNull(result);
            Assert.Equal(1, result.Length);

            var product = result.First();
            Assert.NotNull(product);
            Assert.Equal("p0", product.Id);
            Assert.NotNull(product.Outlines);
            Assert.Equal(1, product.Outlines.Count);

            var outline = product.Outlines.First();
            Assert.NotNull(outline.Items);
            Assert.Equal(2, outline.Items.Count);

            var outlineString = outline.ToString();
            Assert.Equal("c/p0", outlineString);

            var allOutlineItems = product.Outlines.SelectMany(o => o.Items.Where(i => i.SeoObjectType != "Catalog")).ToList();
            Assert.True(allOutlineItems.All(i => i.SeoInfos != null && i.SeoInfos.Any()));
        }

        [Fact]
        public void GetProducts_When_PhysicalCatalog_Expect_SinglePhysicalOutline()
        {
            var service = GetItemService();
            var result = service.GetByIds(new[] { "p1" }, ItemResponseGroup.Outlines | ItemResponseGroup.Seo, "c");

            Assert.NotNull(result);
            Assert.Equal(1, result.Length);

            var product = result.First();
            Assert.NotNull(product);
            Assert.Equal("p1", product.Id);
            Assert.NotNull(product.Outlines);
            Assert.Equal(1, product.Outlines.Count);

            var outline = product.Outlines.First();
            Assert.NotNull(outline.Items);
            Assert.Equal(5, outline.Items.Count);

            var outlineString = outline.ToString();
            Assert.Equal("c/c1/c2/c3/p1", outlineString);

            var allOutlineItems = product.Outlines.SelectMany(o => o.Items.Where(i => i.SeoObjectType != "Catalog")).ToList();
            Assert.True(allOutlineItems.All(i => i.SeoInfos != null && i.SeoInfos.Any()));
        }

        [Fact]
        public void GetProducts_When_VirtualCatalog_Expect_MultipleVirtualOutlines()
        {
            var service = GetItemService();
            var result = service.GetByIds(new[] { "p1" }, ItemResponseGroup.Outlines | ItemResponseGroup.Seo, "v");

            Assert.NotNull(result);
            Assert.Equal(1, result.Length);

            var product = result.First();
            Assert.NotNull(product);
            Assert.Equal("p1", product.Id);
            Assert.NotNull(product.Outlines);
            Assert.Equal(6, product.Outlines.Count);

            var outlineStrings = product.Outlines.Select(o => o.ToString()).ToList();
            Assert.True(outlineStrings.Contains("v/v1/v2/*c1/c2/c3/p1"));
            Assert.True(outlineStrings.Contains("v/v1/v2/*c2/c3/p1"));
            Assert.True(outlineStrings.Contains("v/v1/v2/*c3/p1"));
            Assert.True(outlineStrings.Contains("v/*c3/p1"));
            Assert.True(outlineStrings.Contains("v/v1/v2/*p1"));
            Assert.True(outlineStrings.Contains("v/*p1"));

            var allOutlineItems = product.Outlines.SelectMany(o => o.Items.Where(i => i.SeoObjectType != "Catalog")).ToList();
            Assert.True(allOutlineItems.All(i => i.SeoInfos != null && i.SeoInfos.Any()));
        }

        [Fact]
        public void GetProducts_When_NoCatalog_Expect_PhysicalAndVirtualOutlines()
        {
            var service = GetItemService();
            var result = service.GetByIds(new[] { "p1" }, ItemResponseGroup.Outlines | ItemResponseGroup.Seo);

            Assert.NotNull(result);
            Assert.Equal(1, result.Length);

            var product = result.First();
            Assert.NotNull(product);
            Assert.Equal("p1", product.Id);
            Assert.NotNull(product.Outlines);
            Assert.Equal(7, product.Outlines.Count);

            var outlineStrings = product.Outlines.Select(o => o.ToString()).ToList();
            Assert.True(outlineStrings.Contains("c/c1/c2/c3/p1"));
            Assert.True(outlineStrings.Contains("v/v1/v2/*c1/c2/c3/p1"));
            Assert.True(outlineStrings.Contains("v/v1/v2/*c2/c3/p1"));
            Assert.True(outlineStrings.Contains("v/v1/v2/*c3/p1"));
            Assert.True(outlineStrings.Contains("v/*c3/p1"));
            Assert.True(outlineStrings.Contains("v/v1/v2/*p1"));
            Assert.True(outlineStrings.Contains("v/*p1"));

            var allOutlineItems = product.Outlines.SelectMany(o => o.Items.Where(i => i.SeoObjectType != "Catalog")).ToList();
            Assert.True(allOutlineItems.All(i => i.SeoInfos != null && i.SeoInfos.Any()));
        }


        private static IOutlineService GetOutlineService()
        {
            return new OutlineService(GetCatalogRepository);
        }

        private static ICategoryService GetCategoryService()
        {
            return new CategoryServiceImpl(GetCatalogRepository, GetCommerceService(), GetOutlineService());
        }

        private static IItemService GetItemService()
        {
            return new ItemServiceImpl(GetCatalogRepository, GetCommerceService(), GetOutlineService());
        }

        private static ICommerceService GetCommerceService()
        {
            return new CommerceServiceImpl(GetCommerceRepository);
        }

        private static IСommerceRepository GetCommerceRepository()
        {
            var mock = new Mock<IСommerceRepository>();

            mock.SetupGet(r => r.SeoUrlKeywords).Returns(_seo.AsQueryable());

            return mock.Object;
        }

        private static ICatalogRepository GetCatalogRepository()
        {
            var mock = new Mock<ICatalogRepository>();

            mock
                .Setup(r => r.GetCategoriesByIds(It.IsAny<string[]>(), It.IsAny<CategoryResponseGroup>()))
                .Returns<string[], CategoryResponseGroup>(GetCategoriesByIds);

            mock
                .Setup(r => r.GetItemByIds(It.IsAny<string[]>(), It.IsAny<ItemResponseGroup>()))
                .Returns<string[], ItemResponseGroup>(GetItemsByIds);

            return mock.Object;
        }

        private static Category[] GetCategoriesByIds(string[] categoryIds, CategoryResponseGroup responseGroup)
        {
            var result = _categories
                .Where(c => categoryIds.Contains(c.Id))
                .Select(CloneCategory)
                .ToArray();

            var withLinks = (responseGroup & CategoryResponseGroup.WithLinks) == CategoryResponseGroup.WithLinks;
            var withParents = (responseGroup & CategoryResponseGroup.WithParents) == CategoryResponseGroup.WithParents;

            foreach (var category in result)
            {
                if (withLinks)
                {
                    category.OutgoingLinks = new ObservableCollection<CategoryRelation>(_categoryRelations.Where(r => r.SourceCategoryId == category.Id));
                }

                if (withParents)
                {
                    var parents = new List<Category>();

                    var currentCategory = category;
                    while (currentCategory != null && currentCategory.ParentCategoryId != null)
                    {
                        currentCategory = CloneCategory(_categories.FirstOrDefault(c => c.Id == currentCategory.ParentCategoryId));
                        parents.Insert(0, currentCategory);
                    }

                    category.AllParents = parents.ToArray();
                }
            }

            return result;
        }

        private static Category CloneCategory(Category category)
        {
            return new Category
            {
                Id = category.Id,
                ParentCategoryId = category.ParentCategoryId,
                CatalogId = category.CatalogId,
                Catalog = _catalogs.FirstOrDefault(c => c.Id == category.CatalogId),
            };
        }

        private static Item[] GetItemsByIds(string[] itemIds, ItemResponseGroup responseGroup)
        {
            var result = _items.Where(c => itemIds.Contains(c.Id)).ToArray();

            var withLinks = (responseGroup & ItemResponseGroup.Links) == ItemResponseGroup.Links;

            foreach (var item in result)
            {
                item.Catalog = _catalogs.FirstOrDefault(c => c.Id == item.CatalogId);

                if (withLinks)
                {
                    item.CategoryLinks = new ObservableCollection<CategoryItemRelation>(_categoryItemRelations.Where(r => r.ItemId == item.Id));
                }
            }

            return result;
        }
    }
}
