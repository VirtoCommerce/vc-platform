#region

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.DataContracts.CustomerService;
using VirtoCommerce.ApiClient.DataContracts.Orders;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.Web.Convertors;
using VirtoCommerce.Web.Models.Security;
using VirtoCommerce.Web.Models;
using Address = VirtoCommerce.ApiClient.DataContracts.CustomerService.Address;

#endregion

namespace VirtoCommerce.Web.Models.Services
{
    public class CustomerService
    {
        #region Fields
        private readonly CustomerServiceClient _customerClient;

        private readonly OrderClient _orderClient;

        private readonly SecurityClient _securityClient;
        #endregion

        #region Constructors and Destructors
        public CustomerService()
        {
            this._customerClient = ClientContext.Clients.CreateCustomerServiceClient();
            this._securityClient = ClientContext.Clients.CreateSecurityClient();
            this._orderClient = ClientContext.Clients.CreateOrderClient();
        }
        #endregion

        #region Public Methods and Operators
        public async Task<Customer> GetCustomerAsync(string email, string storeId)
        {
            Customer customer = null;

            var contact = await this.GetContactAsync(email);

            if (contact != null)
            {
                customer = contact.AsWebModel();
            }

            return customer;
        }

        public async Task<VirtoCommerce.ApiClient.DataContracts.Orders.CustomerOrder> GetOrderAsync(
            string storeId, string customerId, string orderNumber)
        {
            return await this._orderClient.GetCustomerOrderAsync(customerId, orderNumber);
        }

        public async Task<OrderSearchResult> GetOrdersAsync(
            string storeId,
            string customerId,
            string query,
            int skip,
            int take)
        {
            var response = await this._orderClient.GetCustomerOrdersAsync(storeId, customerId, query, skip, take);

            return response;
        }

        public async Task UpdateOrderAsync(
            VirtoCommerce.ApiClient.DataContracts.Orders.CustomerOrder order)
        {
            await _orderClient.UpdateOrderAsync(order);
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            var contact = await this.GetContactAsync(customer.Email);

            contact.Addresses = new List<Address>();

            foreach (var customerAddress in customer.Addresses)
            {
                contact.Addresses.Add(customerAddress.AsServiceModel());
            }

            await this._customerClient.UpdateContactAsync(contact);
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            Contact contact = null;

            if (string.IsNullOrEmpty(customer.FirstName) && string.IsNullOrEmpty(customer.LastName))
            {
                contact = new Contact { FullName = customer.Email };
            }
            else
            {
                contact = new Contact { FullName = string.Format("{0} {1}", customer.FirstName, customer.LastName) };
            }

            contact.Emails = new List<string> { customer.Email };

            if (customer.Addresses != null)
            {
                contact.Addresses = new List<Address>();

                foreach (var address in customer.Addresses)
                {
                    contact.Addresses.Add(address.AsServiceModel());
                }
            }

            contact.Id = customer.Id;
            contact.DynamicProperties = customer.DynamicProperties.Select(p => p.ToServiceModel()).ToArray();
            contact = await this._customerClient.CreateContactAsync(contact);

            return contact.AsWebModel();
        }
        #endregion

        #region Methods
        private async Task<Contact> GetContactAsync(string email)
        {
            Contact contact = null;

            var user = await this._securityClient.FindUserByNameAsync(email);

            if (user != null)
            {
                contact = await this._customerClient.GetContactByIdAsync(user.Id);
            }

            return contact;
        }
        #endregion
    }
}