using System;
using System.Linq;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Collections.Specialized;
using MvcSiteMapProvider.Web.Script.Serialization;
using VirtoCommerce.ApiClient.DataContracts;
using VirtoCommerce.ApiWebClient.Extensions.Routing;
using VirtoCommerce.ApiWebClient.Helpers;

namespace VirtoCommerce.Web.Helpers.MVC.SiteMap
{
    public class SeoRouteValueDictionary : RouteValueDictionary
    {
        public SeoRouteValueDictionary(string siteMapNodeKey, string memberName, ISiteMap siteMap,
            IReservedAttributeNameProvider reservedAttributeNameProvider,
            IJsonToDictionaryDeserializer jsonToDictionaryDeserializer, ICache cache)
            : base(
                siteMapNodeKey, memberName, siteMap, reservedAttributeNameProvider, jsonToDictionaryDeserializer, cache)
        {
        }


        protected override bool MatchesValue(string key, object value)
        {
            if (base.MatchesValue(key,value))
            {
                return true;
            }

            var currentValue = this[key].ToString();

            switch (key)
            {
                case Constants.Store:
                    currentValue = SettingsHelper.SeoDecode(currentValue, SeoUrlKeywordTypes.Store);
                    value = SettingsHelper.SeoDecode(value.ToString(), SeoUrlKeywordTypes.Store);   
                    break;
                case Constants.Category:
                    currentValue = SettingsHelper.SeoDecode(currentValue, SeoUrlKeywordTypes.Category);
                    value = SettingsHelper.SeoDecode(value.ToString(), SeoUrlKeywordTypes.Category);
                    if (currentValue.Equals(value.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                    //Check only category code which is last segment in path
                    value = value.ToString().Split(new[] {"/"}, StringSplitOptions.RemoveEmptyEntries).Last();
                    break;
                case Constants.Item:
                    currentValue = SettingsHelper.SeoDecode(currentValue, SeoUrlKeywordTypes.Item);
                    value = SettingsHelper.SeoDecode(value.ToString(), SeoUrlKeywordTypes.Item);   
                    break;
            }

            return currentValue.Equals(value.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }

    }

}