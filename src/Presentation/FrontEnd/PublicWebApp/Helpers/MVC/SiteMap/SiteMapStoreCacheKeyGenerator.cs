using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Web.Mvc;
using VirtoCommerce.ApiWebClient.Helpers;

namespace VirtoCommerce.Web.Helpers.MVC.SiteMap
{
    public class SiteMapStoreCacheKeyGenerator : SiteMapCacheKeyGenerator
    {

        public SiteMapStoreCacheKeyGenerator(IMvcContextFactory mvcContextFactory)
            : base(mvcContextFactory)
        {

        }

        public override string GenerateKey()
        {
            var key = base.GenerateKey();
            return key + StoreHelper.CustomerSession.StoreId;
        }
    }
}