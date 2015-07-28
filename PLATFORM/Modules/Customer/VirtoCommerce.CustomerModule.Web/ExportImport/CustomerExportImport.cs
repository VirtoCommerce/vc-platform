using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.ExportImport;

namespace VirtoCommerce.CustomerModule.Web.ExportImport
{
    public sealed class BackupObject
    {
        public Contact[] Contacts { get; set; }
        public Organization[] Organizations { get; set; }
    }

    public sealed class CustomerExportImport
    {
        private readonly IContactService _contactService;
        private readonly ICustomerSearchService _customerSearchService;
        private readonly IOrganizationService _organizationService;

        public CustomerExportImport(IContactService contactService, IOrganizationService organizationService, ICustomerSearchService customerSearchService)
        {
            _contactService = contactService;
            _organizationService = organizationService;
            _customerSearchService = customerSearchService;
        }

        public void DoExport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(prodgressInfo);

            var backupObject = GetBackupObject();
            backupObject.SerializeJson(backupStream);
        }

        public void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(prodgressInfo);

            var backupObject = backupStream.DeserializeJson<BackupObject>();
            var originalObject = GetBackupObject();

            UpdateOrganizations(originalObject.Organizations, backupObject.Organizations);
            UpdateContacts(originalObject.Contacts, backupObject.Contacts);

        }

        #region Import updates
        
        private void UpdateOrganizations(ICollection<Organization> original, ICollection<Organization> backup)
        {
            var organizationsToUpdate = new List<Organization>();
            backup.CompareTo(original, EqualityComparer<Organization>.Default, (state, x, y) =>
            {
                switch (state)
                {
                    case EntryState.Modified:
                        organizationsToUpdate.Add(x);
                        break;
                    case EntryState.Added:
                        _organizationService.Create(x);
                        break;
                }
            });
            _organizationService.Update(organizationsToUpdate.ToArray());
        }

        private void UpdateContacts(ICollection<Contact> original, ICollection<Contact> backup)
        {
            var contactsToUpdate = new List<Contact>();
            backup.CompareTo(original, EqualityComparer<Contact>.Default, (state, x, y) =>
            {
                switch (state)
                {
                    case EntryState.Modified:
                        contactsToUpdate.Add(x);
                        break;
                    case EntryState.Added:
                        _contactService.Create(x);
                        break;
                }
            });
            _contactService.Update(contactsToUpdate.ToArray());
        }

        #endregion

        #region BackupObject

        public BackupObject GetBackupObject()
        {
            var rootOrganization = GetOrganizations(null);
            var organizations = rootOrganization != null ? rootOrganization.Traverse(ChildrenForOrganization).ToArray() : null;

            var result = new BackupObject();
            if (organizations != null)
            {
                result.Contacts = organizations.SelectMany(x => x.Contacts).ToArray();
                result.Organizations = organizations.SelectMany(x => x.Organizations).ToArray();
            }

            return result;
        }

        private IEnumerable<SearchResult> ChildrenForOrganization(SearchResult result)
        {
            return result != null && result.Organizations != null
                ? result.Organizations.Select(x => GetOrganizations(x.Id))
                : null;
        }

        private SearchResult GetOrganizations(string organizationId)
        {
            return _customerSearchService.Search(new SearchCriteria { Count = int.MaxValue, OrganizationId = organizationId });
        }

        #endregion

    }
}