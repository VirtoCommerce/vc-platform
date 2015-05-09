using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Domain.Customer.Services;
using Zapier.IntegrationModule.Web.Providers.Interfaces;

namespace Zapier.IntegrationModule.Web.Providers.Implementations
{
    public class ContactsProvider: IContactsProvider
    {
        private readonly ICustomerSearchService _customerSearchService;
        private readonly IContactService _contactService;

        public ContactsProvider(ICustomerSearchService customerSearchService, IContactService contactService)
        {
            _customerSearchService = customerSearchService;
            _contactService = contactService;
        }


        public IEnumerable<Contact> GetNewContacts()
        {
            return _customerSearchService.Search(new SearchCriteria()).Contacts.OrderByDescending(c => c.CreatedDate);
        }

        public Contact NewContact(Contact newContact)
        {
            return _contactService.Create(newContact);
        }
    }
}