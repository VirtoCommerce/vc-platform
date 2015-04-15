#region

using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.ApiClient;
using VirtoCommerce.ApiClient.DataContracts.CustomerService;
using VirtoCommerce.ApiClient.DataContracts.Orders;
using VirtoCommerce.ApiClient.Extensions;
using VirtoCommerce.Web.Models.Convertors;
using VirtoCommerce.Web.Models.Security;
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

            var userInfo = await this._securityClient.GetUserInfo(email);

            var contact = await this.GetContactAsync(email);

            if (contact != null)
            {
                customer = contact.AsWebModel();
            }

            return customer;
        }

        public async Task<CustomerOrder> GetOrderAsync(string storeId, string customerId, string orderNumber)
        {
            CustomerOrder customerOrder = null;

            var order = await this._orderClient.GetCustomerOrderAsync(customerId, orderNumber);

            if (order != null)
            {
                customerOrder = order.AsWebModel();
            }

            return customerOrder;
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

        public async Task<Customer> CreateCustomerAsync(string email, string firstName, string lastName, string id, ICollection<CustomerAddress> addresses)
        {
            var contact = new Contact { FullName = string.Format("{0} {1}", firstName, lastName) };

            contact.Emails = new List<string> { email };

            if (addresses != null)
            {
                contact.Addresses = new List<Address>();

                foreach (var address in addresses)
                {
                    contact.Addresses.Add(address.AsServiceModel());
                }
            }

            contact.Id = id;
            contact = await this._customerClient.CreateContactAsync(contact);

            //var authInfo = await this._securityClient.GetUserInfo(email);

            //contact.Id = authInfo.Id;

            return contact.AsWebModel();
        }
        #endregion

        #region Methods
        private async Task<Contact> GetContactAsync(string email)
        {
            Contact contact = null;

            var userInfo = await this._securityClient.GetUserInfo(email);

            if (userInfo != null)
            {
                contact = await this._customerClient.GetContactByIdAsync(userInfo.Id);
            }

            return contact;
        }
        #endregion
    }
}