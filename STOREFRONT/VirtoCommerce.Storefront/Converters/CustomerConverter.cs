using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Converters
{
    public static class CustomerConverter
    {
        private static readonly char[] _nameSeparator = { ' ' };

        public static Customer ToWebModel(this VirtoCommerceCustomerModuleWebModelContact contact)
        {
            var customer = new Customer();
            customer.InjectFrom(contact);

            if (contact.Addresses != null)
            {
                customer.Addresses = contact.Addresses.Select(a => a.ToWebModel()).ToList();
            }

            customer.DefaultBillingAddress = customer.Addresses.FirstOrDefault(a => (a.AddressType & AddressType.Billing) == AddressType.Billing);
            customer.DefaultShippingAddress = customer.Addresses.FirstOrDefault(a => (a.AddressType & AddressType.Shipping) == AddressType.Shipping);

            // TODO: Need separate properties for first, middle and last name
            if (!string.IsNullOrEmpty(contact.FullName))
            {
                var nameParts = contact.FullName.Split(_nameSeparator, 2);

                customer.FirstName = nameParts[0];
                customer.LastName = nameParts[1];
            }

            if (contact.Emails != null)
            {
                customer.Email = contact.Emails.FirstOrDefault();
            }

            return customer;
        }
    }
}
