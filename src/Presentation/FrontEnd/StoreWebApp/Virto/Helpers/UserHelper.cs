using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Omu.ValueInjecter;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Customers;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Services;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Web.Models;

namespace VirtoCommerce.Web.Virto.Helpers
{
	/// <summary>
	/// Class UserHelper.
	/// </summary>
    public class UserHelper
    {
		/// <summary>
		/// The default billing
		/// </summary>
        public const string DefaultBilling = "DefaultBilling";
		/// <summary>
		/// The default shipping
		/// </summary>
        public const string DefaultShipping = "DefaultShipping";

		/// <summary>
		/// The add to cart action
		/// </summary>
        public const string AddToCartAction = "AllToCart";
		/// <summary>
		/// The default comment in wish list
		/// </summary>
        public static string DefaultCommentInWishList = "Please, enter your comments...";

        #region Cache Constants

		/// <summary>
		/// The user cache key
		/// </summary>
        public const string UserCacheKey = "C:U:{0}:{1}";
		/// <summary>
		/// The membership cache key
		/// </summary>
        public const string MembershipCacheKey = "C:M:{0}:{1}";

        #endregion

        #region User Session

		/// <summary>
		/// Gets the customer session.
		/// </summary>
		/// <value>The customer session.</value>
        public static ICustomerSession CustomerSession
        {
            get
            {
                var session = DependencyResolver.Current.GetService<ICustomerSessionService>();
                return session.CustomerSession;
            }
        }

		/// <summary>
		/// Gets the user client.
		/// </summary>
		/// <value>The user client.</value>
        public static UserClient UserClient
        {
            get { return DependencyResolver.Current.GetService<UserClient>(); }
        }

		/// <summary>
		/// Gets the store client.
		/// </summary>
		/// <value>The store client.</value>
        public static StoreClient StoreClient
        {
            get { return DependencyResolver.Current.GetService<StoreClient>(); }
        }

        #endregion

        #region --- User methods ---

		/// <summary>
		/// Creates customer model from contact.
		/// </summary>
		/// <param name="contact">The contact.</param>
		/// <returns>CustomerModel.</returns>
        public static CustomerModel GetCustomerModel(Contact contact)
        {
            if (contact == null)
            {
                contact = new Contact();
            }

            var model = new CustomerModel();

            model.InjectFrom(contact);
            var email = contact.Emails.FirstOrDefault(e => e.Type == EmailType.Primary.ToString());
            if (email != null)
            {
                model.Email = email.Address;
            }

            return model;
        }

        #region GetCurrentUserContract
		/// <summary>
		/// Gets the current user contract.
		/// </summary>
		/// <returns>Contract.</returns>
        public static Contract GetCurrentUserContract()
        {
            var user = UserClient.GetCurrentCustomer();
			if (user == null)
				return null;

			if (user.Contracts != null && user.Contracts.Count > 0)
			{
				return user.Contracts[0];
			}
            return null;
        }
        #endregion

		/// <summary>
		/// Gets all customer addresses from address book and from company address book if available
		/// </summary>
		/// <returns>List{Address}.</returns>
        public static List<Address> GetAllCustomerAddresses()
        {
            var allAddress = new List<Address>();

            var arr = UserClient.GetUserAddresses();

            if (arr != null)
            {
                allAddress.AddRange(arr);
            }

            var org = UserClient.GetOrganizationForCurrentUser();

            if (org != null)
            {
                allAddress.AddRange(org.Addresses.ToList());
            }

            return allAddress;
        }

        #region GetShippingBillingForCustomer

		/// <summary>
		/// Creates OrderAddressModel from given Contact
		/// </summary>
		/// <param name="user">The contact</param>
		/// <returns>OrderAddressModel.</returns>
        public static OrderAddressModel GetShippingBillingForCustomer(Contact user)
        {
            var retVal = new OrderAddressModel {CurrentUser = GetCustomerModel(user)};

            if (user != null && user.Addresses.Count > 0)
            {
                retVal.BillingAddress = user.Addresses.FirstOrDefault(x => x.Name.Contains(DefaultBilling));
                retVal.ShippingAddress = user.Addresses.FirstOrDefault(x => x.Name.Contains(DefaultShipping));
            }

            retVal.OthersAddresses = new Address[] {};
            var allOthers = new List<Address>();
            if (user != null)
            {
                allOthers.AddRange(
                    user.Addresses.Where(
                        addr => !addr.Name.Contains(DefaultShipping) && !addr.Name.Contains(DefaultBilling)));
            }

            retVal.OthersAddresses = allOthers.ToArray();
            return retVal;
        }

        #endregion

        #region --- Company methods ---

		/// <summary>
		/// Gets company address model
		/// </summary>
		/// <param name="currentOrganization">The current organization.</param>
		/// <returns>CompanyAddressModel.</returns>
        public static CompanyAddressModel GetShippingBillingForOrganization(Organization currentOrganization)
        {
            var retVal = new CompanyAddressModel {CurrentOrganization = currentOrganization};

            if (currentOrganization != null)
            {
                if (currentOrganization.Addresses.Count > 0)
                {
                    retVal.BillingAddress =
                        currentOrganization.Addresses.FirstOrDefault(x => x.Name.Contains(DefaultBilling));
                    retVal.ShippingAddress =
                        currentOrganization.Addresses.FirstOrDefault(x => x.Name.Contains(DefaultShipping));
                }

                retVal.OthersAddresses = currentOrganization.Addresses.Where(addr =>
                                                                             !addr.Name.Contains(DefaultShipping) &&
                                                                             !addr.Name.Contains(DefaultBilling))
                                                            .ToArray();
            }

            return retVal;
        }

        #endregion

        #endregion
    }
}