using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Storefront.Common;
using VirtoCommerce.Storefront.Model;
using VirtoCommerce.Storefront.Model.Common;
using Xunit;

namespace VirtoCommerce.Storefront.Test
{
    [Trait("Category", "CI")]
    public class UrlBuilderSingleStoreSingleLanguage
    {
        private readonly WorkContext _workContext;
        private readonly IStorefrontUrlBuilder _builder;

        public UrlBuilderSingleStoreSingleLanguage()
        {
            _workContext = new WorkContext
            {
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
        public void When_AbsoluteUrl_Expect_AbsoluteUrl()
        {
            var result = _builder.ToAppRelative("http://domain/path");
            Assert.Equal("http://domain/path", result);
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

    [Trait("Category", "CI")]
    public class UrlBuilderSingleStoreMultipleLanguages
    {
        private readonly WorkContext _workContext;
        private readonly IStorefrontUrlBuilder _builder;

        public UrlBuilderSingleStoreMultipleLanguages()
        {
            _workContext = new WorkContext
            {
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
        public void When_AbsoluteUrl_Expect_AbsoluteUrl()
        {
            var result = _builder.ToAppRelative("http://domain/path");
            Assert.Equal("http://domain/path", result);
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

    [Trait("Category", "CI")]
    public class UrlBuilderMultipleStoresSingleLanguage
    {
        private readonly WorkContext _workContext;
        private readonly IStorefrontUrlBuilder _builder;

        public UrlBuilderMultipleStoresSingleLanguage()
        {
            _workContext = new WorkContext
            {
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
                    new Store
                    {
                        Id = "Store2",
                        DefaultLanguage = new Language("de-DE"),
                        Languages = new List<Language>(new[]
                        {
                            new Language("de-DE"),
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
        public void When_CurrentStoreAndCurrentLanguage_Expect_CurrentStoreAndNoLanguage()
        {
            var result = _builder.ToAppRelative("/");
            Assert.Equal("~/Store1/", result);
        }

        [Fact]
        public void When_AbsoluteUrl_Expect_AbsoluteUrl()
        {
            var result = _builder.ToAppRelative("http://domain/path");
            Assert.Equal("http://domain/path", result);
        }

        [Fact]
        public void When_CurrentStoreAndUnknownLanguage_Expect_CurrentStoreAndNoLanguage()
        {
            var store = _workContext.CurrentStore;
            var language = new Language("ja-JP");

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("~/Store1/", result);
        }

        [Fact]
        public void When_SelectedStoreAndUnknownLanguage_Expect_SelectedStoreAndNoLanguage()
        {
            var store = _workContext.AllStores.Last();
            var language = new Language("ja-JP");

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("~/Store2/", result);
        }

        [Fact]
        public void When_SelectedStoreAndSelectedLanguage_Expect_SelectedStoreAndNoLanguage()
        {
            var store = _workContext.AllStores.Last();
            var language = store.Languages.Last();

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("~/Store2/", result);
        }
    }

    [Trait("Category", "CI")]
    public class UrlBuilderMultipleStoresMultipleLanguages
    {
        private readonly WorkContext _workContext;
        private readonly IStorefrontUrlBuilder _builder;

        public UrlBuilderMultipleStoresMultipleLanguages()
        {
            _workContext = new WorkContext
            {
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
        public void When_AbsoluteUrl_Expect_AbsoluteUrl()
        {
            var result = _builder.ToAppRelative("http://domain/path");
            Assert.Equal("http://domain/path", result);
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

    [Trait("Category", "CI")]
    public class UrlBuilderStoreWithUrlsInsecureRequest
    {
        private readonly WorkContext _workContext;
        private readonly IStorefrontUrlBuilder _builder;

        public UrlBuilderStoreWithUrlsInsecureRequest()
        {
            _workContext = new WorkContext
            {
                RequestUrl = new Uri("http://localhost/insecure1/path"),
                AllStores = new[]
                {
                    new Store
                    {
                        Id = "Store1",
                        Url = "http://localhost/insecure1",
                        SecureUrl = "https://localhost/secure1",
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
        public void When_CurrentStoreAndCurrentLanguage_Expect_InsecureUrlAndCurrentLanguage()
        {
            var result = _builder.ToAppRelative("/");
            Assert.Equal("http://localhost/insecure1/en-US/", result);
        }

        [Fact]
        public void When_AbsoluteUrl_Expect_AbsoluteUrl()
        {
            var result = _builder.ToAppRelative("http://domain/path");
            Assert.Equal("http://domain/path", result);
        }

        [Fact]
        public void When_CurrentStoreAndUnknownLanguage_Expect_InsecureUrlAndDefaultLanguage()
        {
            var store = _workContext.CurrentStore;
            var language = new Language("ja-JP");

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("http://localhost/insecure1/ru-RU/", result);
        }

        [Fact]
        public void When_CurrentStoreAndSelectedLanguage_Expect_InsecureUrlAndSelectedLanguage()
        {
            var store = _workContext.CurrentStore;
            var language = store.Languages.Last();

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("http://localhost/insecure1/lt-LT/", result);
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

    [Trait("Category", "CI")]
    public class UrlBuilderStoreWithUrlsSecureRequest
    {
        private readonly WorkContext _workContext;
        private readonly IStorefrontUrlBuilder _builder;

        public UrlBuilderStoreWithUrlsSecureRequest()
        {
            _workContext = new WorkContext
            {
                RequestUrl = new Uri("https://localhost/secure1/path"),
                AllStores = new[]
                {
                    new Store
                    {
                        Id = "Store1",
                        Url = "http://localhost/insecure1",
                        SecureUrl = "https://localhost/secure1",
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
            Assert.Equal("https://localhost/secure1/en-US/", result);
        }

        [Fact]
        public void When_AbsoluteUrl_Expect_AbsoluteUrl()
        {
            var result = _builder.ToAppRelative("http://domain/path");
            Assert.Equal("http://domain/path", result);
        }

        [Fact]
        public void When_CurrentStoreAndUnknownLanguage_Expect_SecureUrlAndDefaultLanguage()
        {
            var store = _workContext.CurrentStore;
            var language = new Language("ja-JP");

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("https://localhost/secure1/ru-RU/", result);
        }

        [Fact]
        public void When_CurrentStoreAndSelectedLanguage_Expect_SecureUrlAndSelectedLanguage()
        {
            var store = _workContext.CurrentStore;
            var language = store.Languages.Last();

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("https://localhost/secure1/lt-LT/", result);
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

    [Trait("Category", "CI")]
    public class UrlBuilderStoreWithUrlsRequestFromDifferentStore
    {
        private readonly WorkContext _workContext;
        private readonly IStorefrontUrlBuilder _builder;

        public UrlBuilderStoreWithUrlsRequestFromDifferentStore()
        {
            _workContext = new WorkContext
            {
                RequestUrl = new Uri("https://localhost/secure2/path"),
                AllStores = new[]
                {
                    new Store
                    {
                        Id = "Store1",
                        Url = "http://localhost/insecure1",
                        SecureUrl = "https://localhost/secure1",
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
        public void When_CurrentStoreAndCurrentLanguage_Expect_InsecureUrlAndCurrentLanguage()
        {
            var result = _builder.ToAppRelative("/");
            Assert.Equal("http://localhost/insecure1/en-US/", result);
        }

        [Fact]
        public void When_AbsoluteUrl_Expect_AbsoluteUrl()
        {
            var result = _builder.ToAppRelative("http://domain/path");
            Assert.Equal("http://domain/path", result);
        }

        [Fact]
        public void When_CurrentStoreAndUnknownLanguage_Expect_InsecureUrlAndDefaultLanguage()
        {
            var store = _workContext.CurrentStore;
            var language = new Language("ja-JP");

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("http://localhost/insecure1/ru-RU/", result);
        }

        [Fact]
        public void When_CurrentStoreAndSelectedLanguage_Expect_InsecureUrlAndSelectedLanguage()
        {
            var store = _workContext.CurrentStore;
            var language = store.Languages.Last();

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("http://localhost/insecure1/lt-LT/", result);
        }
    }

    [Trait("Category", "CI")]
    public class UrlBuilderStoreWithUrlsNoRequest
    {
        private readonly WorkContext _workContext;
        private readonly IStorefrontUrlBuilder _builder;

        public UrlBuilderStoreWithUrlsNoRequest()
        {
            _workContext = new WorkContext
            {
                AllStores = new[]
                {
                    new Store
                    {
                        Id = "Store1",
                        Url = "http://localhost/insecure1",
                        SecureUrl = "https://localhost/secure1",
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
        public void When_CurrentStoreAndCurrentLanguage_Expect_InsecureUrlAndCurrentLanguage()
        {
            var result = _builder.ToAppRelative("/");
            Assert.Equal("http://localhost/insecure1/en-US/", result);
        }

        [Fact]
        public void When_AbsoluteUrl_Expect_AbsoluteUrl()
        {
            var result = _builder.ToAppRelative("http://domain/path");
            Assert.Equal("http://domain/path", result);
        }

        [Fact]
        public void When_CurrentStoreAndUnknownLanguage_Expect_InsecureUrlAndDefaultLanguage()
        {
            var store = _workContext.CurrentStore;
            var language = new Language("ja-JP");

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("http://localhost/insecure1/ru-RU/", result);
        }

        [Fact]
        public void When_CurrentStoreAndSelectedLanguage_Expect_InsecureUrlAndSelectedLanguage()
        {
            var store = _workContext.CurrentStore;
            var language = store.Languages.Last();

            var result = _builder.ToAppRelative("/", store, language);
            Assert.Equal("http://localhost/insecure1/lt-LT/", result);
        }
    }
}
