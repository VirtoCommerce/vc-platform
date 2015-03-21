#region
using System.ComponentModel.DataAnnotations;

#endregion

namespace VirtoCommerce.Web.Models.FormModels
{
    public class ResetPasswordFormModel : BaseCustomerFormModel
    {
        #region Public Properties
        [Required]
        [EmailAddress]
        public string Email
        {
            get
            {
                return this.GetValue("email");
            }
            set
            {
                this.SetValue("email", value);
            }
        }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Password
        {
            get
            {
                return this.GetValue("password");
            }
            set
            {
                this.SetValue("password", value);
            }
        }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        [Compare("Password")]
        public string PasswordConfirmation
        {
            get
            {
                return this.GetValue("password_confirmation");
            }
            set
            {
                this.SetValue("password_confirmation", value);
            }
        }
        #endregion
    }
}