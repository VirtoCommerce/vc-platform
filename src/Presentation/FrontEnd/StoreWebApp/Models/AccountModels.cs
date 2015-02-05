using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Web.Mvc;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Web.Client.Extensions.Validation;

namespace VirtoCommerce.Web.Models
{
	/// <summary>
	/// Enum LoginStatus
	/// </summary>
    public enum LoginStatus
    {
		/// <summary>
		/// The success
		/// </summary>
        Success,
		/// <summary>
		/// The not authorized
		/// </summary>
        NotAuthorized,
		/// <summary>
		/// The incorrect login password
		/// </summary>
        IncorrectLoginPassword
    }

	/// <summary>
	/// Class ChangeAccountInfoModel.
	/// </summary>
    public class ChangeAccountInfoModel
    {
		/// <summary>
		/// Gets or sets the old password.
		/// </summary>
		/// <value>The old password.</value>
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        [RequiredIf("ChangePassword", true)]
        public string OldPassword { get; set; }

		/// <summary>
		/// Gets or sets the new password.
		/// </summary>
		/// <value>The new password.</value>
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        [RequiredIf("ChangePassword", true)]
        public string NewPassword { get; set; }

		/// <summary>
		/// Gets or sets the confirm password.
		/// </summary>
		/// <value>The confirm password.</value>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match." )]
        [RequiredIf("ChangePassword", true)]
        public string ConfirmPassword { get; set; }

		/// <summary>
		/// Gets or sets the full name.
		/// </summary>
		/// <value>The full name.</value>
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		/// <value>The email.</value>
        [Required]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [Display(Name = "Change Password")]
        public bool ChangePassword { get; set; }
    }

	/// <summary>
	/// Class LogOnModel.
	/// </summary>
    public class LogOnModel
    {
		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>The name of the user.</value>
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the name of the impersonated user.
        /// </summary>
        /// <value>The name of the impersonated user.</value>
        public string ImpersonatedUserName { get; set; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [remember me].
		/// </summary>
		/// <value><c>true</c> if [remember me]; otherwise, <c>false</c>.</value>
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

	/// <summary>
	/// Class RegisterModel.
	/// </summary>
    public class RegisterModel
    {

		/// <summary>
		/// Initializes a new instance of the <see cref="RegisterModel"/> class.
		/// </summary>
	    public RegisterModel()
	    {
			Addresses = new List<Address>();
	    }

		/// <summary>
		/// Gets or sets the action result.
		/// </summary>
		/// <value>The action result.</value>
        public ActionResult ActionResult { get; set; }

		/// <summary>
		/// Gets or sets the addresses.
		/// </summary>
		/// <value>The addresses.</value>
		public List<Address> Addresses { get; set; }

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

        [Display(Name = "Sign up for news")]
        public bool SignUpForNews { get; set; }

		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		/// <value>The email.</value>
        [Required, EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

		/// <summary>
		/// Gets or sets the confirm password.
		/// </summary>
		/// <value>The confirm password.</value>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password",ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }


	/// <summary>
	/// Class ForgotPasswordModel.
	/// </summary>
    public class ForgotPasswordModel
    {
		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		/// <value>The email.</value>
        [Display(Name = "User name")]
        [Required]
        public string UserName { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }
    }

    public class ResetPasswordModel
    {
        public ResetPasswordModel(){}

        public ResetPasswordModel(string token)
        {
            Token = token;
        }
        /// <summary>
        /// Gets or sets the new password.
        /// </summary>
        /// <value>The new password.</value>
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        [RequiredIf("ChangePassword", true)]
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets the confirm password.
        /// </summary>
        /// <value>The confirm password.</value>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        [RequiredIf("ChangePassword", true)]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        [Required, EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }


        [Required]
        [Display(Name = "Password reset token")]
        public string Token { get; set; }

    }

    [DataContract]
    public class SendEmailTemplate
    {
        public SendEmailTemplate()
        {
            AuthorName = "Virto Commerce";
        }

        [DataMember]
        public string Url { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string AuthorName { get; set; }
    }

	/// <summary>
	/// Class RegisterExternalLoginModel.
	/// </summary>
    public class RegisterExternalLoginModel
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        [Required, EmailAddress]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

		/// <summary>
		/// Gets or sets the new password.
		/// </summary>
		/// <value>The new password.</value>
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		[RequiredIf("CreateLocalLogin",true)]
		public string NewPassword { get; set; }

		/// <summary>
		/// Gets or sets the confirm password.
		/// </summary>
		/// <value>The confirm password.</value>
		[DataType(DataType.Password)]
		[Display(Name = "Confirm new password")]
		[System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
		[RequiredIf("CreateLocalLogin", true)]
		public string ConfirmPassword { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [create local login].
		/// </summary>
		/// <value><c>true</c> if [create local login]; otherwise, <c>false</c>.</value>
		[Display(Name = "Create/Use local account?")]
	    public bool CreateLocalLogin { get; set; }
    }

	/// <summary>
	/// Class LocalPasswordModel.
	/// </summary>
    public class LocalPasswordModel
    {
		/// <summary>
		/// Gets or sets the old password.
		/// </summary>
		/// <value>The old password.</value>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

		/// <summary>
		/// Gets or sets the new password.
		/// </summary>
		/// <value>The new password.</value>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

		/// <summary>
		/// Gets or sets the confirm password.
		/// </summary>
		/// <value>The confirm password.</value>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword",
            ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

	/// <summary>
	/// Class LoginModel.
	/// </summary>
    public class LoginModel
    {
		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>The name of the user.</value>
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [remember me].
		/// </summary>
		/// <value><c>true</c> if [remember me]; otherwise, <c>false</c>.</value>
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

	/// <summary>
	/// Class ExternalLogin.
	/// </summary>
    public class ExternalLogin
    {
		/// <summary>
		/// Gets or sets the provider.
		/// </summary>
		/// <value>The provider.</value>
        public string Provider { get; set; }
		/// <summary>
		/// Gets or sets the display name of the provider.
		/// </summary>
		/// <value>The display name of the provider.</value>
        public string ProviderDisplayName { get; set; }
		/// <summary>
		/// Gets or sets the provider user identifier.
		/// </summary>
		/// <value>The provider user identifier.</value>
        public string ProviderUserId { get; set; }
    }

    public class RegistrationResult
    {
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }
        /// <summary>
        /// Gets or sets the confirmation token.
        /// </summary>
        /// <value>
        /// The confirmation token.
        /// </value>
        public string ConfirmationToken { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is success.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is success; otherwise, <c>false</c>.
        /// </value>
        public bool IsSuccess { get; set; }
    }
}