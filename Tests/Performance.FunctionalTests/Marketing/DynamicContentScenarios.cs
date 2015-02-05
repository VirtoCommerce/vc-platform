using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VirtoCommerce.Caching.HttpCache;
using VirtoCommerce.Foundation.Frameworks.Tagging;
using VirtoCommerce.Foundation.Marketing.DynamicContent;
using VirtoCommerce.Foundation.Marketing.Model.DynamicContent;
using VirtoCommerce.Foundation.Marketing.Services;
using VirtoCommerce.Foundation.Data.Marketing;

namespace PerformanceTests
{
    [TestClass]
    public class MarketingDynamicContent
    {
        [TestMethod]
        [DeploymentItem("connectionStrings.config")]
        [DeploymentItem("Configs/DynamicContent.config", "Configs")]
        public void Run_dynamiccontent_performance()
        {
            var repository = new EFDynamicContentRepository();
            var cache = new HttpCacheRepository();
            var evaluator = new DynamicContentEvaluator(repository, null, cache);
            var service = new DynamicContentService(repository, evaluator);

            DynamicContentConfiguration.Instance.Cache.IsEnabled = true;

            var tags = new TagSet();
            tags.Add("CategoryId", new Tag("VendorVirtual"));
            tags.Add("StoreId", new Tag("SampleStore"));
            tags.Add("CurrentUrl", new Tag("http://localhost/store"));

            var items = service.GetItems("HomeMain", DateTime.Now, tags);
            //items = service.GetItems("HomeMain", DateTime.Now, tags);
        }
    }
}
