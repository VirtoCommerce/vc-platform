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
    public class MemberServiceImpl : ServiceBase, IMemberService
    {
        private readonly Func<ICustomerRepository> _repositoryFactory;
        private readonly IDynamicPropertyService _dynamicPropertyService;
        private readonly ISecurityService _securityService;

        public MemberServiceImpl(Func<ICustomerRepository> repositoryFactory, IDynamicPropertyService dynamicPropertyService, ISecurityService securityService)
        {
            _repositoryFactory = repositoryFactory;
            _dynamicPropertyService = dynamicPropertyService;
            _securityService = securityService;
        }

        #region IMemberService Members

        public Member[] GetByIds(params string[] memberIds)
        {
            var retVal = new List<Member>();
            using (var repository = _repositoryFactory())
            {
                var dbMembers = repository.GetMembersByIds(memberIds);
                retVal.AddRange(dbMembers.Select(x => ConvertToMember(x)));
            }

            foreach (var member in retVal)
            {
                //Load dynamic properties for member
                _dynamicPropertyService.LoadDynamicPropertyValues(member);
            }

            return retVal.ToArray();
        }

        public void CreateOrUpdate(Member[] members)
        {
            var pkMap = new PrimaryKeyResolvingMap();
            using (var repository = _repositoryFactory())
            using (var changeTracker = GetChangeTracker(repository))
            {
                var dbExistsMembers = repository.GetMembersByIds(members.Where(x => !x.IsTransient()).Select(x => x.Id).ToArray());
                foreach (var member in members)
                {
                    var dbChangedMember = ConvertToDataMember(member, pkMap);
                    var dbExistMember = dbExistsMembers.FirstOrDefault(x => x.Id == member.Id);
                    if (dbExistMember != null)
                    {
                        changeTracker.Attach(dbExistMember);
                        if (dbChangedMember is dataModel.Contact)
                        {
                            ((dataModel.Contact)dbChangedMember).Patch((dataModel.Contact)dbExistMember);
                        }
                        if (dbChangedMember is dataModel.Organization)
                        {
                            ((dataModel.Organization)dbChangedMember).Patch((dataModel.Organization)dbExistMember);
                        }
                    }
                    else
                    {
                        repository.Add(dbChangedMember);
                    }
                }
                CommitChanges(repository);
                pkMap.ResolvePrimaryKeys();

                foreach (var member in members)
                {
                    _dynamicPropertyService.SaveDynamicPropertyValues(member);
                }
            }

        }

        public void Delete(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                foreach (var id in ids)
                {
                    var members = GetByIds(ids);
                    foreach (var member in members)
                    {
                        _dynamicPropertyService.DeleteDynamicPropertyValues(member);
                    }

                    var dbMembers = repository.GetMembersByIds(ids);
                    foreach (var dbMember in dbMembers)
                    {
                        foreach (var relation in dbMember.MemberRelations.ToArray())
                        {
                            repository.Remove(relation);
                        }
                        repository.Remove(dbMember);
                    }
                }
                CommitChanges(repository);
            }
        }

 
        public SearchResult SearchMembers(SearchCriteria criteria)
        {
            var retVal = new SearchResult();

            using (var repository = _repositoryFactory())
            {
                var query = repository.Members;
                if (criteria.MemberType != null)
                {
                    query = query.Where(x => x.MemberType == criteria.MemberType || x.MemberType == null);
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
                    var contactQuery = query.OfType<dataModel.Contact>().Where(x => x.FullName.Contains(criteria.Keyword) || x.Emails.Any(y => y.Address.Contains(criteria.Keyword))).Select(x => x.Id).ToArray();
                    var orgQuery = query.OfType<dataModel.Organization>().Where(x => x.Name.Contains(criteria.Keyword)).Select(x => x.Id).ToArray();
                    var ids = contactQuery.Concat(orgQuery).ToArray();
                    query = query.Where(x => ids.Contains(x.Id));
                }

                retVal.TotalCount = query.Count();

                var sortInfos = criteria.SortInfos;
                if (sortInfos.IsNullOrEmpty())
                {
                    sortInfos = new[] { new SortInfo { SortColumn = "Name" } };
                }
                //Workaround - need display organization first (OrderByDescending(x => x.MemberType))
                query = query.OrderByDescending(x => x.MemberType).ThenBySortInfos(sortInfos);

                retVal.Members = query.Skip(criteria.Skip)
                                      .Take(criteria.Take)
                                      .ToArray()
                                      .Select(x => ConvertToMember(x))
                                      .ToList();            
                return retVal;
            }
        }
        #endregion

        /// <summary>
        /// Data type factory methods. This methods can be override to extend exist member type to new
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        protected virtual dataModel.Member ConvertToDataMember(Member member, PrimaryKeyResolvingMap pkMap)
        {
            dataModel.Member retVal = null;
            var contact = member as Contact;
            var org = member as Organization;

            if (contact != null)
            {
                retVal = contact.ToDataModel(pkMap);
            }
            if (org != null)
            {
                retVal = org.ToDataModel(pkMap);
            }
            return retVal;
        }

        protected virtual Member ConvertToMember(dataModel.Member dbMember)
        {
            Member retVal = null;
            var dbContact = dbMember as dataModel.Contact;
            var dbOrg = dbMember as dataModel.Organization;

            if (dbContact != null)
            {
                var contact = dbContact.ToCoreModel();
                //Load all security accounts associated with this contact
                var result = Task.Run(() => _securityService.SearchUsersAsync(new UserSearchRequest { MemberId = contact.Id, TakeCount = int.MaxValue })).Result;
                contact.SecurityAccounts = result.Users.ToList();
                retVal = contact;
            }

            if (dbOrg != null)
            {
                retVal = dbOrg.ToCoreModel();
            }
            
            return retVal;
        }
    }
}
