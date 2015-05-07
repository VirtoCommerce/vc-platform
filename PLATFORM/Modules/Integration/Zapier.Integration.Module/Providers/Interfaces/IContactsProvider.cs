using System.Collections.Generic;
using VirtoCommerce.Domain.Customer.Model;

namespace Zapier.IntegrationModule.Web.Providers.Interfaces
{
    public interface IContactsProvider
    {
        IEnumerable<Contact> GetNewContacts();
        Contact NewContact(Contact newContact);
    }
}
