using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Marketing.DynamicContent;
using VirtoCommerce.Foundation.Marketing.Repositories;


namespace VirtoCommerce.Foundation.Marketing.Model.DynamicContent
{
    public class DynamicContentEvaluator : EvaluatorBase, IDynamicContentEvaluator
    {
        #region Private Variables
        private readonly IDynamicContentRepository _repository;
        private IEvaluationPolicy[] _policies;


        private static bool IsEnabled = false;
        public const string DynamicContentCacheKey = "M:D:{0}";

        #endregion

        public DynamicContentEvaluator(IDynamicContentRepository repository, IEvaluationPolicy[] policies, ICacheRepository cache)
            : base(cache)
        {
            _repository = repository;
            _policies = policies;

            IsEnabled = DynamicContentConfiguration.Instance != null && DynamicContentConfiguration.Instance.Cache.IsEnabled;

        }

        #region IDynamicContentEvaluator Members

        public DynamicContentItem[] Evaluate(IDynamicContentEvaluationContext context)
        {
            DynamicContentItem[] retVal = null;

            if (!(context.ContextObject is Dictionary<string, object>))
                throw new ArgumentException("context.ContextObject must be a Dictionary");

            var query = GetPublishingGroups();
            var places = GetPlaces();

            var place = places.FirstOrDefault(x => x.Name.Equals(context.ContentPlace, StringComparison.OrdinalIgnoreCase));

            if (place != null)
            {
                // sort content by type and priority
                query = query.OrderByDescending(x => x.Priority).ThenByDescending(x => x.Name);

                //filter by date expiration
                query = query.Where(x => (x.StartDate == null || context.CurrentDate >= x.StartDate) && (x.EndDate == null || x.EndDate >= context.CurrentDate));

                //filter only active
                query = query.Where(x => x.IsActive);

                //filter by content places
                query = query.Where(x => x.ContentPlaces.Any(y => y.ContentPlace != null && y.ContentPlace.Name.Equals(context.ContentPlace, StringComparison.OrdinalIgnoreCase)));

                //Evaluate query
                var current = query.ToArray();

                //Evaluate condition expression
                Func<string, bool> conditionPredicate = (x) =>
                {
                    var condition = DeserializeExpression<Func<IEvaluationContext, bool>>(x);
                    return condition(context);
                };

                current = current.Where(x => string.IsNullOrEmpty(x.ConditionExpression) || conditionPredicate(x.ConditionExpression)).ToArray();

                var list = new List<DynamicContentItem>();

                var items = GetDynamicItems();
                current.ToList().ForEach(x => x.ContentItems.ToList().ForEach(y => list.Add(items.Where(z => z.DynamicContentItemId == y.DynamicContentItemId))));
                if (list.Count > 0)
                    retVal = list.ToArray();
            }

            return retVal;
        }

        #endregion

        private IQueryable<DynamicContentPublishingGroup> GetPublishingGroups()
        {
            var query = _repository.PublishingGroups.Expand(g => g.ContentPlaces.Select(c => c.ContentPlace)).Expand(g => g.ContentItems);
            return Cache.Get(
                CacheHelper.CreateCacheKey(Constants.DynamicContentCachePrefix, string.Format(DynamicContentCacheKey, "allGroups")),
                () => (query).ToArray(),
                DynamicContentConfiguration.Instance != null ? DynamicContentConfiguration.Instance.Cache.DynamicContentTimeout : new TimeSpan(),
                IsEnabled).AsQueryable();
        }

        private IQueryable<DynamicContentPlace> GetPlaces()
        {
            var query = _repository.Places;
            return Cache.Get(
                CacheHelper.CreateCacheKey(Constants.DynamicContentCachePrefix, string.Format(DynamicContentCacheKey, "allPlaces")),
                () => (query).ToArray(),
                DynamicContentConfiguration.Instance != null ? DynamicContentConfiguration.Instance.Cache.DynamicContentTimeout : new TimeSpan(),
                IsEnabled).AsQueryable();
        }

        private IQueryable<DynamicContentItem> GetDynamicItems()
        {
            return Cache.Get(
                CacheHelper.CreateCacheKey(Constants.DynamicContentCachePrefix, string.Format(DynamicContentCacheKey, "allItems")),
                () => (_repository.Items.Expand("PropertyValues")).ToArray(),
                DynamicContentConfiguration.Instance.Cache.DynamicContentTimeout,
                IsEnabled).AsQueryable();
        }
    } //class
} //namespace
