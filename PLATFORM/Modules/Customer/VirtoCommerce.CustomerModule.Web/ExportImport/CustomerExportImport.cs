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
        public ICollection<Contact> Contacts { get; set; }
        public ICollection<Organization> Organizations { get; set; }
    }

    public sealed class CustomerExportImport
    {
        private readonly IContactService _contactService;
        private readonly ICustomerSearchService _customerSearchService;
        private readonly IOrganizationService _organizationService;

        public CustomerExportImport(IContactService contactService, ICustomerSearchService customerSearchService, IOrganizationService organizationService)
        {
            _contactService = contactService;
            _customerSearchService = customerSearchService;
            _organizationService = organizationService;
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

        public BackupObject GetBackupObject()
        {
           var responce = _customerSearchService.Search(new SearchCriteria { Count = int.MaxValue });
           return new BackupObject
            {
                Contacts = responce.Contacts.Select(x => x.Id).Select(_contactService.GetById).ToArray(),
                Organizations = responce.Organizations.Select(x => x.Id).Select(_organizationService.GetById).ToArray()
            };
        }

    }
}