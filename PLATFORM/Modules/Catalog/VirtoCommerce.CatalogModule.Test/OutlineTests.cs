using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtoCommerce.CatalogModule.Data.Repositories;
using VirtoCommerce.CatalogModule.Data.Services;
using VirtoCommerce.CoreModule.Data.Repositories;
using VirtoCommerce.CoreModule.Data.Services;
using VirtoCommerce.Domain.Catalog.Model;
using VirtoCommerce.Domain.Catalog.Services;
using VirtoCommerce.Domain.Commerce.Services;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.CatalogModule.Test
{
    [TestClass]
    public class OutlineTests
    {
        [TestMethod]
        public void SearchCategories_When_PhysicalCatalog_Expect_SinglePhysicalOutline()
        {
            var service = GetCategoryService();
            var result = service.GetByIds(new[] { "fe4d82e57c20435c8a91b6ac75dc557f" }, CategoryResponseGroup.WithOutlines | CategoryResponseGroup.WithSeo, "b61aa9d1d0024bc4be12d79bf5786e9f");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);

            var category = result.First();
            Assert.IsNotNull(category);
            Assert.AreEqual("Summer-Dresses", category.Code);
            Assert.AreEqual("fe4d82e57c20435c8a91b6ac75dc557f", category.Id);
            Assert.IsNotNull(category.Outlines);
            Assert.AreEqual(1, category.Outlines.Count);

            var outline = category.Outlines.First();
            Assert.IsNotNull(outline.Items);
            Assert.AreEqual(4, outline.Items.Count);

            var outlineString = string.Join("/", outline.Items.Select(c => c.Id));
            Assert.AreEqual("b61aa9d1d0024bc4be12d79bf5786e9f/7c51d90394f145d0859810e38a48a41e/33505982c1134ff5b10c7a461f10a4cb/fe4d82e57c20435c8a91b6ac75dc557f", outlineString);

            var allOutlineItems = category.Outlines.SelectMany(o => o.Items.Where(i => i.SeoObjectType != "Catalog")).ToList();
            Assert.IsTrue(allOutlineItems.All(i => i.SeoInfos != null && i.SeoInfos.Any()));
        }

        [TestMethod]
        public void SearchCategories_When_VirtualCatalog_Expect_MultipleVirtualOutlines()
        {
            var service = GetCategoryService();
            var result = service.GetByIds(new[] { "fe4d82e57c20435c8a91b6ac75dc557f" }, CategoryResponseGroup.WithOutlines | CategoryResponseGroup.WithSeo, "25f5ea1b52e54ec1aa903d44cc889324");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);

            var category = result.First();
            Assert.IsNotNull(category);
            Assert.AreEqual("Summer-Dresses", category.Code);
            Assert.AreEqual("fe4d82e57c20435c8a91b6ac75dc557f", category.Id);
            Assert.IsNotNull(category.Outlines);
            Assert.AreEqual(4, category.Outlines.Count);

            var outlineStrings = category.Outlines.Select(o => string.Join("/", o.Items.Select(c => c.Id))).ToList();
            Assert.IsTrue(outlineStrings.Contains("25f5ea1b52e54ec1aa903d44cc889324/87fcc2308e46478192d6a31b7f505580/7c51d90394f145d0859810e38a48a41e/33505982c1134ff5b10c7a461f10a4cb/fe4d82e57c20435c8a91b6ac75dc557f"));
            Assert.IsTrue(outlineStrings.Contains("25f5ea1b52e54ec1aa903d44cc889324/87fcc2308e46478192d6a31b7f505580/33505982c1134ff5b10c7a461f10a4cb/fe4d82e57c20435c8a91b6ac75dc557f"));
            Assert.IsTrue(outlineStrings.Contains("25f5ea1b52e54ec1aa903d44cc889324/87fcc2308e46478192d6a31b7f505580/fe4d82e57c20435c8a91b6ac75dc557f"));
            Assert.IsTrue(outlineStrings.Contains("25f5ea1b52e54ec1aa903d44cc889324/fe4d82e57c20435c8a91b6ac75dc557f"));

            var allOutlineItems = category.Outlines.SelectMany(o => o.Items.Where(i => i.SeoObjectType != "Catalog")).ToList();
            Assert.IsTrue(allOutlineItems.All(i => i.SeoInfos != null && i.SeoInfos.Any()));
        }

        [TestMethod]
        public void SearchProducts_When_PhysicalCatalog_Expect_SinglePhysicalOutline()
        {
            var service = GetItemService();
            var result = service.GetByIds(new[] { "6b9797660c28496faef5fbfae871d735" }, ItemResponseGroup.Outlines | ItemResponseGroup.Seo, "b61aa9d1d0024bc4be12d79bf5786e9f");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);

            var product = result.First();
            Assert.IsNotNull(product);
            Assert.AreEqual("355789384", product.Code);
            Assert.AreEqual("6b9797660c28496faef5fbfae871d735", product.Id);
            Assert.IsNotNull(product.Outlines);
            Assert.AreEqual(1, product.Outlines.Count);

            var outline = product.Outlines.First();
            Assert.IsNotNull(outline.Items);
            Assert.AreEqual(5, outline.Items.Count);

            var outlineString = string.Join("/", outline.Items.Select(c => c.Id));
            Assert.AreEqual("b61aa9d1d0024bc4be12d79bf5786e9f/7c51d90394f145d0859810e38a48a41e/33505982c1134ff5b10c7a461f10a4cb/fe4d82e57c20435c8a91b6ac75dc557f/6b9797660c28496faef5fbfae871d735", outlineString);

            var allOutlineItems = product.Outlines.SelectMany(o => o.Items.Where(i => i.SeoObjectType != "Catalog")).ToList();
            Assert.IsTrue(allOutlineItems.All(i => i.SeoInfos != null && i.SeoInfos.Any()));
        }

        [TestMethod]
        public void SearchProducts_When_VirtualCatalog_Expect_MultipleVirtualOutlines()
        {
            var service = GetItemService();
            var result = service.GetByIds(new[] { "6b9797660c28496faef5fbfae871d735" }, ItemResponseGroup.Outlines | ItemResponseGroup.Seo, "25f5ea1b52e54ec1aa903d44cc889324");

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Length);

            var product = result.First();
            Assert.IsNotNull(product);
            Assert.AreEqual("355789384", product.Code);
            Assert.AreEqual("6b9797660c28496faef5fbfae871d735", product.Id);
            Assert.IsNotNull(product.Outlines);
            Assert.AreEqual(6, product.Outlines.Count);

            var outlineStrings = product.Outlines.Select(o => string.Join("/", o.Items.Select(c => c.Id))).ToList();
            Assert.IsTrue(outlineStrings.Contains("25f5ea1b52e54ec1aa903d44cc889324/87fcc2308e46478192d6a31b7f505580/7c51d90394f145d0859810e38a48a41e/33505982c1134ff5b10c7a461f10a4cb/fe4d82e57c20435c8a91b6ac75dc557f/6b9797660c28496faef5fbfae871d735"));
            Assert.IsTrue(outlineStrings.Contains("25f5ea1b52e54ec1aa903d44cc889324/87fcc2308e46478192d6a31b7f505580/33505982c1134ff5b10c7a461f10a4cb/fe4d82e57c20435c8a91b6ac75dc557f/6b9797660c28496faef5fbfae871d735"));
            Assert.IsTrue(outlineStrings.Contains("25f5ea1b52e54ec1aa903d44cc889324/87fcc2308e46478192d6a31b7f505580/fe4d82e57c20435c8a91b6ac75dc557f/6b9797660c28496faef5fbfae871d735"));
            Assert.IsTrue(outlineStrings.Contains("25f5ea1b52e54ec1aa903d44cc889324/fe4d82e57c20435c8a91b6ac75dc557f/6b9797660c28496faef5fbfae871d735"));
            Assert.IsTrue(outlineStrings.Contains("25f5ea1b52e54ec1aa903d44cc889324/87fcc2308e46478192d6a31b7f505580/6b9797660c28496faef5fbfae871d735"));
            Assert.IsTrue(outlineStrings.Contains("25f5ea1b52e54ec1aa903d44cc889324/6b9797660c28496faef5fbfae871d735"));

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
            var retVal = new CommerceRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor(null));
            return retVal;
        }

        private static ICatalogRepository GetCatalogRepository()
        {
            var retVal = new CatalogRepositoryImpl("VirtoCommerce", new EntityPrimaryKeyGeneratorInterceptor(), new AuditableInterceptor(null));
            return retVal;
        }
    }
}
