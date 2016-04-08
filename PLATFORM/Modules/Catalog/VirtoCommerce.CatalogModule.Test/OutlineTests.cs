using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
using Catalog = VirtoCommerce.CatalogModule.Data.Model.Catalog;
using Category = VirtoCommerce.CatalogModule.Data.Model.Category;

namespace VirtoCommerce.CatalogModule.Test
{
    [TestClass]
    public class OutlineTests
    {
        #region Mocks

        private static readonly List<Category> _categories = new List<Category>
        {
            new Category
            {
                CatalogId = "v",
                Id = "v1",
                Catalog = new Catalog { Id = "v", Virtual = true },
            },

            new Category
            {
                CatalogId = "c",
                Id = "c1",
                Catalog = new Catalog { Id = "c" },
                OutgoingLinks = new ObservableCollection<CategoryRelation>
                {
                    new CategoryRelation { TargetCatalogId  = "v", TargetCategoryId = "v1" },
                },
            },
            new Category
            {
                CatalogId = "c",
                Id = "c2",
                ParentCategoryId = "c1",
                Catalog = new Catalog { Id = "c" },
                OutgoingLinks = new ObservableCollection<CategoryRelation>
                {
                    new CategoryRelation { TargetCatalogId  = "v", TargetCategoryId = "v1" },
                },
            },
            new Category
            {
                CatalogId = "c",
                Id = "c3",
                ParentCategoryId = "c2",
                Catalog = new Catalog { Id = "c" },
                OutgoingLinks = new ObservableCollection<CategoryRelation>
                {
                    new CategoryRelation { TargetCatalogId  = "v" },
                    new CategoryRelation { TargetCatalogId  = "v", TargetCategoryId = "v1" },
                },
            },
        };

        private static readonly List<Item> _items = new List<Item>
        {
            new Item
            {
                CatalogId = "c",
                CategoryId = "c3",
                Id = "p1",
                Catalog = new Catalog { Id = "c" },
                CategoryLinks = new ObservableCollection<CategoryItemRelation>
                {
                    new CategoryItemRelation { CatalogId  = "v" },
                    new CategoryItemRelation { CatalogId  = "v", CategoryId = "v1" },
                },
            },
        };

        private static readonly List<SeoUrlKeyword> _seo = new List<SeoUrlKeyword>
        {
            new SeoUrlKeyword { ObjectType = "Category", ObjectId = "v1" },
            new SeoUrlKeyword { ObjectType = "Category", ObjectId = "c1" },
            new SeoUrlKeyword { ObjectType = "Category", ObjectId = "c2" },
            new SeoUrlKeyword { ObjectType = "Category", ObjectId = "c3" },
            new SeoUrlKeyword { ObjectType = "CatalogProduct", ObjectId = "p1" },
        };

        #endregion

        [TestMethod]
        public void SearchCategories_When_PhysicalCatalog_Expect_SinglePhysicalOutline()
        {
            var service = GetCategoryService();
            var result = service.GetByIds(new[] { "c3" }, CategoryResponseGroup.WithOutlines | CategoryResponseGroup.WithSeo, "c");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);

            var category = result.First();
            Assert.IsNotNull(category);
            Assert.AreEqual("c3", category.Id);
            Assert.IsNotNull(category.Outlines);
            Assert.AreEqual(1, category.Outlines.Count);

            var outline = category.Outlines.First();
            Assert.IsNotNull(outline.Items);
            Assert.AreEqual(4, outline.Items.Count);

            var outlineString = string.Join("/", outline.Items.Select(c => c.Id));
            Assert.AreEqual("c/c1/c2/c3", outlineString);

            var allOutlineItems = category.Outlines.SelectMany(o => o.Items.Where(i => i.SeoObjectType != "Catalog")).ToList();
            Assert.IsTrue(allOutlineItems.All(i => i.SeoInfos != null && i.SeoInfos.Any()));
        }

