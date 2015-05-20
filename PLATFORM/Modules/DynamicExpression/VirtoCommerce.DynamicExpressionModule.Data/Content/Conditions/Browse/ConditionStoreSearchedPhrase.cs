using System;
using System.Reflection;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Model.DynamicContent;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.DynamicExpressionModule.Data.Content
{
	//Shopper searched for phrase [] in store
	public class ConditionStoreSearchedPhrase : MatchedConditionBase
	{
		public ConditionStoreSearchedPhrase()
			: base("ShopperSearchedPhraseInStore")
		{
		}
	
	}
}