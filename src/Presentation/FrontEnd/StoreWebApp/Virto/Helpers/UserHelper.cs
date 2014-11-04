using System.Threading.Tasks;
using System.Web.Http.OData;
using Microsoft.AspNet.Identity.Owin;
using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using VirtoCommerce.Client;
using VirtoCommerce.Foundation.Customers;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Services;
using VirtoCommerce.Foundation.Frameworks.Email;
using VirtoCommerce.Foundation.Frameworks.Templates;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.Web.Client.Services.Emails;
using VirtoCommerce.Web.Client.Services.Security;
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

        public static IdentityUserSecurity UserSecurity
        {
            get { return DependencyResolver.Current.GetService<IdentityUserSecurity>(); }
        }

	    /// <summary>
	    /// Gets the store client.
	    /// </summary>
	    /// <value>The store client.</value>
	    public static StoreClient StoreClient
	    {
	        get { return DependencyResolver.Current.GetService<StoreClient>(); }
	    }


        /// <summary>
        /// Gets the template service.
        /// </summary>
        /// <value>
        /// The template service.
        /// </value>
	    public static ITemplateService TemplateService
        {
            get { return DependencyResolver.Current.GetService<ITemplateService>(); }
        }

        /// <summary>
        /// Gets the email service.
        /// </summary>
        /// <value>
        /// The email service.
        /// </value>
        public static IEmailService EmailService
        {
            get { return DependencyResolver.Current.GetService<IEmailService>(); }
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

        #region Registration

        /// <summary>
        /// Registers the specified user.
        /// </summary>
        /// <param name="model">The registration model.</param>
        /// <param name="requireConfirmation">if set to <c>true</c> [require confirmation].</param>
        /// <returns>
        /// true when user is registered and logged in
        /// </returns>
        public static async Task<RegistrationResult> RegisterAsync(RegisterModel model, bool requireConfirmation)
	    {
	        var retVal = new RegistrationResult();

            try
            {
                var id = Guid.NewGuid().ToString();

                retVal.ConfirmationToken = await UserSecurity.CreateUserAndAccountAsync(model.Email, model.Password, new
                {
                    MemberId = id,
                    CustomerSession.StoreId,
                    RegisterType = RegisterType.GuestUser.GetHashCode(),
                    AccountState = requireConfirmation ? AccountState.PendingApproval.GetHashCode() : AccountState.Approved.GetHashCode(),
                    Discriminator = "Account"
                }, requireConfirmation);

                var contact = new Contact
                {
                    MemberId = id,
                    FullName = String.Format("{0} {1}", model.FirstName, model.LastName)
                };
                contact.Emails.Add(new Email { Address = model.Email, MemberId = id, Type = EmailType.Primary.ToString() });
                foreach (var addr in model.Addresses)
                {
                    contact.Addresses.Add(addr);
                }

                UserClient.CreateContact(contact);

                retVal.IsSuccess = requireConfirmation || await UserSecurity.LoginAsync(model.Email, model.Password) == SignInStatus.Success;

                return retVal;
            }
            catch (MembershipCreateUserException e)
            {
                retVal.ErrorMessage = ErrorCodeToString(e.StatusCode);
            }
            catch (Exception ex)
            {
                retVal.ErrorMessage = ex.Message;
            }

            return retVal;
        }

        public static bool SendEmail(string linkUrl, string user, string email, string templateName, Action<EmailMessage> defaultMessage = null)
	    {

            //Get template
            var context = new Dictionary<string, object> { { templateName, new SendEmailTemplate { Url = linkUrl, Username = user } } };

            //Send email
            return SendEmail(context, email, templateName, defaultMessage);
	    }

        public static bool SendEmail(IDictionary<string,object> context, string email, string templateName, Action<EmailMessage> defaultMessage = null)
        {

            //Get template
            var template = TemplateService.ProcessTemplate(templateName, context, CultureInfo.CreateSpecificCulture(CustomerSession.Language));

            //Create email message
            var emailMessage = new EmailMessage();
            emailMessage.To.Add(email);


            if (template != null)
            {
                emailMessage.Html = template.Body;
                emailMessage.Subject = template.Subject;
            }
            else if (defaultMessage != null)
            {
                //Use default template
                 defaultMessage(emailMessage);
            }

            //UserSecurity.UserManager.SendEmailAsync()

            //Send email
            return EmailService.SendEmail(emailMessage);
        }

        /// <summary>
        /// After user has logged in do some actions
        /// </summary>
        public async static Task OnPostLogonAsync(string userName, string csrUserName = null)
        {
            var customerId = await UserSecurity.GetUserIdAsync(userName);
            var contact = UserClient.GetCustomer(customerId.ToString(CultureInfo.InvariantCulture), false);

            if (!string.IsNullOrEmpty(csrUserName))
            {
                CustomerSession.CsrUsername = csrUserName;
            }

            if (contact != null)
            {
                var lastVisited = contact.ContactPropertyValues.FirstOrDefault(x => x.Name == ContactPropertyValueName.LastVisit);


                if (lastVisited != null)
                {
                    lastVisited.DateTimeValue = DateTime.UtcNow;
                }
                else
                {
                    lastVisited = new ContactPropertyValue
                    {
                        Name = ContactPropertyValueName.LastVisit,
                        DateTimeValue = DateTime.UtcNow,
                        ValueType = PropertyValueType.DateTime.GetHashCode()
                    };
                    contact.ContactPropertyValues.Add(lastVisited);
                }

                if (!string.IsNullOrEmpty(csrUserName))
                {
                    var lastVisitedByCsr = new ContactPropertyValue
                    {
                        Name = ContactPropertyValueName.LastVisitCSR,
                        DateTimeValue = DateTime.UtcNow,
                        ShortTextValue = string.Format("CSR username: {0}", csrUserName),
                        ValueType = PropertyValueType.DateTime.GetHashCode()
                    };
                    contact.ContactPropertyValues.Add(lastVisitedByCsr);
                }
                UserClient.SaveCustomerChanges(contact.MemberId);
            }
        }

        /// <summary>
        /// Converts error code to string.
        /// </summary>
        /// <param name="createStatus">The create status.</param>
        /// <returns>System.String.</returns>
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return
                        "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return
                        "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return
                        "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return
                        "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion

        #endregion
    }
}