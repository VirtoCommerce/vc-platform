using System;
using System.Linq;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Caching;
using MvcSiteMapProvider.Collections.Specialized;
using MvcSiteMapProvider.Web.Script.Serialization;
using VirtoCommerce.ApiWebClient.Extensions.Routing;
using VirtoCommerce.ApiWebClient.Helpers;
using VirtoCommerce.Web.Core.DataContracts;

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

            var language = ContainsKey(Constants.Language) ? this[Constants.Language] as string : null;

            switch (key)
            {
                case Constants.Store:
                    currentValue = SettingsHelper.EncodeRouteValue(currentValue, SeoUrlKeywordTypes.Store, language);
                    value = SettingsHelper.EncodeRouteValue(value.ToString(), SeoUrlKeywordTypes.Store, language);
                    break;
                case Constants.Category:
                    currentValue = SettingsHelper.EncodeRouteValue(currentValue, SeoUrlKeywordTypes.Category, language);
                    value = SettingsHelper.EncodeRouteValue(value.ToString(), SeoUrlKeywordTypes.Category, language);
                    if (currentValue.Equals(value.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                    //Check only category code which is last segment in path
                    value = value.ToString().Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries).Last();
                    break;
                case Constants.Item:
                    currentValue = SettingsHelper.EncodeRouteValue(currentValue, SeoUrlKeywordTypes.Item, language);
                    value = SettingsHelper.EncodeRouteValue(value.ToString(), SeoUrlKeywordTypes.Item, language);
                    break;
            }

            return currentValue.Equals(value.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }

    }

}