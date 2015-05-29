using System;
using System.Reflection;
using VirtoCommerce.Domain.Common;
using VirtoCommerce.Domain.Marketing.Model;
using VirtoCommerce.Domain.Marketing.Model.DynamicContent;
using linq = System.Linq.Expressions;

namespace VirtoCommerce.DynamicExpressionModule.Data.Common
{
	//Shopper searched for phrase [] in store
	public class ConditionStoreSearchedPhrase : MatchedConditionBase<EvaluationContextBase>
	{
		public ConditionStoreSearchedPhrase()
			: base("ShopperSearchedPhraseInStore")
		{
		}
	
	}
}