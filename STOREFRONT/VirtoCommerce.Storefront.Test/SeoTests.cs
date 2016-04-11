using System.Collections.Generic;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Model;
using Xunit;

namespace VirtoCommerce.Storefront.Test
{
    [Trait("Category", "CI")]
    public class CategorySeoTests
    {
        private readonly Store _store = new Store
        {
            Id = "Store1",
            DefaultLanguage = new Language("en-US"),
            Languages = new List<Language>(new[]
            {
                new Language("en-US"),
            }),
            SeoLinksType = SeoLinksType.Long,
        };

        [Fact]
        public void When_HasNoSeoRecords_Expect_Null()
        {
            var category = new VirtoCommerceCatalogModuleWebModelCategory();

            var result = category.Outlines.GetSeoPath(_store, new Language("en-US"), null);
            Assert.Null(result);
        }

        [Fact]
        public void When_HasSeoRecords_Expect_ShortPath()
        {
            var category = new VirtoCommerceCatalogModuleWebModelCategory
            {
                Outlines = new List<VirtoCommerceDomainCatalogModelOutline>
                {
                    new VirtoCommerceDomainCatalogModelOutline
                    {
                        Items = new List<VirtoCommerceDomainCatalogModelOutlineItem>
                        {
                            new VirtoCommerceDomainCatalogModelOutlineItem
                            {
                                SeoObjectType = "Catalog",
                            },
                            new VirtoCommerceDomainCatalogModelOutlineItem
                            {
                                SeoObjectType = "Category",
                                SeoInfos = new List<VirtoCommerceDomainCommerceModelSeoInfo>
                                {
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "en-US", SemanticUrl = "category1" },
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "ru-RU", SemanticUrl = "category2" },
                                },
                            }
                        },
                    },
                },
            };

