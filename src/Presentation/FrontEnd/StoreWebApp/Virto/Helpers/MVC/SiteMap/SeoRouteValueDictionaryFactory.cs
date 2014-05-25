using System;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Collections.Specialized;
using MvcSiteMapProvider.Web.Script.Serialization;

namespace VirtoCommerce.Web.Virto.Helpers.MVC.SiteMap
{
    public class SeoRouteValueDictionaryFactory
    : IRouteValueDictionaryFactory
    {
        public SeoRouteValueDictionaryFactory(
        IRequestCache requestCache,
        IReservedAttributeNameProvider reservedAttributeNameProvider,
        IJsonToDictionaryDeserializer jsonToDictionaryDeserializer
        )
        {
            if (requestCache == null)
                throw new ArgumentNullException("requestCache");
            if (reservedAttributeNameProvider == null)
                throw new ArgumentNullException("reservedAttributeNameProvider");
            if (jsonToDictionaryDeserializer == null)
                throw new ArgumentNullException("jsonToDictionaryDeserializer");

            this.requestCache = requestCache;
            this.reservedAttributeNameProvider = reservedAttributeNameProvider;
            this.jsonToDictionaryDeserializer = jsonToDictionaryDeserializer;
        }

        protected readonly IRequestCache requestCache;
        protected readonly IReservedAttributeNameProvider reservedAttributeNameProvider;
        protected readonly IJsonToDictionaryDeserializer jsonToDictionaryDeserializer;

        #region IRouteValueDictionaryFactory Members

        public IRouteValueDictionary Create(string siteMapNodeKey, string memberName, ISiteMap siteMap)
        {
            return new SeoRouteValueDictionary(siteMapNodeKey, memberName, siteMap, this.reservedAttributeNameProvider, this.jsonToDictionaryDeserializer, this.requestCache);
        }

        #endregion
    }
}