using System;
using System.Linq;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Collections.Specialized;
using MvcSiteMapProvider.Web.Script.Serialization;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Web.Client.Helpers;

namespace VirtoCommerce.Web.Virto.Helpers.MVC.SiteMap
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
                case Client.Extensions.Routing.Constants.Store:
                    currentValue = SettingsHelper.SeoDecode(currentValue, SeoUrlKeywordTypes.Store);
                    value = SettingsHelper.SeoDecode(value.ToString(), SeoUrlKeywordTypes.Store);   
                    break;
                case Client.Extensions.Routing.Constants.Category:
                    currentValue = SettingsHelper.SeoDecode(currentValue, SeoUrlKeywordTypes.Category);
                    value = SettingsHelper.SeoDecode(value.ToString(), SeoUrlKeywordTypes.Category);
                    if (currentValue.Equals(value.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                    //Check only category code which is last segment in path
                    value = value.ToString().Split(new[] {"/"}, StringSplitOptions.RemoveEmptyEntries).Last();
                    break;
                case Client.Extensions.Routing.Constants.Item:
                    currentValue = SettingsHelper.SeoDecode(currentValue, SeoUrlKeywordTypes.Item);
                    value = SettingsHelper.SeoDecode(value.ToString(), SeoUrlKeywordTypes.Item);   
                    break;
            }

            return currentValue.Equals(value.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }

    }

}