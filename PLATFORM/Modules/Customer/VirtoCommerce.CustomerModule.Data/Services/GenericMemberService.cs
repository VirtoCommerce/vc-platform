using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Security;

namespace VirtoCommerce.CustomerModule.Data.Services
{
    /// <summary>
    /// Generic member service implementation used members service factory to working with custom members types 
    /// </summary>
    public class GenericMemberService : IMemberService
    {
        private readonly IDynamicPropertyService _dynamicPropertyService;
        private readonly ISecurityService _securityService;
        private readonly IMembersFactory _memberFactory;

        public GenericMemberService(IDynamicPropertyService dynamicPropertyService, ISecurityService securityService, IMembersFactory memberFactory)
        {
            _dynamicPropertyService = dynamicPropertyService;
            _securityService = securityService;
            _memberFactory = memberFactory;
        }

        #region IMemberService Members
        public virtual Member TryCreateMember(string memberType)
        {
           return _memberFactory.GetAllServices().Select(x => x.TryCreateMember(memberType)).Where(x => x != null).FirstOrDefault();
        }

        public virtual void CreateOrUpdate(Member[] members)
        {
            foreach (var memberService in _memberFactory.GetAllServices())
            {
                memberService.CreateOrUpdate(members);
            }         
        }

        public virtual void Delete(string[] ids)
        {
            foreach(var memberService in _memberFactory.GetAllServices())
            {
                memberService.Delete(ids);
            }
        }

        public virtual Member[] GetByIds(string[] memberIds)
        {
            var retVal = new ConcurrentBag<Member>();
            Parallel.ForEach(_memberFactory.GetAllServices(), (x) =>
            {
                foreach(var member in x.GetByIds(memberIds))
                {
                    retVal.Add(member);
                }
            });
            return retVal.ToArray();
        }

        public virtual SearchResult SearchMembers(SearchCriteria criteria)
        {
            var retVal = new SearchResult();
            var skip = criteria.Skip;
            var take = criteria.Take;
            foreach (var memberService in _memberFactory.GetAllServices())
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
