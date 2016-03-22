using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CustomerModule.Data.Converters;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Data.Infrastructure;
using dataModel = VirtoCommerce.CustomerModule.Data.Model;

namespace VirtoCommerce.CustomerModule.Data.Services
{
    public class OrganizationMemberServiceImpl : ServiceBase, IMemberService
    {
        private readonly Func<ICustomerRepository> _repositoryFactory;
        private readonly IDynamicPropertyService _dynamicPropertyService;
        public OrganizationMemberServiceImpl(Func<ICustomerRepository> repositoryFactory, IDynamicPropertyService dynamicPropertyService)
        {
            _repositoryFactory = repositoryFactory;
            _dynamicPropertyService = dynamicPropertyService;
        }

        #region IMemberService Members
        public virtual Member TryCreateMember(string memberType)
        {
            Member retVal = null;
            if (string.Equals(memberType, "organization", StringComparison.OrdinalIgnoreCase))
            {
                retVal = new Organization();
            }
            return retVal;
        }

        public Member[] GetByIds(params string[] memberIds)
        {
            var retVal = new List<Organization>();
            using (var repository = _repositoryFactory())
            {
                var dbOrgs = repository.GetMembersByIds(memberIds).OfType<dataModel.Organization>();
                foreach (var dbOrg in dbOrgs)
                {
                    var org = dbOrg.ToCoreModel();
                    retVal.Add(org);
                }
            }
            foreach (var org in retVal)
            {
                //Load dynamic properties for org
                _dynamicPropertyService.LoadDynamicPropertyValues(org);
            }

            return retVal.ToArray();
        }

        public void CreateOrUpdate(Member[] members)
        {
            var pkMap = new PrimaryKeyResolvingMap();
            var orgs = members.OfType<Organization>();

            using (var repository = _repositoryFactory())
            using (var changeTracker = GetChangeTracker(repository))
            {
                var dbExistsOrgs = repository.GetMembersByIds(orgs.Where(x => !x.IsTransient()).Select(x => x.Id).ToArray()).OfType<dataModel.Organization>();
                foreach (var org in orgs)
                {
                    var dbSourceOrg = org.ToDataModel(pkMap);
                    var dbTargetOrg = dbExistsOrgs.FirstOrDefault(x => x.Id == org.Id);
                    if (dbTargetOrg != null)
                    {
                        changeTracker.Attach(dbTargetOrg);
                        dbSourceOrg.Patch(dbTargetOrg);
                    }
                    else
                    {
                        repository.Add(dbSourceOrg);
                    }
                }
                CommitChanges(repository);
                pkMap.ResolvePrimaryKeys();
            }
            //Save dynamic properties
            foreach (var org in orgs)
            {
                _dynamicPropertyService.SaveDynamicPropertyValues(org);
            }

        }

        public void Delete(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                var orgs = GetByIds(ids);
                repository.RemoveMembersByIds(orgs.Select(x => x.Id).ToArray());
                CommitChanges(repository);
                foreach (var org in orgs)
                {
                    _dynamicPropertyService.DeleteDynamicPropertyValues(org);
                }
            }
        }


        public SearchResult SearchMembers(SearchCriteria criteria)
        {
            var retVal = new SearchResult();

            using (var repository = _repositoryFactory())
            {
                var query = repository.Organizations;
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

                if (!String.IsNullOrEmpty(criteria.Keyword))
                {
                    query = query.Where(x => x.Name.Contains(criteria.Keyword));
                }

                retVal.TotalCount = query.Count();

                var sortInfos = criteria.SortInfos;
                if (sortInfos.IsNullOrEmpty())
                {
                    sortInfos = new[] { new SortInfo { SortColumn = "Name" } };
                }
                query = query.OrderBySortInfos(sortInfos);

                retVal.Members = query.Skip(criteria.Skip).Take(criteria.Take).ToArray()
                                      .Select(x => x.ToCoreModel()).OfType<Member>().ToList();
                return retVal;
            }
        }
        #endregion   
    }
}
