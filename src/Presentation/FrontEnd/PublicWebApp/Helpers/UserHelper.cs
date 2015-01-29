using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.ApiClient.DataContracts.Security;
using VirtoCommerce.Web.Converters;
using VirtoCommerce.Web.Models;

namespace VirtoCommerce.Web.Helpers
{
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

        public static OrderAddressModel GetShippingBillingForCustomer(AuthInfo user)
        {
            var retVal = new OrderAddressModel { CurrentUser = user.ToWebModel() };

            if (user != null && user.Addresses.Length > 0)
            {
                retVal.BillingAddress = user.Addresses.FirstOrDefault(x => x.Name.Contains(DefaultBilling));
                retVal.ShippingAddress = user.Addresses.FirstOrDefault(x => x.Name.Contains(DefaultShipping));
            }

            retVal.OthersAddresses = new Address[] { };
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
    }
}