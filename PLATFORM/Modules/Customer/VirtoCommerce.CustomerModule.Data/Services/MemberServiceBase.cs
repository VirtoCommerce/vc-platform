using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VirtoCommerce.CustomerModule.Data.Model;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.Domain.Customer.Events;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Events;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CustomerModule.Data.Services
{
    /// <summary>
    /// Abstract base class for all derived custom members services used IMemberRepository for persistent
    /// </summary>
    public abstract class MemberServiceBase : ServiceBase, IMemberService, IMemberSearchService
    {
        public MemberServiceBase(Func<IMemberRepository> repositoryFactory, IDynamicPropertyService dynamicPropertyService,
                                 IMemberFactory memberFactory, IEventPublisher<MemberChangingEvent> eventPublisher)
        {
            RepositoryFactory = repositoryFactory;
            DynamicPropertyService = dynamicPropertyService;
            MemberFactory = memberFactory;
            MemberEventventPublisher = eventPublisher;
        }
        protected IEventPublisher<MemberChangingEvent> MemberEventventPublisher { get; private set; }
        protected IMemberFactory MemberFactory { get; private set; }
        protected Func<IMemberRepository> RepositoryFactory { get; private set; }
        protected IDynamicPropertyService DynamicPropertyService { get; private set; }

        /// <summary>
        /// Create database persistent type DataMember instance for Member instance (domain -> data mapping)
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        protected abstract MemberDataEntity TryCreateDataMember(Member member);

        #region IMemberService Members
        /// <summary>
        /// Return members by requested ids can be override for load extra data for resulting members
        /// </summary>
        /// <param name="memberIds"></param>
        /// <param name="memberTypes"></param>
        /// <returns></returns>
        public virtual Member[] GetByIds(string[] memberIds, string[] memberTypes = null)
        {
            var retVal = new List<Member>();
            using (var repository = RepositoryFactory())
            {
                var dataMembers = repository.GetMembersByIds(memberIds, memberTypes);
                foreach (var dataMember in dataMembers)
                {
                    var member = MemberFactory.TryCreateMember(dataMember.MemberType);
                    if (member != null)
                    {
                        dataMember.ToMember(member);
                        retVal.Add(member);
                    }
                }
            }
            foreach (var member in retVal)
            {
                //Load dynamic properties for member
                DynamicPropertyService.LoadDynamicPropertyValues(member);
            }
            return retVal.ToArray();
        }

        /// <summary>
        /// Create or update members in database
        /// </summary>
        /// <param name="members"></param>
        public virtual void CreateOrUpdate(Member[] members)
        {
            var pkMap = new PrimaryKeyResolvingMap();
        
            using (var repository = RepositoryFactory())
            using (var changeTracker = GetChangeTracker(repository))
            {
                var dataExistMembers = repository.GetMembersByIds(members.Where(x => !x.IsTransient()).Select(x => x.Id).ToArray());
                foreach (var member in members)
                {
                    var dataSourceMember = TryCreateDataMember(member);
                     if (dataSourceMember != null)
                    {
                        dataSourceMember.FromMember(member, pkMap);
                        var dataTargetMember = dataExistMembers.FirstOrDefault(x => x.Id == member.Id);
                        if (dataTargetMember != null)
                        {
                            changeTracker.Attach(dataTargetMember);
                            dataSourceMember.Patch(dataTargetMember);
                            MemberEventventPublisher.Publish(new MemberChangingEvent(EntryState.Modified, member));
                        }
                        else
                        {
                            repository.Add(dataSourceMember);
                            MemberEventventPublisher.Publish(new MemberChangingEvent(EntryState.Added, member));
                        }
                    }                 
                }
                CommitChanges(repository);
                pkMap.ResolvePrimaryKeys();
            }
            //Save dynamic properties
            foreach (var member in members)
            {
                DynamicPropertyService.SaveDynamicPropertyValues(member);
            }
        }

        public virtual void Delete(string[] ids, string[] memberTypes = null)
        {
            using (var repository = RepositoryFactory())
            {
                var members = GetByIds(ids, memberTypes);
                if (!members.IsNullOrEmpty())
                {
                    foreach (var member in members)
                    {
                        MemberEventventPublisher.Publish(new MemberChangingEvent(EntryState.Deleted, member));
                    }
                    repository.RemoveMembersByIds(members.Select(x => x.Id).ToArray());
                    CommitChanges(repository);
                    foreach (var member in members)
                    {
                        DynamicPropertyService.DeleteDynamicPropertyValues(member);
                    }
                }
            }
        }
        #endregion

        #region IMemberSearchService Members
        /// <summary>
        /// Search members in database by given criteria
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual MembersSearchResult SearchMembers(MembersSearchCriteria criteria)
        {
            var retVal = new MembersSearchResult();

            using (var repository = RepositoryFactory())
            {
                var query = LinqKit.Extensions.AsExpandable(repository.Members);

                if (!criteria.MemberTypes.IsNullOrEmpty())
                {
                    query = query.Where(x => criteria.MemberTypes.Contains(x.MemberType) || x.MemberType == null);
                }

                if (criteria.MemberId != null)
                {
                    //TODO: DeepSearch in specified member
                    query = query.Where(x => x.MemberRelations.Any(y => y.AncestorId == criteria.MemberId));
                }
                else
                {
                    if (!criteria.DeepSearch)
                    {
                        query = query.Where(x => !x.MemberRelations.Any());
                    }
                }
                //Get extra predicates (where clause)
                var predicate = GetQueryPredicate(criteria);

                query = query.Where(LinqKit.Extensions.Expand(predicate));

                var sortInfos = criteria.SortInfos;
                if (sortInfos.IsNullOrEmpty())
                {
                    sortInfos = new[] { new SortInfo { SortColumn = ReflectionUtility.GetPropertyName<Member>(x => x.MemberType), SortDirection = SortDirection.Descending } };
                }
                query = query.OrderBySortInfos(sortInfos);

                retVal.TotalCount = query.Count();

                retVal.Members = query.Skip(criteria.Skip).Take(criteria.Take).ToArray()
                                      .Select(x => x.ToMember(MemberFactory.TryCreateMember(x.MemberType))).ToList();
                return retVal;
            }
        }
        #endregion

       /// <summary>
       /// Used to define extra where clause for members search
       /// </summary>
       /// <param name="criteria"></param>
       /// <returns></returns>
        protected virtual  Expression<Func<MemberDataEntity, bool>> GetQueryPredicate(MembersSearchCriteria criteria)
        {
            if (!String.IsNullOrEmpty(criteria.Keyword))
            {
                var predicate = PredicateBuilder.False<MemberDataEntity>();
                predicate = predicate.Or(x =>  x.Name.Contains(criteria.Keyword) || x.Emails.Any(y => y.Address.Contains(criteria.Keyword)));
                //Should use Expand() to all predicates to prevent EF error
                //http://stackoverflow.com/questions/2947820/c-sharp-predicatebuilder-entities-the-parameter-f-was-not-bound-in-the-specif?rq=1
                return LinqKit.Extensions.Expand(predicate);
            }         
            return PredicateBuilder.True<MemberDataEntity>();
        }

      

    }
}
