using System;
using System.Reflection;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Model.DynamicContent;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.MarketingModule.Expressions.Content
{
	//Are browsing from zip/postal code []
	public class ConditionGeoZipCode : MatchedConditionBase
	{
		public ConditionGeoZipCode()
			:base("GeoZipCode")
		{
		}
	}
}