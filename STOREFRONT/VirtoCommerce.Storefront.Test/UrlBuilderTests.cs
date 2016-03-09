using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using Xunit;

namespace VirtoCommerce.Storefront.Test
{
    public class UrlBuilderSingleStoreSingleLanguage
    {
        private readonly WorkContext _workContext;
        private readonly IStorefrontUrlBuilder _builder;

        public UrlBuilderSingleStoreSingleLanguage()
        {
            _workContext = new WorkContext
            {
                RequestUrl = new Uri("http://localhost/path"),
                AllStores = new[]
                {
                    new Store
                    {
                        Id = "Store1",
                        DefaultLanguage = new Language("en-US"),
                        Languages = new List<Language>(new[]
                        {
                            new Language("en-US"),
                        }),
                    },
                }
            };

            _workContext.CurrentStore = _workContext.AllStores.First();
            _workContext.CurrentLanguage = _workContext.CurrentStore.Languages.First();

            _builder = new StorefrontUrlBuilder(_workContext);
        }

        [Fact]
        public void When_StoreIsNullAndLanguageIsNull_Expect_VirtualRoot()
        {
            var result = _builder.ToAppRelative("/", null, null);
            Assert.Equal("~/", result);
        }

        [Fact]
        public void When_StoreIsNullAndLanguageIsAny_Expect_VirtualRoot()
        {
            var result = _builder.ToAppRelative("/", null, new Language("en-US"));
            Assert.Equal("~/", result);
        }

        [Fact]
        public void When_CurrentStoreAndCurrentLanguage_Expect_VirtualRoot()
        {
            var result = _builder.ToAppRelative("/");
            Assert.Equal("~/", result);
        }

        [Fact]
        public void When_CurrentStoreAndUnknownLanguage_Expect_VirtualRoot()
        {
            var store = _workContext.CurrentStore;
            var language = new Language("ja-JP");

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("~/", result);
        }

        [Fact]
        public void When_CurrentStoreAndSelectedLanguage_Expect_VirtualRoot()
        {
            var store = _workContext.CurrentStore;
            var language = store.Languages.Last();

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("~/", result);
        }
    }

    public class UrlBuilderSingleStoreMultipleLanguages
    {
        private readonly WorkContext _workContext;
        private readonly IStorefrontUrlBuilder _builder;

        public UrlBuilderSingleStoreMultipleLanguages()
        {
            _workContext = new WorkContext
            {
                RequestUrl = new Uri("http://localhost/path"),
                AllStores = new[]
                {
                    new Store
                    {
                        Id = "Store1",
                        DefaultLanguage = new Language("ru-RU"),
                        Languages = new List<Language>(new[]
                        {
                            new Language("en-US"),
                            new Language("ru-RU"),
                            new Language("lt-LT"),
                        }),
                    },
                }
            };

            _workContext.CurrentStore = _workContext.AllStores.First();
            _workContext.CurrentLanguage = _workContext.CurrentStore.Languages.First();

            _builder = new StorefrontUrlBuilder(_workContext);
        }

        [Fact]
        public void When_StoreIsNullAndLanguageIsNull_Expect_VirtualRoot()
        {
            var result = _builder.ToAppRelative("/", null, null);
            Assert.Equal("~/", result);
        }

        [Fact]
        public void When_StoreIsNullAndLanguageIsAny_Expect_VirtualRoot()
        {
            var result = _builder.ToAppRelative("/", null, new Language("en-US"));
            Assert.Equal("~/", result);
        }

        [Fact]
        public void When_CurrentStoreAndCurrentLanguage_Expect_CurrentLanguage()
        {
            var result = _builder.ToAppRelative("/");
            Assert.Equal("~/en-US/", result);
        }

        [Fact]
        public void When_CurrentStoreAndUnknownLanguage_Expect_DefaultLanguage()
        {
            var store = _workContext.CurrentStore;
            var language = new Language("ja-JP");

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("~/ru-RU/", result);
        }

        [Fact]
        public void When_CurrentStoreAndSelectedLanguage_Expect_SelectedLanguage()
        {
            var store = _workContext.CurrentStore;
            var language = store.Languages.Last();

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("~/lt-LT/", result);
        }
    }

    public class UrlBuilderMultipleStoresMultipleLanguages
    {
        private readonly WorkContext _workContext;
        private readonly IStorefrontUrlBuilder _builder;

