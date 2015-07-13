using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Domain.Customer.Services;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Data.ExportImport;

namespace VirtoCommerce.CustomerModule.Web.ExportImport
{
    public sealed class BackupObject
    {
        public ICollection<Contact> Contacts { get; set; }
        public ICollection<Organization> Organizations { get; set; }
    }

    public sealed class CustomerExportImport : JsonExportImport
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

            var responce = _customerSearchService.Search(new SearchCriteria());
            var backupObject = new BackupObject
            {
                Contacts = responce.Contacts.Select(x => x.Id).Select(_contactService.GetById).ToArray(),
                Organizations = responce.Organizations.Select(x => x.Id).Select(_organizationService.GetById).ToArray()
            };

            Save(backupStream, backupObject, progressCallback, prodgressInfo);
        }

        public void DoImport(Stream backupStream, Action<ExportImportProgressInfo> progressCallback)
        {
            var prodgressInfo = new ExportImportProgressInfo { Description = "loading data..." };
            progressCallback(prodgressInfo);

            var backupObject = Load<BackupObject>(backupStream, progressCallback, prodgressInfo);
            //foreach (var contact in contacts)
            //{
            //    var originalContact = _contactService.GetById(contact.Id);
            //    if (originalContact == null)
            //    {
            //        _contactService.Create(contact);
            //    }
            //    else
            //    {
            //        originalContact.InjectFrom(contact);
            //        _contactService.Update(new[] { originalContact });
            //    }
            //}
        }

    }
}