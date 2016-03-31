using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CustomerModule.Data.Model;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Infrastructure;

namespace VirtoCommerce.CustomerModule.Data.Services
{
    public class CustomerMemberServiceImpl : MemberServiceBase
    {
        private Dictionary<Type, Type> _knownMemberTypesMap = new Dictionary<Type, Type>()
         {
            { typeof(Organization), typeof(OrganizationDataEntity) },
            { typeof(Contact), typeof(ContactDataEntity) },
            { typeof(Employee), typeof(EmployeeDataEntity) },
            { typeof(Vendor), typeof(VendorDataEntity) }
        };

        private readonly ISecurityService _securityService;
        public CustomerMemberServiceImpl(Func<ICustomerRepository> repositoryFactory, IDynamicPropertyService dynamicPropertyService, ISecurityService securityService, IMemberFactory memberFactory)
            : base(repositoryFactory, dynamicPropertyService, memberFactory)
        {
            _securityService = securityService;
        }

        #region MemberServiceBase Overrides
        protected override MemberDataEntity TryCreateDataMember(Member member)
        {
            MemberDataEntity retVal = null;
            var memberDataEntityType = _knownMemberTypesMap.Where(x => x.Key == member.GetType()).Select(x => x.Value).FirstOrDefault();
            if (memberDataEntityType != null)
            {
                retVal = Activator.CreateInstance(memberDataEntityType) as MemberDataEntity;
            }
            return retVal;
        } 
        #endregion
       

        #region IMemberService Members

        public override Member[] GetByIds(params string[] memberIds)
        {
            var retVal = base.GetByIds(memberIds);
            Parallel.ForEach(retVal, new ParallelOptions { MaxDegreeOfParallelism = 10 }, (member) =>
            {
                //Load security accounts for members which support them 
                if (member is IHasSecurityAccounts)
                {
                    //Load all security accounts associated with this contact
                    var result = Task.Run(() => _securityService.SearchUsersAsync(new UserSearchRequest { MemberId = member.Id, TakeCount = int.MaxValue })).Result;
                    ((IHasSecurityAccounts)member).SecurityAccounts.AddRange(result.Users);
                }
            });        
            return retVal;
        }
        #endregion

        protected override Expression<Func<MemberDataEntity, bool>> GetQueryPredicate(MembersSearchCriteria criteria)
        {
            var retVal = base.GetQueryPredicate(criteria);
            if (!String.IsNullOrEmpty(criteria.Keyword))
            {
                //where x or (y1 or y2)
                var predicate = PredicateBuilder.False<MemberDataEntity>();
                predicate = predicate.Or(x => (x is ContactDataEntity && (x as ContactDataEntity).FullName.Contains(criteria.Keyword)));
                predicate = predicate.Or(x => (x is EmployeeDataEntity && (x as EmployeeDataEntity).FullName.Contains(criteria.Keyword)));
                //Should use Expand() to all predicates to prevent EF error
                //http://stackoverflow.com/questions/2947820/c-sharp-predicatebuilder-entities-the-parameter-f-was-not-bound-in-the-specif?rq=1
                retVal = LinqKit.Extensions.Expand(retVal.Or(LinqKit.Extensions.Expand(predicate)));
            }
            return retVal;

        }

     

    }
}
