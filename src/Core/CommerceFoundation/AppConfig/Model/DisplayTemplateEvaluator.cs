using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExpressionSerialization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.AppConfig.Repositories;


namespace VirtoCommerce.Foundation.AppConfig.Model
{
	public class DisplayTemplateEvaluator : EvaluatorBase, IDisplayTemplateEvaluator
	{
		#region Dependencies
		private readonly IAppConfigRepository _repository;
		private readonly IEvaluationPolicy[] _policies;
		#endregion

		#region privates
		private static bool _isEnabled;
		#endregion

        public const string DisplayTemplateCacheKey = "D:T:{0}";

	    #region ctor
		public DisplayTemplateEvaluator(IAppConfigRepository repository, IEvaluationPolicy[] policies, ICacheRepository cache)
			:base(cache)
		{
			_repository = repository;
			_policies = policies;

			_isEnabled = AppConfigConfiguration.Instance != null && AppConfigConfiguration.Instance.Cache.IsEnabled;
			Cache = new CacheHelper(cache);
		}
		#endregion

		#region IDisplayTemplateEvaluator members
		public string Evaluate(IDisplayTemplateEvaluationContext context)
		{
			if (!(context.ContextObject is Dictionary<string, object>))
				throw new ArgumentException("context.ContextObject must be a Dictionary");

			string retVal = string.Empty;

			var query = GetTemplateMappings();

			//filter only active
			query = query.Where(t => t.IsActive);

			// sort content by type and priority
			query = query.OrderByDescending(x => x.Priority).ThenByDescending(x => x.Name);
									
			query = query.Where(x => x.TargetType == (int) context.TargetType);
						
			//Evaluate query
			var current = query.ToArray();

			//Evaluate condition expression
			Func<string, bool> conditionPredicate = (x) =>
			{
				var condition = DeserializeExpression<Func<IEvaluationContext, bool>>(x);
				return condition(context);
			};

			current = current.Where(x => string.IsNullOrEmpty(x.PredicateSerialized) || conditionPredicate(x.PredicateSerialized)).ToArray();

			if (current.Any())
				retVal = current.First().DisplayTemplate;

			return retVal;
		}
		#endregion

		private IQueryable<DisplayTemplateMapping> GetTemplateMappings()
		{
			var query = _repository.DisplayTemplateMappings;

			return Cache.Get(
                CacheHelper.CreateCacheKey(Constants.DisplayTemplateCachePrefix, string.Format(DisplayTemplateCacheKey, "allTemplates")),
				() => (query).ToArray(),
				AppConfigConfiguration.Instance != null ? AppConfigConfiguration.Instance.Cache.DisplayTemplateMappingsTimeout : new TimeSpan(),
				_isEnabled).AsQueryable();
		}
	} //class
} //namespace
