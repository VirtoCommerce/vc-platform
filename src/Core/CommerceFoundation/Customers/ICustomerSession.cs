using System;
using VirtoCommerce.Foundation.Frameworks.Tagging;
namespace VirtoCommerce.Foundation.Customers
{
    /// <summary>
    /// Holds current state of customer session
    /// </summary>
    public interface ICustomerSession
    {
        /// <summary>
        /// Gets or sets the catalog id. The front end can only have one catalog associated with it.
        /// </summary>
        /// <value>
        /// The catalog id.
        /// </value>
        string CatalogId { get; set; }

        /// <summary>
        /// Gets or sets the category id.
        /// </summary>
        /// <value>
        /// The category id.
        /// </value>
		string CategoryId { get; set; }


        /// <summary>
        /// Gets or sets the category outline.
        /// </summary>
        /// <value>
        /// The category outline.
        /// </value>
        string CategoryOutline { get; set; }

        /// <summary>
        /// Gets or sets the customer id. Corresponds to MemberId used throughout the application and by various modules.
        /// </summary>
        /// <value>
        /// The customer id.
        /// </value>
        string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the username, used by customer to login. Typically is the same as HttpContext.Current.User.Identity.Name, but in some CMS systems can be different.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        string Username { get; set; }

        /// <summary>
        ///  Gets or sets the username, used by customer service representative.
        /// </summary>
        string CsrUsername { get; set; }

        /// <summary>
        /// Gets or sets the name of the customer. Used to display the name of the currently logged in customer.
        /// </summary>
        /// <value>
        /// The name of the customer.
        /// </value>
        string CustomerName { get; set; }

        /// <summary>
        /// Gets the customer tag set. These tags can be used by marketing and other systems to segment customers.
        /// </summary>
        /// <returns>Collection of tags</returns>
        TagSet GetCustomerTagSet();

        /// <summary>
        /// Gets or sets the pricelists for the customer.
        /// </summary>
        /// <value>
        /// The pricelists.
        /// </value>
        string[] Pricelists { get; set; }

        /// <summary>
        /// Gets or sets the current store id.
        /// </summary>
        /// <value>
        /// The store id.
        /// </value>
        string StoreId { get; set; }

        /// <summary>
        /// Gets or sets the name of the store.
        /// </summary>
        /// <value>
        /// The name of the store.
        /// </value>
        string StoreName { get; set; }

        /// <summary>
        /// Gets or sets the current date time. Used during marketing calculations and so on.
        /// </summary>
        /// <value>
        /// The current date time.
        /// </value>
        DateTime CurrentDateTime { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> with the specified key.
        /// </summary>
        /// <value>
        /// The <see cref="System.Object"/>.
        /// </value>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        object this[string key] { get; set; }

        /// <summary>
        /// Gets or sets the currency.
        /// </summary>
        /// <value>
        /// The currency.
        /// </value>
        string Currency { get; set; }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>
        /// The language.
        /// </value>
        string Language { get; set; }

        /// <summary>
        /// Gets or sets the coupon code.
        /// </summary>
        /// <value>
        /// The coupon code.
        /// </value>
        string CouponCode { get; set; }

        /// <summary>
        /// Gets or sets the last shopping page.
        /// </summary>
        /// <value>
        /// The last shopping page.
        /// </value>
        string LastShoppingPage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether customer is registered. Authenticated customer is always registered.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is registered; otherwise, <c>false</c>.
        /// </value>
        bool IsRegistered { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this customer is first time buyer.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is first time buyer; otherwise, <c>false</c>.
        /// </value>
        bool IsFirstTimeBuyer { get; set; }

        /// <summary>
        /// Gets or sets the last order identifier.
        /// </summary>
        /// <value>
        /// The last order identifier.
        /// </value>
        string LastOrderId { get; set; }	
    }
}
