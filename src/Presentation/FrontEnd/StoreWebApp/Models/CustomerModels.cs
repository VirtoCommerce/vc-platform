using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Orders.Model.Countries;
using VirtoCommerce.Foundation.Security.Model;

namespace VirtoCommerce.Web.Models
{
	/// <summary>
	/// Class CustomerModel.
	/// </summary>
    public class CustomerModel
    {
		/// <summary>
		/// Gets or sets the member identifier.
		/// </summary>
		/// <value>The member identifier.</value>
        public string MemberId { get; set; }
		/// <summary>
		/// Gets or sets the full name.
		/// </summary>
		/// <value>The full name.</value>
        public string FullName { get; set; }

		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		/// <value>The email.</value>
        [EmailAddress(ErrorMessage = "Enter valid email")]
        public string Email { get; set; }

		/// <summary>
		/// Gets or sets the birth date.
		/// </summary>
		/// <value>The birth date.</value>
        public DateTime? BirthDate { get; set; }

		/// <summary>
		/// Gets or sets the taxpayer identifier.
		/// </summary>
		/// <value>The taxpayer identifier.</value>
        public string TaxpayerId { get; set; }

		/// <summary>
		/// Gets or sets the preferred delivery.
		/// </summary>
		/// <value>The preferred delivery.</value>
        public string PreferredDelivery { get; set; }

		/// <summary>
		/// Gets or sets the preferred communication.
		/// </summary>
		/// <value>The preferred communication.</value>
        public string PreferredCommunication { get; set; }

		/// <summary>
		/// Gets or sets the photo URL.
		/// </summary>
		/// <value>The photo URL.</value>
        public string PhotoUrl { get; set; }
		/// <summary>
		/// Gets or sets the created.
		/// </summary>
		/// <value>The created.</value>
        public DateTime Created { get; set; }
    }

	/// <summary>
	/// Class OrderAddressModel.
	/// </summary>
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

