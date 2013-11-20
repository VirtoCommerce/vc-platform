using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalTests.Search
{
    using VirtoCommerce.Foundation.Search;
    using VirtoCommerce.Search.Providers.Lucene;

    public class SearchHelper
    {
        public static void CreateSampleIndex(ISearchProvider provider, string scope)
        {
            provider.Index(scope, "catalogitem", CreateDocument("12345", "sample product", "red", 123.23m, 2));
            provider.Index(scope, "catalogitem", CreateDocument("sad121", "red shirt", "red", 10m, 3));
            provider.Index(scope, "catalogitem", CreateDocument("jdashf", "blue shirt", "blue", 23.12m,8));
            provider.Index(scope, "catalogitem", CreateDocument("32894hjf", "black sox", "black", 243.12m, 10));
            provider.Commit(scope);
            provider.Close(scope, "catalogitem");
        }

        private static ResultDocument CreateDocument(string key, string name, string color, decimal price, int size)
        {
            var doc = new ResultDocument();

            doc.Add(new DocumentField("__key", key, new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
            doc.Add(new DocumentField("__type", "product", new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
            doc.Add(new DocumentField("__sort", "1", new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
            doc.Add(new DocumentField("__hidden", "false", new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
            doc.Add(new DocumentField("code", "prd12321", new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
            doc.Add(new DocumentField("name", name, new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
            doc.Add(new DocumentField("startdate", DateTime.Now, new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
            doc.Add(new DocumentField("enddate", DateTime.MaxValue, new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
            doc.Add(new DocumentField("price_usd_default", price, new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
            doc.Add(new DocumentField("price_usd_default_value", price.ToString(), new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
            doc.Add(new DocumentField("color", color, new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
            doc.Add(new DocumentField("catalog", "goods", new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
            doc.Add(new DocumentField("size", size, new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));
            doc.Add(new DocumentField("currency", "goods", new[] { IndexStore.YES, IndexType.NOT_ANALYZED }));

            doc.Add(new DocumentField("__content", name, new[] { IndexStore.YES, IndexType.ANALYZED }));

            return doc;
        }
    }
}
