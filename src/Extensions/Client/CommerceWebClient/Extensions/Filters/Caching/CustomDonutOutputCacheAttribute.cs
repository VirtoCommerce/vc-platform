using System;
using System.Web.Mvc;
using DevTrends.MvcDonutCaching;
using VirtoCommerce.Web.Client.Helpers;

namespace VirtoCommerce.Web.Client.Extensions.Filters.Caching
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CustomDonutOutputCacheAttribute : DonutOutputCacheAttribute
    {

        /// <summary>
        /// Gets or sets a value indicating whether cache is for anonymous users only.
        /// </summary>
        /// <value>
        ///   <c>true</c> if cache is for anonymous users only otherwise, <c>false</c>.
        /// </value>
        public bool AnonymousOnly { get; set; }

        public CustomDonutOutputCacheAttribute()
            : base(new CacheKeyBuilder())
        {
            //Must go afer canonicalize and localize
            Order = 3;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (AnonymousOnly && StoreHelper.CustomerSession.IsRegistered)
            {
                var originalDuration = Duration;
                Duration = 0;
                base.OnActionExecuting(filterContext);
                Duration = originalDuration;
            }
            else
            {
                base.OnActionExecuting(filterContext);
            }
        }
    }
}
