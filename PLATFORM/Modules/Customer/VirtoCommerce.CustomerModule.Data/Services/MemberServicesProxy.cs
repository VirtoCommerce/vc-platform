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
    public class MemberServicesProxy : IMemberService
    {
        private readonly IDynamicPropertyService _dynamicPropertyService;
        private readonly ISecurityService _securityService;
        private readonly IMemberServicesFactory _memberFactory;

        public MemberServicesProxy(IDynamicPropertyService dynamicPropertyService, ISecurityService securityService, IMemberServicesFactory memberFactory)
        {
            _dynamicPropertyService = dynamicPropertyService;
            _securityService = securityService;
            _memberFactory = memberFactory;
        }


        #region IMemberService Members
       
        public virtual Member TryCreateMember(string memberType)
        {
            //LastOrDefault needs to override  member type to new in other service
           return _memberFactory.MemberServices.Select(x => x.TryCreateMember(memberType)).Where(x => x != null).LastOrDefault();
        }

        public virtual void CreateOrUpdate(Member[] members)
        {
            foreach (var memberService in _memberFactory.MemberServices)
            {
                memberService.CreateOrUpdate(members);
            }         
        }

        public virtual void Delete(string[] ids)
        {
            foreach(var memberService in _memberFactory.MemberServices)
            {
                memberService.Delete(ids);
            }
        }

        public virtual Member[] GetByIds(string[] memberIds)
        {
            var retVal = new ConcurrentBag<Member>();
            Parallel.ForEach(_memberFactory.MemberServices, (x) =>
            {
                foreach(var member in x.GetByIds(memberIds))
                {
                    retVal.Add(member);
                }
            });
            return retVal.ToArray();
        }

        public virtual MembersSearchResult SearchMembers(MembersSearchCriteria criteria)
        {
            var retVal = new MembersSearchResult();
            var skip = criteria.Skip;
            var take = criteria.Take;

            foreach (var memberService in _memberFactory.MemberServices)
            {
                if (criteria.Take >= 0)
                {
                    var result = memberService.SearchMembers(criteria);
                    retVal.Members.AddRange(result.Members);
                    retVal.TotalCount += result.TotalCount;
                    criteria.Skip = Math.Max(0, skip - retVal.TotalCount);
                    criteria.Take = Math.Max(0, take - result.Members.Count());
                }
            }
            //restore back criteria property values
            criteria.Skip = skip;
            criteria.Take = take;
            return retVal;
        }
        #endregion

        
    }
}
