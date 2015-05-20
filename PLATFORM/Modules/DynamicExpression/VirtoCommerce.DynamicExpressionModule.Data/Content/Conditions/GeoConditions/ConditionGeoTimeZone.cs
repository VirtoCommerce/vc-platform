using System;
using System.Reflection;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Model.DynamicContent;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.DynamicExpressionModule.Data.Content
{
	//Are browsing from a time zone -/+ offset from UTC 
	public class ConditionGeoTimeZone : CompareConditionBase
	{
		public ConditionGeoTimeZone()
			: base("GeoTimeZone")
		{

		}
		
	}
}