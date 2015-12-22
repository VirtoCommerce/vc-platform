using System.Linq;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Model;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.Storefront.Converters
{
    public static class CustomerConverter
    {
        private static readonly char[] _nameSeparator = { ' ' };

        public static Customer ToWebModel(this VirtoCommerceCustomerModuleWebModelContact contact, string userName)
        {
            var customer = new Customer();
            customer.InjectFrom(contact);
            customer.UserName = userName;

            if (contact.Addresses != null)
            {
                customer.Addresses = contact.Addresses.Select(a => a.ToWebModel()).ToList();
            }

            customer.DefaultBillingAddress = customer.Addresses.FirstOrDefault(a => (a.Type & AddressType.Billing) == AddressType.Billing);
            customer.DefaultShippingAddress = customer.Addresses.FirstOrDefault(a => (a.Type & AddressType.Shipping) == AddressType.Shipping);

            // TODO: Need separate properties for first, middle and last name
            if (!string.IsNullOrEmpty(contact.FullName))
            {
                var nameParts = contact.FullName.Split(_nameSeparator, 2);

                if (nameParts.Length > 0)
                {
                    customer.FirstName = nameParts[0];
                }

                if (nameParts.Length > 1)
                {
                    customer.LastName = nameParts[1];
                }
            }

            if (contact.Emails != null)
            {
                customer.Email = contact.Emails.FirstOrDefault();
            }

            return customer;
        }
    }
}