        public UrlBuilderMultipleStoresMultipleLanguages()
        {
            _workContext = new WorkContext
            {
                RequestUrl = new Uri("http://localhost/path"),
                AllStores = new[]
                {
                    new Store
                    {
                        Id = "Store1",
                        DefaultLanguage = new Language("ru-RU"),
                        Languages = new List<Language>(new[]
                        {
                            new Language("en-US"),
                            new Language("ru-RU"),
                            new Language("lt-LT"),
                        }),
                    },
                    new Store
                    {
                        Id = "Store2",
                        DefaultLanguage = new Language("es-ES"),
                        Languages = new List<Language>(new[]
                        {
                            new Language("de-DE"),
                            new Language("es-ES"),
                            new Language("fr-FR"),
                        }),
                    }
                }
            };

            _workContext.CurrentStore = _workContext.AllStores.First();
            _workContext.CurrentLanguage = _workContext.CurrentStore.Languages.First();

            _builder = new StorefrontUrlBuilder(_workContext);
        }

        [Fact]
        public void When_StoreIsNullAndLanguageIsNull_Expect_VirtualRoot()
        {
            var result = _builder.ToAppRelative("/", null, null);
            Assert.Equal("~/", result);
        }

        [Fact]
        public void When_StoreIsNullAndLanguageIsAny_Expect_VirtualRoot()
        {
            var result = _builder.ToAppRelative("/", null, new Language("en-US"));
            Assert.Equal("~/", result);
        }

        [Fact]
        public void When_CurrentStoreAndCurrentLanguage_Expect_CurrentStoreAndCurrentLanguage()
        {
            var result = _builder.ToAppRelative("/");
            Assert.Equal("~/Store1/en-US/", result);
        }

        [Fact]
        public void When_CurrentStoreAndUnknownLanguage_Expect_CurrentStoreAndDefaultLanguage()
        {
            var store = _workContext.CurrentStore;
            var language = new Language("ja-JP");

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("~/Store1/ru-RU/", result);
        }

        [Fact]
        public void When_CurrentStoreAndSelectedLanguage_Expect_CurrentStoreAndSelectedLanguage()
        {
            var store = _workContext.CurrentStore;
            var language = store.Languages.Last();

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("~/Store1/lt-LT/", result);
        }

        [Fact]
        public void When_SelectedStoreAndUnknownLanguage_Expect_SelectedStoreAndDefaultLanguage()
        {
            var store = _workContext.AllStores.Last();
            var language = new Language("ja-JP");

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("~/Store2/es-ES/", result);
        }

        [Fact]
        public void When_SelectedStoreAndSelectedLanguage_Expect_SelectedStoreAndSelectedLanguage()
        {
            var store = _workContext.AllStores.Last();
            var language = store.Languages.Last();

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("~/Store2/fr-FR/", result);
        }
    }

    public class UrlBuilderPublicStoreUrl
    {
        private readonly WorkContext _workContext;
        private readonly IStorefrontUrlBuilder _builder;

        public UrlBuilderPublicStoreUrl()
        {
            _workContext = new WorkContext
            {
                RequestUrl = new Uri("http://localhost/public/path"),
                AllStores = new[]
                {
                    new Store
                    {
                        Id = "Store1",
                        Url = "http://localhost/public",
                        SecureUrl = "https://localhost/secure",
                        DefaultLanguage = new Language("ru-RU"),
                        Languages = new List<Language>(new[]
                        {
                            new Language("en-US"),
                            new Language("ru-RU"),
                            new Language("lt-LT"),
                        }),
                    },
                    new Store
                    {
                        Id = "Store2",
                        DefaultLanguage = new Language("es-ES"),
                        Languages = new List<Language>(new[]
                        {
                            new Language("de-DE"),
                            new Language("es-ES"),
                            new Language("fr-FR"),
                        }),
                    }
                }
            };

            _workContext.CurrentStore = _workContext.AllStores.First();
            _workContext.CurrentLanguage = _workContext.CurrentStore.Languages.First();

            _builder = new StorefrontUrlBuilder(_workContext);
        }

        [Fact]
        public void When_StoreIsNullAndLanguageIsNull_Expect_VirtualRoot()
        {
            var result = _builder.ToAppRelative("/", null, null);
            Assert.Equal("~/", result);
        }

        [Fact]
        public void When_StoreIsNullAndLanguageIsAny_Expect_VirtualRoot()
        {
            var result = _builder.ToAppRelative("/", null, new Language("en-US"));
            Assert.Equal("~/", result);
        }

