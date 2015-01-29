using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VirtoCommerce.ApiClient.DataContracts.Security;

namespace VirtoCommerce.Web.Models
{
    public class OrderAddressModel
    {
        /// <summary>
        /// Gets or sets the billing address.
        /// </summary>
        /// <value>The billing address.</value>
        [Display(Name = "Billing Address")]
        public Address BillingAddress { get; set; }

        /// <summary>
        /// Gets or sets the shipping address.
        /// </summary>
        /// <value>The shipping address.</value>
        [Display(Name = "Shipping Address")]
        public Address ShippingAddress { get; set; }

        /// <summary>
        /// Gets or sets the others addresses.
        /// </summary>
        /// <value>The others addresses.</value>
        public Address[] OthersAddresses { get; set; }

        /// <summary>
        /// Gets or sets the current user.
        /// </summary>
        /// <value>The current user.</value>
        public CustomerModel CurrentUser { get; set; }
    }

    public class CustomerModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}