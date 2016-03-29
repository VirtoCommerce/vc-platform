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
    public class DefaultMemberService : ServiceBase, IMemberService
    {
        private readonly Func<ICustomerRepository> _repositoryFactory;
        private readonly IDynamicPropertyService _dynamicPropertyService;
        private readonly ISecurityService _securityService;
        private Type[] _knownMemberTypes = new Type []{ typeof(Organization), typeof(Contact), typeof(Employee), typeof(Vendor) };
        public DefaultMemberService(Func<ICustomerRepository> repositoryFactory, IDynamicPropertyService dynamicPropertyService, ISecurityService securityService)
        {
            _repositoryFactory = repositoryFactory;
            _dynamicPropertyService = dynamicPropertyService;
            _securityService = securityService;
        }
         

        #region IMemberService Members

        public virtual Member TryCreateMember(string memberType)
        {
            Member retVal = null;
            var knownMemberType = _knownMemberTypes.FirstOrDefault(x => string.Equals(x.Name, memberType, StringComparison.OrdinalIgnoreCase));
            if (knownMemberType != null)
            {
                retVal = Activator.CreateInstance(knownMemberType) as Member;
            }
            return retVal;
        }
 
        public virtual Member[] GetByIds(params string[] memberIds)
        {
            var retVal = new List<Member>();
            using (var repository = _repositoryFactory())
            {
                var dataMembers = repository.GetMembersByIds(memberIds);
                foreach (var dataMember in dataMembers)
                {
                   var member = ToCoreMember(dataMember);
                    //Load security accounts for members which support them 
                    if(member is IHasSecurityAccounts)
                    {
                        //Load all security accounts associated with this contact
                        var result = Task.Run(() => _securityService.SearchUsersAsync(new UserSearchRequest { MemberId = member.Id, TakeCount = int.MaxValue })).Result;
                        ((IHasSecurityAccounts)member).SecurityAccounts.AddRange(result.Users);
                    }
                    if (member != null)
                    {
                        retVal.Add(member);
                    }
                }
            }
            foreach (var member in retVal)
            {
                //Load dynamic properties for member
                _dynamicPropertyService.LoadDynamicPropertyValues(member);
            }
            return retVal.ToArray();
        }

        public virtual void CreateOrUpdate(Member[] members)
        {
            var pkMap = new PrimaryKeyResolvingMap();
        
            using (var repository = _repositoryFactory())
            using (var changeTracker = GetChangeTracker(repository))
            {
                var dataExistMembers = repository.GetMembersByIds(members.Where(x => !x.IsTransient()).Select(x => x.Id).ToArray());
                foreach (var member in members)
                {
                    var dataSourceMember = ToDataMember(member, pkMap);
                    if (dataSourceMember != null)
                    {
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
                _dynamicPropertyService.SaveDynamicPropertyValues(member);
            }
        }

        public virtual void Delete(string[] ids)
        {
            using (var repository = _repositoryFactory())
            {
                var members = GetByIds(ids);
                if (!members.IsNullOrEmpty())
                {
                    repository.RemoveMembersByIds(members.Select(x => x.Id).ToArray());
                    CommitChanges(repository);
                    foreach (var member in members)
                    {
                        _dynamicPropertyService.DeleteDynamicPropertyValues(member);
                    }
                }
            }
        }

        public virtual MembersSearchResult SearchMembers(MembersSearchCriteria criteria)
        {
            var retVal = new MembersSearchResult();

            using (var repository = _repositoryFactory())
            {
                var query = repository.Members;
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
                    query = query.Where(x => x.Name.Contains(criteria.Keyword) || x.Emails.Any(y => y.Address.Contains(criteria.Keyword)));
                }

                retVal.TotalCount = query.Count();

                var sortInfos = criteria.SortInfos;
                if (sortInfos.IsNullOrEmpty())
                {
                    sortInfos = new[] { new SortInfo { SortColumn = ReflectionUtility.GetPropertyName<Member>(x => x.MemberType), SortDirection = SortDirection.Descending } };
                }
                query = query.OrderBySortInfos(sortInfos);

                retVal.Members = query.Skip(criteria.Skip).Take(criteria.Take).ToArray()
                                      .Select(x => ToCoreMember(x)).ToList();
                return retVal;
            }
        }
        #endregion

        protected virtual dataModel.Member ToDataMember(Member member, PrimaryKeyResolvingMap pkMap)
        {
            var contact = member as Contact;
            var employee = member as Employee;
            var organization = member as Organization;
            var vendor = member as Vendor;

            dataModel.Member retVal = null;
            if (contact != null)
            {
                retVal = new dataModel.Contact();
                contact.ToDataModel((dataModel.Contact)retVal, pkMap);
            }
            else if (employee != null)
            {
                retVal = new dataModel.Employee();
                employee.ToDataModel((dataModel.Employee)retVal, pkMap);
            }
            else if (organization != null)
            {
                retVal = new dataModel.Organization();
                organization.ToDataModel((dataModel.Organization)retVal, pkMap);
            }
            else if (vendor != null)
            {
                retVal = new dataModel.Vendor();
                vendor.ToDataModel((dataModel.Vendor)retVal, pkMap);
            }

            member.ToDataModel(retVal);
            return retVal;
        }

        protected virtual Member ToCoreMember(dataModel.Member dbMember)
        {
            var dbContact = dbMember as dataModel.Contact;
            var dbEmployee = dbMember as dataModel.Employee;
            var dbOrganization = dbMember as dataModel.Organization;
            var dbVendor = dbMember as dataModel.Vendor;

            Member retVal = null;
            if (dbContact != null)
            {
                retVal = new Contact();
                dbContact.ToCoreModel((Contact)retVal);
            }
            else if (dbEmployee != null)
            {
                retVal = new Employee();
                dbEmployee.ToCoreModel((Employee)retVal);
            }
            else if (dbOrganization != null)
            {
                retVal = new Organization();
                dbOrganization.ToCoreModel((Organization)retVal);
            }
            else if (dbVendor != null)
            {
                retVal = new Vendor();
                dbVendor.ToCoreModel((Vendor)retVal);
            }

            dbMember.ToCoreModel(retVal);

            return retVal;
        }
    }
}
