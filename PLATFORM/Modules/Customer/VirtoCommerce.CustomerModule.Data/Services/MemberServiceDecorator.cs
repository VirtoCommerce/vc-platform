using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.CustomerModule.Data.Services
{
    /// <summary>
    /// Translate all IMemberServices calls to multiple different same service instances with other member types getting from IMembersFactory. 
    /// </summary>
    public sealed class MemberServiceDecorator : IMemberService, IMemberFactory, IMemberSearchService
    {
        private List<Type> _knownMemberTypes = new List<Type>(); 
        private Dictionary<Type, IMemberService> _memberServicesMap = new Dictionary<Type, IMemberService>();
        private Dictionary<Type, IMemberSearchService> _memberSearchServicesMap = new Dictionary<Type, IMemberSearchService>();
        private Type _memberSearchCriteriaType = typeof(MembersSearchCriteria);

        public MemberServiceDecorator UseMemberSearchCriteriaType<T>()
        {
            _memberSearchCriteriaType = typeof(T);
            return this;
        }

        public MemberServiceDecorator RegisterMemberType<T>(IMemberService memberService, IMemberSearchService memberSearchService) where T : Member
        {
            var memberType = typeof(T);
            if(_knownMemberTypes.Contains(memberType))
            {
                throw new ArgumentException("Already exist");
            }
            _knownMemberTypes.Add(memberType);
            if (memberService != null)
            {
                _memberServicesMap[memberType] = memberService;
            }
            if (memberSearchService != null)
            {
                _memberSearchServicesMap[memberType] = memberSearchService;
            }
            return this;
        }

        public MemberServiceDecorator OverrideMemberType<TOld, TNew>(IMemberService memberService = null, IMemberSearchService memberSearchService = null) where TOld : Member where TNew : Member
        {
            var oldMemberType = typeof(TOld);
            var newMemberType = typeof(TNew);
            if (!_knownMemberTypes.Contains(oldMemberType))
            {
                throw new ArgumentException("Type not exist");
            }
            if(!newMemberType.IsDerivativeOf(oldMemberType))
            {
                throw new ArgumentException("The new type must be inherited from the old");
            }
            _knownMemberTypes.Remove(oldMemberType);
            _knownMemberTypes.Add(newMemberType);

            if (memberService != null)
            {
                _memberServicesMap.Remove(oldMemberType);
                _memberServicesMap[newMemberType] = memberService;
            }
            if (memberSearchService != null)
            {
                _memberSearchServicesMap.Remove(oldMemberType);
                _memberSearchServicesMap[newMemberType] = memberSearchService;
            }

            return this;
        }

        private IEnumerable<IMemberService> AllMemberServices
        {
            get
            {
                return _memberServicesMap.GroupBy(x => x.Value).Select(x => x.Key);
            }
        }
       
        #region IMembersFactory Members
        public Member TryCreateMember(string memberType)
        {
            Member retVal = null;
            //Find in known types inheritance chain to support derived member type
            var knownMemberType = _knownMemberTypes.FirstOrDefault(x => x.GetTypeInheritanceChainTo(typeof(Member)).Any(y=>string.Equals(y.Name, memberType, StringComparison.OrdinalIgnoreCase)));
            if (knownMemberType != null)
            {
                retVal = Activator.CreateInstance(knownMemberType) as Member;
            }
            return retVal;
        }

        public MembersSearchCriteria CreateMemberSearchCriteria()
        {
            return Activator.CreateInstance(_memberSearchCriteriaType) as MembersSearchCriteria;
        }
        #endregion

        #region IMemberService Members

        public void CreateOrUpdate(Member[] members)
        {
            var memberServicesGroups = _memberServicesMap.GroupBy(x => x.Value);
            foreach (var memberServiceGroup in memberServicesGroups)
            {
                var supportMemberTypes = memberServiceGroup.SelectMany(x => x.Key.GetTypeInheritanceChainTo(typeof(Member))).Distinct().ToArray();
                var supportMembers = members.Where(x => supportMemberTypes.Contains(x.GetType())).ToArray();
                if (!supportMembers.IsNullOrEmpty())
                {
                    memberServiceGroup.Key.CreateOrUpdate(members);
                }
            }         
        }

        public void Delete(string[] ids)
        {
            foreach(var memberService in AllMemberServices)
            {
                memberService.Delete(ids);
            }
        }

        public  Member[] GetByIds(string[] memberIds)
        {
            var retVal = new ConcurrentBag<Member>();
            Parallel.ForEach(AllMemberServices, (x) =>
            {
                foreach(var member in x.GetByIds(memberIds))
                {
                    retVal.Add(member);
                }
            });
            return retVal.ToArray();
        }

        #endregion

        #region IMemberSearchService Members
        public MembersSearchResult SearchMembers(MembersSearchCriteria criteria)
        {
            var retVal = new MembersSearchResult();
            var skip = criteria.Skip;
            var take = criteria.Take;
            var memberTypes = criteria.MemberTypes;
            foreach (var memberSearchServiceGroup in _memberSearchServicesMap.GroupBy(x => x.Value))
            {
                //Get all inheritance chain
                var searchServiceSupportedMemberTypes = memberSearchServiceGroup.SelectMany(x => x.Key.GetTypeInheritanceChainTo(typeof(Member))).Select(x => x.Name).Distinct().ToArray();
                criteria.MemberTypes = memberTypes.IsNullOrEmpty() ? searchServiceSupportedMemberTypes : searchServiceSupportedMemberTypes.Intersect(criteria.MemberTypes, StringComparer.OrdinalIgnoreCase).ToArray();
                if (!criteria.MemberTypes.IsNullOrEmpty() && criteria.Take >= 0)
                {
                    var result = memberSearchServiceGroup.Key.SearchMembers(criteria);
                    retVal.Members.AddRange(result.Members);
                    retVal.TotalCount += result.TotalCount;
                    criteria.Skip = Math.Max(0, skip - retVal.TotalCount);
                    criteria.Take = Math.Max(0, take - result.Members.Count());
                }
            }
            //restore back criteria property values
            criteria.Skip = skip;
            criteria.Take = take;
            criteria.MemberTypes = memberTypes;
            return retVal;
        } 
        #endregion




    }

    
}
