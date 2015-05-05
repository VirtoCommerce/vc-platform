using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Domain.Customer.Services;
using Zapier.IntegrationModule.Web.Providers.Interfaces;

namespace Zapier.IntegrationModule.Web.Providers.Implementations
{
    public class ContactsProvider: IContactsProvider
    {
        private readonly ICustomerSearchService _contactService;

        public ContactsProvider(ICustomerSearchService contactService)
        {
            _contactService = contactService;
        }


        public IEnumerable<Contact> GetNewContacts()
        {
            return _contactService.Search(new SearchCriteria()).Contacts.OrderByDescending(c => c.CreatedDate);
        }
    }
}