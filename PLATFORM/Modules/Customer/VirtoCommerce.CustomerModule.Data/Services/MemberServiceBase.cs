using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using VirtoCommerce.CustomerModule.Data.Model;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CustomerModule.Data.Services
{
    public abstract class MemberServiceBase : ServiceBase, IMemberService, IMemberSearchService
    {
        public MemberServiceBase(Func<IMemberRepository> repositoryFactory, IDynamicPropertyService dynamicPropertyService, IMemberFactory memberFactory)
        {
            RepositoryFactory = repositoryFactory;
            DynamicPropertyService = dynamicPropertyService;
            MemberFactory = memberFactory;
        }
        protected IMemberFactory MemberFactory { get; private set; }
        protected Func<IMemberRepository> RepositoryFactory { get; private set; }
        protected IDynamicPropertyService DynamicPropertyService { get; private set; }

        protected abstract MemberDataEntity TryCreateDataMember(Member member);

        #region IMemberService Members
        public virtual Member[] GetByIds(params string[] memberIds)
        {
            var retVal = new List<Member>();
            using (var repository = RepositoryFactory())
            {
                var dataMembers = repository.GetMembersByIds(memberIds);
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
                        }
                        else
                        {
                            repository.Add(dataSourceMember);
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

        public virtual void Delete(string[] ids)
        {
            using (var repository = RepositoryFactory())
            {
                var members = GetByIds(ids);
                if (!members.IsNullOrEmpty())
                {
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
                if (EnumerableExtensions.IsNullOrEmpty(sortInfos))
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