        [TestMethod]
        public void SearchCategories_When_VirtualCatalog_Expect_MultipleVirtualOutlines()
        {
            var service = GetCategoryService();
            var result = service.GetByIds(new[] { "c3" }, CategoryResponseGroup.WithOutlines | CategoryResponseGroup.WithSeo, "v");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);

            var category = result.First();
            Assert.IsNotNull(category);
            Assert.AreEqual("c3", category.Id);
            Assert.IsNotNull(category.Outlines);
            Assert.AreEqual(4, category.Outlines.Count);

            var outlineStrings = category.Outlines.Select(o => string.Join("/", o.Items.Select(c => c.Id))).ToList();
            Assert.IsTrue(outlineStrings.Contains("v/v1/c1/c2/c3"));
            Assert.IsTrue(outlineStrings.Contains("v/v1/c2/c3"));
            Assert.IsTrue(outlineStrings.Contains("v/v1/c3"));
            Assert.IsTrue(outlineStrings.Contains("v/c3"));

            var allOutlineItems = category.Outlines.SelectMany(o => o.Items.Where(i => i.SeoObjectType != "Catalog")).ToList();
            Assert.IsTrue(allOutlineItems.All(i => i.SeoInfos != null && i.SeoInfos.Any()));
        }

        [TestMethod]
        public void SearchProducts_When_PhysicalCatalog_Expect_SinglePhysicalOutline()
        {
            var service = GetItemService();
            var result = service.GetByIds(new[] { "p1" }, ItemResponseGroup.Outlines | ItemResponseGroup.Seo, "c");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);

            var product = result.First();
            Assert.IsNotNull(product);
            Assert.AreEqual("p1", product.Id);
            Assert.IsNotNull(product.Outlines);
            Assert.AreEqual(1, product.Outlines.Count);

            var outline = product.Outlines.First();
            Assert.IsNotNull(outline.Items);
            Assert.AreEqual(5, outline.Items.Count);

            var outlineString = string.Join("/", outline.Items.Select(c => c.Id));
            Assert.AreEqual("c/c1/c2/c3/p1", outlineString);

            var allOutlineItems = product.Outlines.SelectMany(o => o.Items.Where(i => i.SeoObjectType != "Catalog")).ToList();
            Assert.IsTrue(allOutlineItems.All(i => i.SeoInfos != null && i.SeoInfos.Any()));
        }

        [TestMethod]
        public void SearchProducts_When_VirtualCatalog_Expect_MultipleVirtualOutlines()
        {
            var service = GetItemService();
            var result = service.GetByIds(new[] { "p1" }, ItemResponseGroup.Outlines | ItemResponseGroup.Seo, "v");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);

            var product = result.First();
            Assert.IsNotNull(product);
            Assert.AreEqual("p1", product.Id);
            Assert.IsNotNull(product.Outlines);
            Assert.AreEqual(6, product.Outlines.Count);

            var outlineStrings = product.Outlines.Select(o => string.Join("/", o.Items.Select(c => c.Id))).ToList();
            Assert.IsTrue(outlineStrings.Contains("v/v1/c1/c2/c3/p1"));
            Assert.IsTrue(outlineStrings.Contains("v/v1/c2/c3/p1"));
            Assert.IsTrue(outlineStrings.Contains("v/v1/c3/p1"));
            Assert.IsTrue(outlineStrings.Contains("v/c3/p1"));
            Assert.IsTrue(outlineStrings.Contains("v/v1/p1"));
            Assert.IsTrue(outlineStrings.Contains("v/p1"));

            var allOutlineItems = product.Outlines.SelectMany(o => o.Items.Where(i => i.SeoObjectType != "Catalog")).ToList();
            Assert.IsTrue(allOutlineItems.All(i => i.SeoInfos != null && i.SeoInfos.Any()));
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
            return _categories.Where(c => categoryIds.Contains(c.Id)).ToArray();
        }

        private static Item[] GetItemsByIds(string[] itemIds, ItemResponseGroup responseGroup)
        {
            return _items.Where(c => itemIds.Contains(c.Id)).ToArray();
        }
    }
}