	/// <summary>
	/// Class CompanyAddressModel.
	/// </summary>
    public class CompanyAddressModel
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
		/// Gets or sets the current organization.
		/// </summary>
		/// <value>The current organization.</value>
        public Organization CurrentOrganization { get; set; }
    }

	/// <summary>
	/// Class AddressModel.
	/// </summary>
	public class AddressModel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AddressModel"/> class.
		/// </summary>
		public AddressModel()
		{
			//Default Values
			AddressId = Guid.NewGuid().ToString();
			//Name = String.Format("{0}{1}", UserHelper.DefaultBilling, UserHelper.DefaultShipping);
			CountryName = "United States";
		}
		/// <summary>
		/// Gets or sets the address identifier.
		/// </summary>
		/// <value>The address identifier.</value>
		public string AddressId { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		[Required(ErrorMessage = "Address Name can't be empty")]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		/// <value>The first name.</value>
		[Display(Name = "First Name")]
		[Required(ErrorMessage = "First name is required")]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the last name.
		/// </summary>
		/// <value>The last name.</value>
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[Display(Name = "Last Name")]
		[Required(ErrorMessage = "Last name is required")]
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the line1.
		/// </summary>
		/// <value>The line1.</value>
		[Required(ErrorMessage = "Street address is required")]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[Display(Name = "Street Address")]
		public string Line1 { get; set; }

		/// <summary>
		/// Gets or sets the line2.
		/// </summary>
		/// <value>The line2.</value>
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string Line2 { get; set; }

		/// <summary>
		/// Gets or sets the city.
		/// </summary>
		/// <value>The city.</value>
		[Required(ErrorMessage = "City can't be empty")]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string City { get; set; }

		/// <summary>
		/// Gets or sets the country code.
		/// </summary>
		/// <value>The country code.</value>
		[Required(ErrorMessage = "Country code can't be empty")]
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
		[Display(Name = "Country")]
		public string CountryCode { get; set; }

		/// <summary>
		/// Gets or sets the state province.
		/// </summary>
		/// <value>The state province.</value>
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[Required]
		[Display(Name = "State")]
		public string StateProvince { get; set; }

		/// <summary>
		/// Gets or sets the name of the country.
		/// </summary>
		/// <value>The name of the country.</value>
		[Required(ErrorMessage = "Country name can't be empty")]
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string CountryName { get; set; }

		/// <summary>
		/// Gets or sets the postal code.
		/// </summary>
		/// <value>The postal code.</value>
		[Required(ErrorMessage = "Zip is required")]
		[StringLength(32, ErrorMessage = "Only 32 characters allowed.")]
		[Display(Name = "Zip Code")]
		public string PostalCode { get; set; }

		/// <summary>
		/// Gets or sets the region identifier.
		/// </summary>
		/// <value>The region identifier.</value>
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string RegionId { get; set; }

		/// <summary>
		/// Gets or sets the name of the region.
		/// </summary>
		/// <value>The name of the region.</value>
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		public string RegionName { get; set; }

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		/// <value>The type.</value>
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
		public string Type { get; set; }

		/// <summary>
		/// Gets or sets the daytime phone number.
		/// </summary>
		/// <value>The daytime phone number.</value>
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
		[Display(Name = "Telephone")]
		[Required]
		public string DaytimePhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets the evening phone number.
		/// </summary>
		/// <value>The evening phone number.</value>
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
		public string EveningPhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets the fax number.
		/// </summary>
		/// <value>The fax number.</value>
		[StringLength(64, ErrorMessage = "Only 64 characters allowed.")]
		[Display(Name = "Fax")]
		public string FaxNumber { get; set; }

		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		/// <value>The email.</value>
		[StringLength(256, ErrorMessage = "Only 256 characters allowed.")]
		[EmailAddress]
		[Required(ErrorMessage = "Email address is required")]
		[Display(Name = "Email Address")]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the organization.
		/// </summary>
		/// <value>The organization.</value>
		[StringLength(128, ErrorMessage = "Only 128 characters allowed.")]
		[Display(Name = "Company")]
		public string Organization { get; set; }

	}

	/// <summary>
	/// Class AddressEditModel.
	/// </summary>
    public class AddressEditModel
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="AddressEditModel"/> class.
		/// </summary>
		public AddressEditModel()
		{
			Address = new AddressModel();
		}

		/// <summary>
		/// The _regions
		/// </summary>
        private CountryRegionScriptModel[] _regions;
		/// <summary>
		/// Gets or sets the countries.
		/// </summary>
		/// <value>The countries.</value>
        public Country[] Countries { get; set; }
		/// <summary>
		/// Gets or sets the address.
		/// </summary>
		/// <value>The address.</value>
		public AddressModel Address { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is default billing.
		/// </summary>
		/// <value><c>true</c> if this instance is default billing; otherwise, <c>false</c>.</value>
        [Display(Name = "Save as Default Billing Address")]
        public bool IsDefaultBilling { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this instance is default shipping.
		/// </summary>
		/// <value><c>true</c> if this instance is default shipping; otherwise, <c>false</c>.</value>
        [Display(Name = "Save as Default Shipping Address")]
        public bool IsDefaultShipping { get; set; }

		/// <summary>
		/// Gets or sets the organization identifier.
		/// </summary>
		/// <value>The organization identifier.</value>
        public string OrganizationId { get; set; }

		/// <summary>
		/// Gets the script country regions.
		/// </summary>
		/// <value>The script country regions.</value>
        public CountryRegionScriptModel[] ScriptCountryRegions
        {
            get
            {
                if (_regions != null)
                {
                    return _regions;
                }

                var regions = from c in Countries
                              select
                                  new CountryRegionScriptModel(c.CountryId,
                                                               (from r in c.Regions
                                                                select new RegionScriptModel(r.RegionId, r.DisplayName))
                                                                   .ToArray());

                _regions = regions.ToArray();

                return _regions;
            }
        }
    }

	/// <summary>
	/// Class CompareListModel.
	/// </summary>
    public class CompareListModel
    {
		/// <summary>
		/// The _line items
		/// </summary>
        private readonly LineItemModel[] _lineItems;
		/// <summary>
		/// The _available properties
		/// </summary>
		private readonly IGrouping<string, PropertyModel>[] _availableProperties;

		/// <summary>
		/// Initializes a new instance of the <see cref="CompareListModel"/> class.
		/// </summary>
		/// <param name="items">The items.</param>
        public CompareListModel(LineItemModel[] items)
        {
            _lineItems = items;

		    if (items != null)
		    {

		        _availableProperties = items.SelectMany(x => x.CatalogItem.Properties)
		                                    .OrderBy(x => x.Priority)
		                                    .GroupBy(x => x.Name).ToArray();
		    }
        }

		/// <summary>
		/// Gets the line items.
		/// </summary>
		/// <value>The line items.</value>
        public LineItemModel[] LineItems
        {
            get { return _lineItems; }
        }

		/// <summary>
		/// Gets the available properties.
		/// </summary>
		/// <value>The available properties.</value>
		public IGrouping<string, PropertyModel>[] AvailableProperties
		{
			get { return _availableProperties; }
		}
    }

	/// <summary>
	/// Class CompanyEditModel.
	/// </summary>
    public class CompanyEditModel
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="CompanyEditModel"/> class.
		/// </summary>
        public CompanyEditModel()
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="CompanyEditModel"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <param name="description">The description.</param>
        public CompanyEditModel(string name, string description)
        {
            Description = description;
            Name = name;
        }

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>The description.</value>
        public string Description { get; set; }
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
        public string Name { get; set; }
    }

	/// <summary>
	/// Class CompanyUserListModel.
	/// </summary>
    public class CompanyUserListModel
    {
		/// <summary>
		/// Gets or sets the users.
		/// </summary>
		/// <value>The users.</value>
        public Account[] Users { get; set; }
		/// <summary>
		/// Gets or sets the current organization.
		/// </summary>
		/// <value>The current organization.</value>
        public Organization CurrentOrganization { get; set; }
    }

	/// <summary>
	/// Class CompanyNewUserModel.
	/// </summary>
    public class CompanyNewUserModel
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="CompanyNewUserModel"/> class.
		/// </summary>
        public CompanyNewUserModel()
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="CompanyNewUserModel"/> class.
		/// </summary>
		/// <param name="u">The u.</param>
        public CompanyNewUserModel(Contact u)
        {
            //this.FirstName = u.FirstName;
            //this.LastName = u.LastName;
            //this.EMail = u.Email;
            UserId = u.MemberId;
            CurrentUser = u;
        }

		/// <summary>
		/// Gets or sets the first name.
		/// </summary>
		/// <value>The first name.</value>
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets the last name.
		/// </summary>
		/// <value>The last name.</value>
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

		/// <summary>
		/// Gets or sets the e mail.
		/// </summary>
		/// <value>The e mail.</value>
        [Required]
        [Display(Name = "Email address")]
        [DataType(DataType.EmailAddress)]
        public string EMail { get; set; }


		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
        [DataType(DataType.Password)]
        public string Password { get; set; }


		/// <summary>
		/// Gets or sets the confirm password.
		/// </summary>
		/// <value>The confirm password.</value>
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        public string ConfirmPassword { get; set; }

		/// <summary>
		/// Gets or sets the user identifier.
		/// </summary>
		/// <value>The user identifier.</value>
        public string UserId { get; set; }

		/// <summary>
		/// Gets or sets the current user.
		/// </summary>
		/// <value>The current user.</value>
        public Contact CurrentUser { get; set; }

		/// <summary>
		/// Gets or sets the user roles.
		/// </summary>
		/// <value>The user roles.</value>
        [Display(Name = "User Roles")]
        public IEnumerable<SelectListItem> UserRoles { get; set; }

		/// <summary>
		/// Gets or sets all roles.
		/// </summary>
		/// <value>All roles.</value>
        [Display(Name = "All Roles")]
        public IEnumerable<SelectListItem> AllRoles { get; set; }

		/// <summary>
		/// Gets or sets the selected user roles.
		/// </summary>
		/// <value>The selected user roles.</value>
        public string SelectedUserRoles { get; set; }

		/// <summary>
		/// Gets the get selected user roles.
		/// </summary>
		/// <value>The get selected user roles.</value>
        public string[] GetSelectedUserRoles
        {
            get
            {
                if (String.IsNullOrEmpty(SelectedUserRoles))
                {
                    return new string[] {};
                }

                var list = new List<string> {""};
                return SelectedUserRoles.Split(';').Except(list).ToArray();
            }
        }
    }

	/// <summary>
	/// Class AssignPermissionModel.
	/// </summary>
    public class AssignPermissionModel
    {
		/// <summary>
		/// Gets or sets the new permission.
		/// </summary>
		/// <value>The new permission.</value>
        public string NewPermission { get; set; }
    }
}