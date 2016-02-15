using System;
using VirtoCommerce.Domain.Search.Model;
using VirtoCommerce.Domain.Search.Services;

namespace VirtoCommerce.SearchModule.Tests
{
    public class SearchHelper
    {
        public static void CreateSampleIndex(ISearchProvider provider, string scope)
        {
            provider.Index(scope, "catalogitem", CreateDocument("12345", "sample product", "red", 123.23m, 2, new[] { "sony/186d61d8-d843-4675-9f77-ec5ef603fda3", "apple/186d61d8-d843-4675-9f77-ec5ef603fda3" }));
            provider.Index(scope, "catalogitem", CreateDocument("sad121", "red shirt", "red", 10m, 3, new[] { "sony/186d61d8-d843-4675-9f77-ec5ef603fda3", "apple/186d61d8-d843-4675-9f77-ec5ef603fda3" }));
            provider.Index(scope, "catalogitem", CreateDocument("jdashf", "blue shirt", "blue", 23.12m, 8, new[] { "sony/186d61d8-d843-4675-9f77-ec5ef603fda3", "apple/186d61d8-d843-4675-9f77-ec5ef603fda3" }));
            provider.Index(scope, "catalogitem", CreateDocument("32894hjf", "black sox", "black", 243.12m, 10, new[] { "sony/186d61d8-d843-4675-9f77-ec5ef603fda3", "apple/186d61d8-d843-4675-9f77-ec5ef603fda3" }));
            provider.Commit(scope);
            provider.Close(scope, "catalogitem");
        }

        private static ResultDocument CreateDocument(string key, string name, string color, decimal price, int size, string[] outlines)
        {
            var doc = new ResultDocument();

            doc.Add(new DocumentField("__key", key, new[] { IndexStore.Yes, IndexType.NotAnalyzed }));
            doc.Add(new DocumentField("__type", "product", new[] { IndexStore.Yes, IndexType.NotAnalyzed }));
            doc.Add(new DocumentField("__sort", "1", new[] { IndexStore.Yes, IndexType.NotAnalyzed }));
            doc.Add(new DocumentField("__hidden", "false", new[] { IndexStore.Yes, IndexType.NotAnalyzed }));
            doc.Add(new DocumentField("code", "prd12321", new[] { IndexStore.Yes, IndexType.NotAnalyzed }));
            doc.Add(new DocumentField("name", name, new[] { IndexStore.Yes, IndexType.NotAnalyzed }));
            doc.Add(new DocumentField("startdate", DateTime.UtcNow, new[] { IndexStore.Yes, IndexType.NotAnalyzed }));
            doc.Add(new DocumentField("enddate", DateTime.MaxValue, new[] { IndexStore.Yes, IndexType.NotAnalyzed }));
            doc.Add(new DocumentField("price_usd_default", price, new[] { IndexStore.Yes, IndexType.NotAnalyzed }));
            doc.Add(new DocumentField("price_usd_default_value", price.ToString(), new[] { IndexStore.Yes, IndexType.NotAnalyzed }));
            doc.Add(new DocumentField("color", color, new[] { IndexStore.Yes, IndexType.NotAnalyzed }));
            doc.Add(new DocumentField("catalog", "goods", new[] { IndexStore.Yes, IndexType.NotAnalyzed, IndexDataType.StringCollection }));
            doc.Add(new DocumentField("size", size, new[] { IndexStore.Yes, IndexType.NotAnalyzed }));
            doc.Add(new DocumentField("currency", "USD", new[] { IndexStore.Yes, IndexType.NotAnalyzed }));

            if (outlines != null)
            {
                foreach (var outline in outlines)
                {
                    doc.Add(new DocumentField("__outline", outline, new[] { IndexStore.Yes, IndexType.NotAnalyzed, IndexDataType.StringCollection }));
                }
            }

            doc.Add(new DocumentField("__content", name, new[] { IndexStore.Yes, IndexType.Analyzed }));

            return doc;
        }
    }
}