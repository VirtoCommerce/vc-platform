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
    public class ContactMemberServiceImpl : ServiceBase, IMemberService
    {
        private readonly Func<ICustomerRepository> _repositoryFactory;
        private readonly ISecurityService _securityService;
        private readonly IDynamicPropertyService _dynamicPropertyService;
        public ContactMemberServiceImpl(Func<ICustomerRepository> repositoryFactory, ISecurityService securityService, IDynamicPropertyService dynamicPropertyService)
        {
            _repositoryFactory = repositoryFactory;
            _securityService = securityService;
            _dynamicPropertyService = dynamicPropertyService;
        }

        #region IMemberService Members

        public Member[] GetByIds(params string[] memberIds)
        {
            var retVal = new List<Contact>();
            using (var repository = _repositoryFactory())
            {
                var dbContacts = repository.GetMembersByIds(memberIds).OfType<dataModel.Contact>();
                foreach(var dbContact in dbContacts)
                {
                    var contact = dbContact.ToCoreModel();
                    //Load all security accounts associated with this contact
                    var result = Task.Run(() => _securityService.SearchUsersAsync(new UserSearchRequest { MemberId = contact.Id, TakeCount = int.MaxValue })).Result;
                    contact.SecurityAccounts = result.Users.ToList();
                    retVal.Add(contact);
                }
            }
            foreach (var contact in retVal)
            {
                //Load dynamic properties for member
                _dynamicPropertyService.LoadDynamicPropertyValues(contact);
            }

            return retVal.ToArray();
        }

        public void CreateOrUpdate(Member[] members)
        {
            var pkMap = new PrimaryKeyResolvingMap();
            var contacts = members.OfType<Contact>();

            using (var repository = _repositoryFactory())
            using (var changeTracker = GetChangeTracker(repository))
            {
                var dbExistsContacts = repository.GetMembersByIds(contacts.Where(x => !x.IsTransient()).Select(x => x.Id).ToArray()).OfType<dataModel.Contact>();
                foreach (var contact in contacts)
                {
                    var dbSourceContact = contact.ToDataModel(pkMap);
                    var dbTargetContact = dbExistsContacts.FirstOrDefault(x => x.Id == contact.Id);
                    if (dbTargetContact != null)
                    {
                        changeTracker.Attach(dbTargetContact);
                        dbSourceContact.Patch(dbTargetContact);
                    }
                    else
                    {
                        repository.Add(dbSourceContact);
                    }
                }
                CommitChanges(repository);
                pkMap.ResolvePrimaryKeys();
            }
            //Save dynamic properties
            foreach (var contact in contacts)
            {
                _dynamicPropertyService.SaveDynamicPropertyValues(contact);
            }

        }

        public void Delete(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                var contacts = GetByIds(ids);
                repository.RemoveMembersByIds(contacts.Select(x=>x.Id).ToArray());
                CommitChanges(repository);
                foreach(var contact in contacts)
                {
                    _dynamicPropertyService.DeleteDynamicPropertyValues(contact);
                }
            }
        }


        public SearchResult SearchMembers(SearchCriteria criteria)
        {
            var retVal = new SearchResult();

            using (var repository = _repositoryFactory())
            {
                var query = repository.Contacts;
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
                    query = query.Where(x => x.FullName.Contains(criteria.Keyword) || x.Emails.Any(y => y.Address.Contains(criteria.Keyword)));
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