        [Fact]
        public void When_CurrentStoreAndCurrentLanguage_Expect_PublicUrlAndCurrentLanguage()
        {
            var result = _builder.ToAppRelative("/");
            Assert.Equal("http://localhost/public/en-US/", result);
        }

        [Fact]
        public void When_CurrentStoreAndUnknownLanguage_Expect_PublicUrlAndDefaultLanguage()
        {
            var store = _workContext.CurrentStore;
            var language = new Language("ja-JP");

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("http://localhost/public/ru-RU/", result);
        }

        [Fact]
        public void When_CurrentStoreAndSelectedLanguage_Expect_PublicUrlAndSelectedLanguage()
        {
            var store = _workContext.CurrentStore;
            var language = store.Languages.Last();

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("http://localhost/public/lt-LT/", result);
        }

        [Fact]
        public void When_SelectedStoreAndUnknownLanguage_Expect_SelectedStoreAndDefaultLanguage()
        {
            var store = _workContext.AllStores.Last();
            var language = new Language("ja-JP");

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("~/Store2/es-ES/", result);
        }

        [Fact]
        public void When_SelectedStoreAndSelectedLanguage_Expect_SelectedStoreAndSelectedLanguage()
        {
            var store = _workContext.AllStores.Last();
            var language = store.Languages.Last();

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("~/Store2/fr-FR/", result);
        }
    }

    public class UrlBuilderSecureStoreUrl
    {
        private readonly WorkContext _workContext;
        private readonly IStorefrontUrlBuilder _builder;

        public UrlBuilderSecureStoreUrl()
        {
            _workContext = new WorkContext
            {
                RequestUrl = new Uri("https://localhost/secure/path"),
                AllStores = new[]
                {
                    new Store
                    {
                        Id = "Store1",
                        Url = "http://localhost/public",
                        SecureUrl = "https://localhost/secure",
                        DefaultLanguage = new Language("ru-RU"),
                        Languages = new List<Language>(new[]
                        {
                            new Language("en-US"),
                            new Language("ru-RU"),
                            new Language("lt-LT"),
                        }),
                    },
                    new Store
                    {
                        Id = "Store2",
                        DefaultLanguage = new Language("es-ES"),
                        Languages = new List<Language>(new[]
                        {
                            new Language("de-DE"),
                            new Language("es-ES"),
                            new Language("fr-FR"),
                        }),
                    }
                }
            };

            _workContext.CurrentStore = _workContext.AllStores.First();
            _workContext.CurrentLanguage = _workContext.CurrentStore.Languages.First();

            _builder = new StorefrontUrlBuilder(_workContext);
        }

        [Fact]
        public void When_StoreIsNullAndLanguageIsNull_Expect_VirtualRoot()
        {
            var result = _builder.ToAppRelative("/", null, null);
            Assert.Equal("~/", result);
        }

        [Fact]
        public void When_StoreIsNullAndLanguageIsAny_Expect_VirtualRoot()
        {
            var result = _builder.ToAppRelative("/", null, new Language("en-US"));
            Assert.Equal("~/", result);
        }

        [Fact]
        public void When_CurrentStoreAndCurrentLanguage_Expect_SecureUrlAndCurrentLanguage()
        {
            var result = _builder.ToAppRelative("/");
            Assert.Equal("https://localhost/secure/en-US/", result);
        }

        [Fact]
        public void When_CurrentStoreAndUnknownLanguage_Expect_SecureUrlAndDefaultLanguage()
        {
            var store = _workContext.CurrentStore;
            var language = new Language("ja-JP");

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("https://localhost/secure/ru-RU/", result);
        }

        [Fact]
        public void When_CurrentStoreAndSelectedLanguage_Expect_SecureUrlAndSelectedLanguage()
        {
            var store = _workContext.CurrentStore;
            var language = store.Languages.Last();

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("https://localhost/secure/lt-LT/", result);
        }

        [Fact]
        public void When_SelectedStoreAndUnknownLanguage_Expect_SelectedStoreAndDefaultLanguage()
        {
            var store = _workContext.AllStores.Last();
            var language = new Language("ja-JP");

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("~/Store2/es-ES/", result);
        }

        [Fact]
        public void When_SelectedStoreAndSelectedLanguage_Expect_SelectedStoreAndSelectedLanguage()
        {
            var store = _workContext.AllStores.Last();
            var language = store.Languages.Last();

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("~/Store2/fr-FR/", result);
        }
    }
}