            var result = category.Outlines.GetSeoPath(_store, new Language("ru-RU"), null);
            Assert.Equal("category2", result);
        }

        [Fact]
        public void When_HasParentSeoRecords_Expect_LongPath()
        {
            var category = new VirtoCommerceCatalogModuleWebModelCategory
            {
                Outlines = new List<VirtoCommerceDomainCatalogModelOutline>
                {
                    new VirtoCommerceDomainCatalogModelOutline
                    {
                        Items = new List<VirtoCommerceDomainCatalogModelOutlineItem>
                        {
                            new VirtoCommerceDomainCatalogModelOutlineItem
                            {
                                SeoObjectType = "Catalog",
                            },
                            new VirtoCommerceDomainCatalogModelOutlineItem
                            {
                                SeoObjectType = "Category",
                                SeoInfos = new List<VirtoCommerceDomainCommerceModelSeoInfo>
                                {
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "en-US", SemanticUrl = "grandparent1" },
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "ru-RU", SemanticUrl = "grandparent2" },
                                }
                            },
                            new VirtoCommerceDomainCatalogModelOutlineItem
                            {
                                SeoObjectType = "Category",
                                SeoInfos = new List<VirtoCommerceDomainCommerceModelSeoInfo>
                                {
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "en-US", SemanticUrl = "parent1" },
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "ru-RU", SemanticUrl = "parent2" },
                                }
                            },
                            new VirtoCommerceDomainCatalogModelOutlineItem
                            {
                                SeoObjectType = "Category",
                                SeoInfos = new List<VirtoCommerceDomainCommerceModelSeoInfo>
                                {
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "en-US", SemanticUrl = "category1" },
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "ru-RU", SemanticUrl = "category2" },
                                },
                            },
                        },
                    },
                },
            };

            var result = category.Outlines.GetSeoPath(_store, new Language("ru-RU"), null);
            Assert.Equal("grandparent2/parent2/category2", result);
        }

        [Fact]
        public void When_MissingAnyParentSeoRecord_Expect_Null()
        {
            var category = new VirtoCommerceCatalogModuleWebModelCategory
            {
                Outlines = new List<VirtoCommerceDomainCatalogModelOutline>
                {
                    new VirtoCommerceDomainCatalogModelOutline
                    {
                        Items = new List<VirtoCommerceDomainCatalogModelOutlineItem>
                        {
                            new VirtoCommerceDomainCatalogModelOutlineItem
                            {
                                SeoObjectType = "Catalog",
                            },
                            new VirtoCommerceDomainCatalogModelOutlineItem
                            {
                                SeoObjectType = "Category",
                                SeoInfos = new List<VirtoCommerceDomainCommerceModelSeoInfo>(),
                            },
                            new VirtoCommerceDomainCatalogModelOutlineItem
                            {
                                SeoObjectType = "Category",
                                SeoInfos = new List<VirtoCommerceDomainCommerceModelSeoInfo>
                                {
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "en-US", SemanticUrl = "parent1" },
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "ru-RU", SemanticUrl = "parent2" },
                                }
                            },
                            new VirtoCommerceDomainCatalogModelOutlineItem
                            {
                                SeoObjectType = "Category",
                                SeoInfos = new List<VirtoCommerceDomainCommerceModelSeoInfo>
                                {
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "en-US", SemanticUrl = "category1" },
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "ru-RU", SemanticUrl = "category2" },
                                },
                            },
                        },
                    },
                },
            };

            var result = category.Outlines.GetSeoPath(_store, new Language("ru-RU"), null);
            Assert.Null(result);
        }
    }

    [Trait("Category", "CI")]
    public class ProductSeoTests
    {
        private readonly Store _store = new Store
        {
            Id = "Store1",
            DefaultLanguage = new Language("en-US"),
            Languages = new List<Language>(new[]
            {
                new Language("en-US"),
            }),
            SeoLinksType = SeoLinksType.Long,
        };

        [Fact]
        public void When_HasNoSeoRecords_Expect_Null()
        {
            var product = new VirtoCommerceCatalogModuleWebModelProduct
            {
                Category = new VirtoCommerceCatalogModuleWebModelCategory(),
            };

            var result = product.Outlines.GetSeoPath(_store, new Language("en-US"), null);
            Assert.Null(result);
        }

        [Fact]
        public void When_HasSeoRecordsAndNoCategorySeoRecords_Expect_Null()
        {
            var product = new VirtoCommerceCatalogModuleWebModelProduct
            {
                Outlines = new List<VirtoCommerceDomainCatalogModelOutline>
                {
                    new VirtoCommerceDomainCatalogModelOutline
                    {
                        Items = new List<VirtoCommerceDomainCatalogModelOutlineItem>
                        {
                            new VirtoCommerceDomainCatalogModelOutlineItem
                            {
                                SeoObjectType = "Catalog",
                            },
                            new VirtoCommerceDomainCatalogModelOutlineItem
                            {
                                SeoObjectType = "Category",
                            },
                            new VirtoCommerceDomainCatalogModelOutlineItem
                            {
                                SeoObjectType = "CatalogProduct",
                                SeoInfos = new List<VirtoCommerceDomainCommerceModelSeoInfo>
                                {
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "en-US", SemanticUrl = "product1" },
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "ru-RU", SemanticUrl = "product2" },
                                },
                            },
                        },
                    },
                },
            };

            var result = product.Outlines.GetSeoPath(_store, new Language("ru-RU"), null);
            Assert.Null(result);
        }

        [Fact]
        public void When_HasCategorySeoRecords_Expect_LongPath()
        {
            var product = new VirtoCommerceCatalogModuleWebModelProduct
            {
                Outlines = new List<VirtoCommerceDomainCatalogModelOutline>
                {
                    new VirtoCommerceDomainCatalogModelOutline
                    {
                        Items = new List<VirtoCommerceDomainCatalogModelOutlineItem>
                        {
                            new VirtoCommerceDomainCatalogModelOutlineItem
                            {
                                SeoObjectType = "Catalog",
                            },
                            new VirtoCommerceDomainCatalogModelOutlineItem
                            {
                                SeoObjectType = "Category",
                                SeoInfos = new List<VirtoCommerceDomainCommerceModelSeoInfo>
                                {
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "en-US", SemanticUrl = "category1" },
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "ru-RU", SemanticUrl = "category2" },
                                },
                            },
                            new VirtoCommerceDomainCatalogModelOutlineItem
                            {
                                SeoObjectType = "CatalogProduct",
                                SeoInfos = new List<VirtoCommerceDomainCommerceModelSeoInfo>
                                {
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "en-US", SemanticUrl = "product1" },
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "ru-RU", SemanticUrl = "product2" },
                                },
                            },
                        },
                    },
                },
            };

            var result = product.Outlines.GetSeoPath(_store, new Language("ru-RU"), null);
            Assert.Equal("category2/product2", result);
        }

        [Fact]
        public void When_HasParentCategorySeoRecords_Expect_LongPath()
        {
            var product = new VirtoCommerceCatalogModuleWebModelProduct
            {
                Outlines = new List<VirtoCommerceDomainCatalogModelOutline>
                {
                    new VirtoCommerceDomainCatalogModelOutline
                    {
                        Items = new List<VirtoCommerceDomainCatalogModelOutlineItem>
                        {
                            new VirtoCommerceDomainCatalogModelOutlineItem
                            {
                                SeoObjectType = "Catalog",
                            },
                            new VirtoCommerceDomainCatalogModelOutlineItem
                            {
                                SeoObjectType = "Category",
                                SeoInfos = new List<VirtoCommerceDomainCommerceModelSeoInfo>
                                {
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "en-US", SemanticUrl = "grandparent1" },
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "ru-RU", SemanticUrl = "grandparent2" },
                                }
                            },
                            new VirtoCommerceDomainCatalogModelOutlineItem
                            {
                                SeoObjectType = "Category",
                                SeoInfos = new List<VirtoCommerceDomainCommerceModelSeoInfo>
                                {
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "en-US", SemanticUrl = "parent1" },
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "ru-RU", SemanticUrl = "parent2" },
                                }
                            },
                            new VirtoCommerceDomainCatalogModelOutlineItem
                            {
                                SeoObjectType = "Category",
                                SeoInfos = new List<VirtoCommerceDomainCommerceModelSeoInfo>
                                {
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "en-US", SemanticUrl = "category1" },
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "ru-RU", SemanticUrl = "category2" },
                                },
                            },
                            new VirtoCommerceDomainCatalogModelOutlineItem
                            {
                                SeoObjectType = "CatalogProduct",
                                SeoInfos = new List<VirtoCommerceDomainCommerceModelSeoInfo>
                                {
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "en-US", SemanticUrl = "product1" },
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "ru-RU", SemanticUrl = "product2" },
                                },
                            },
                        },
                    },
                },
            };

            var result = product.Outlines.GetSeoPath(_store, new Language("ru-RU"), null);
            Assert.Equal("grandparent2/parent2/category2/product2", result);
        }

        [Fact]
        public void When_MissingAnyParentSeoRecord_Expect_Null()
        {
            var product = new VirtoCommerceCatalogModuleWebModelProduct
            {
                Outlines = new List<VirtoCommerceDomainCatalogModelOutline>
                {
                    new VirtoCommerceDomainCatalogModelOutline
                    {
                        Items = new List<VirtoCommerceDomainCatalogModelOutlineItem>
                        {
                            new VirtoCommerceDomainCatalogModelOutlineItem
                            {
                                SeoObjectType = "Catalog",
                            },
                            new VirtoCommerceDomainCatalogModelOutlineItem
                            {
                                SeoObjectType = "Category",
                                SeoInfos = new List<VirtoCommerceDomainCommerceModelSeoInfo>(),
                            },
                            new VirtoCommerceDomainCatalogModelOutlineItem
                            {
                                SeoObjectType = "Category",
                                SeoInfos = new List<VirtoCommerceDomainCommerceModelSeoInfo>
                                {
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "en-US", SemanticUrl = "parent1" },
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "ru-RU", SemanticUrl = "parent2" },
                                }
                            },
                            new VirtoCommerceDomainCatalogModelOutlineItem
                            {
                                SeoObjectType = "Category",
                                SeoInfos = new List<VirtoCommerceDomainCommerceModelSeoInfo>
                                {
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "en-US", SemanticUrl = "category1" },
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "ru-RU", SemanticUrl = "category2" },
                                },
                            },
                            new VirtoCommerceDomainCatalogModelOutlineItem
                            {
                                SeoObjectType = "Category",
                                SeoInfos = new List<VirtoCommerceDomainCommerceModelSeoInfo>
                                {
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "en-US", SemanticUrl = "product1" },
                                    new VirtoCommerceDomainCommerceModelSeoInfo { StoreId = "Store1", LanguageCode = "ru-RU", SemanticUrl = "product2" },
                                },
                            },
                        },
                    },
                },
            };

            var result = product.Outlines.GetSeoPath(_store, new Language("ru-RU"), null);
            Assert.Null(result);
        }
    }
}
