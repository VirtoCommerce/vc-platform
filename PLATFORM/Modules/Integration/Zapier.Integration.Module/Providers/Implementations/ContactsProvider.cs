using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.Domain.Customer.Model;
using VirtoCommerce.Domain.Customer.Services;
using Zapier.IntegrationModule.Web.Providers.Interfaces;

namespace Zapier.IntegrationModule.Web.Providers.Implementations
{
    public class ContactsProvider: IContactsProvider
    {
        private readonly IMemberService _memberService;
        public ContactsProvider(IMemberService memberService)
        {
            _memberService = memberService;
        }


        public IEnumerable<Contact> GetNewContacts()
        {
            var searchCrit = new SearchCriteria
            {
                MemberType = "Contact",
                Take = 100,
                Sort = "CreatedDate:desc"
            };

            return _memberService.SearchMembers(searchCrit).Members.OfType<Contact>();
          
        }

        public Contact NewContact(Contact newContact)
        {
            _memberService.CreateOrUpdate(new[] { newContact });
            return _memberService.GetByIds(new[] { newContact.Id }).OfType<Contact>().FirstOrDefault();
        }
    }
}