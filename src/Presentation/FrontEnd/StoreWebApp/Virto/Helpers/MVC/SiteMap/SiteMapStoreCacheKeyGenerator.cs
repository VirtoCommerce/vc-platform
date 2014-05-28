using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Web.Mvc;
using VirtoCommerce.Web.Client.Helpers;

namespace VirtoCommerce.Web.Virto.Helpers.MVC.SiteMap
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