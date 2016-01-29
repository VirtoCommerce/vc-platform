using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using foundationModel = VirtoCommerce.CustomerModule.Data.Model;
using coreModel = VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Domain.Store.Services;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.CustomerModule.Data.Repositories;
using VirtoCommerce.CustomerModule.Data.Converters;
using System.Data.Entity;
using System.Collections.Concurrent;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.CustomerModule.Data.Services
{
    public class CustomerSearchServiceImpl : ICustomerSearchService
    {
        private readonly Func<ICustomerRepository> _repositoryFactory;
        private Dictionary<string, string> _contactSortingAliases = new Dictionary<string, string>();
        private Dictionary<string, string> _organizationSortingAliases = new Dictionary<string, string>();


        public CustomerSearchServiceImpl(Func<ICustomerRepository> repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
            _contactSortingAliases["displayname"] = ReflectionUtility.GetPropertyName<coreModel.Contact>(x => x.FullName);
            _organizationSortingAliases["displayname"] = ReflectionUtility.GetPropertyName<coreModel.Organization>(x => x.Name);
        }

        #region IContactSearchService Members

        public coreModel.SearchResult Search(coreModel.SearchCriteria criteria)
        {
            var retVal = new coreModel.SearchResult();
            var taskList = new List<Task>();

            taskList.Add(Task.Factory.StartNew(() => SearchOrganizations(criteria, retVal)));
            taskList.Add(Task.Factory.StartNew(() => SearchContacts(criteria, retVal)));

            Task.WaitAll(taskList.ToArray());

            return retVal;
        }

        #endregion

        private void SearchOrganizations(coreModel.SearchCriteria criteria, coreModel.SearchResult result)
        {
            using (var repository = _repositoryFactory())
            {
                var query = repository.Organizations;
                if (criteria.OrganizationId != null)
                {
                    query = query.Where(x => x.MemberRelations.Any(y => y.AncestorId == criteria.OrganizationId));
                    if (!String.IsNullOrEmpty(criteria.Keyword))
                    {
                        query = query.Where(x => x.Name.Contains(criteria.Keyword));
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(criteria.Keyword))
                    {
                        query = query.Where(x => x.Name.Contains(criteria.Keyword));
                    }
                    else
                    {
                        query = query.Where(x => !x.MemberRelations.Any());
                    }
                }

                var sortInfos = criteria.SortInfos;
                if (sortInfos.IsNullOrEmpty())
                {
                    sortInfos = new[] { new SortInfo { SortColumn = "Name" } };
                }
                //Try to replace sorting columns names
                TryTransformSortingInfoColumnNames(_organizationSortingAliases, sortInfos);
                query = query.OrderBySortInfos(sortInfos);


                result.Organizations = query.ToArray()
                                            .Select(x => x.ToCoreModel())
                                            .ToList();
            }
        }

        private void SearchContacts(coreModel.SearchCriteria criteria, coreModel.SearchResult result)
        {
            using (var repository = _repositoryFactory())
            {
                var query = repository.Members.OfType<foundationModel.Contact>();

                if (criteria.OrganizationId != null)
                {
                    query = query.Where(x => x.MemberRelations.Any(y => y.AncestorId == criteria.OrganizationId));
                    if (!String.IsNullOrEmpty(criteria.Keyword))
                    {
                        query = query.Where(x => x.FullName.Contains(criteria.Keyword));
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(criteria.Keyword))
                    {
                        query = query.Where(x => x.FullName.Contains(criteria.Keyword) || x.Emails.Any(y => y.Address.Contains(criteria.Keyword)));
                    }
                    else
                    {
                        query = query.Where(x => !x.MemberRelations.Any());
                    }
                }

                result.TotalCount = query.Count();

                var sortInfos = criteria.SortInfos;
                if (sortInfos.IsNullOrEmpty())
                {
                    sortInfos = new[] { new SortInfo { SortColumn = "FullName" } };
                }
                //Try to replace sorting columns names
                TryTransformSortingInfoColumnNames(_contactSortingAliases, sortInfos);
                query = query.OrderBySortInfos(sortInfos);

                result.Contacts = query.Skip(criteria.Skip)
                                   .Take(criteria.Take)
                                   .ToArray()
                                   .Select(x => x.ToCoreModel())
                                   .ToList();
            }
        }

        private static void TryTransformSortingInfoColumnNames(IDictionary<string, string> transformationMap, SortInfo[] sortingInfos)
        {
            //Try to replace sorting columns names
            foreach (var sortInfo in sortingInfos)
            {
                string newColumnName;
                if (transformationMap.TryGetValue(sortInfo.SortColumn.ToLowerInvariant(), out newColumnName))
                {
                    sortInfo.SortColumn = newColumnName;
                }
            }
        }
    }
}
