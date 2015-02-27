using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using ExpressionSerialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.Foundation.Catalogs.Services
{
	public class PriceListAssignmentEvaluator : IPriceListAssignmentEvaluator
	{
		#region Private Variables
		private readonly IPricelistRepository _repository;
		private IEvaluationPolicy[] _policies;
		private static CacheHelper _cache;
		private static bool _isEnabled;
		#endregion

        public const string PricelistAssignmentCacheKey = "C:PLA:{0}";

	    public PriceListAssignmentEvaluator(IPricelistRepository repository, IEvaluationPolicy[] policies, ICacheRepository cache)
		{
			_repository = repository;
			_policies = policies;

			_isEnabled = CatalogConfiguration.Instance.Cache.IsEnabled;
			_cache = new CacheHelper(cache);
		}

		private static ExpressionSerializer _expressionSerializer;
		private static ExpressionSerializer ExpressionSerializer
		{
			get
			{
				if (_expressionSerializer == null)
				{
					var typeResolver = new TypeResolver(AppDomain.CurrentDomain.GetAssemblies());
					_expressionSerializer = new ExpressionSerializer(typeResolver);
				}
				return _expressionSerializer;

			}
		}
		#region IPriceListAssignmentEvaluator Members

        /// <summary>
        /// Evaluates the specified context and returns price lists in the order they should be applied.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
		public Pricelist[] Evaluate(IPriceListAssignmentEvaluationContext context)
		{
			Pricelist[] retVal = null;

			var query = GetPricelistAssignments();

			// sort content by type and priority
			query = query.OrderByDescending(x => x.Priority).ThenByDescending(x => x.Name);

            //filter by catalog
            query = query.Where(x => (x.CatalogId.Equals(context.CatalogId, StringComparison.OrdinalIgnoreCase)));

            //filter by currency
            query = query.Where(x => (x.Pricelist.Currency.Equals(context.Currency, StringComparison.OrdinalIgnoreCase)));

			//filter by date expiration
			query = query.Where(x => (x.StartDate == null || context.CurrentDate >= x.StartDate) && (x.EndDate == null || x.EndDate >= context.CurrentDate));
			
			//Evaluate query
			var current = query.ToArray();

			//Evaluate condition expression
			Func<string, bool> conditionPredicate = x =>
			{
				var condition = DeserializeExpression<Func<IEvaluationContext, bool>>(x);
				return condition(context);
			};

			current = current.Where(x => String.IsNullOrEmpty(x.ConditionExpression) || conditionPredicate(x.ConditionExpression)).ToArray();

			var list = new List<Pricelist>();

			current.ToList().ForEach(x => list.Add(x.Pricelist));
			if (list.Count > 0)
				retVal = list.ToArray();

			return retVal;
		}

		#endregion

		# region privates

		private static T DeserializeExpression<T>(string expression)
		{
			var xElement = XElement.Parse(expression);
			var conditionExpression = ExpressionSerializer.Deserialize<T>(xElement);
			var retVal = conditionExpression.Compile();
			return retVal;
		}

		private IQueryable<PricelistAssignment> GetPricelistAssignments()
		{
			return _cache.Get(
                CacheHelper.CreateCacheKey(Constants.PricelistCachePrefix, string.Format(PricelistAssignmentCacheKey, "all")),
				() => (_repository.PricelistAssignments.Expand("Pricelist")).ToArray(),
				CatalogConfiguration.Instance.Cache.PricelistTimeout,
				_isEnabled).AsQueryable();
		}

		#endregion
	}
}
