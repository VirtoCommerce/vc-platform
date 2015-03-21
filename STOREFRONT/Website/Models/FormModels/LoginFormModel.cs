#region
using System.ComponentModel.DataAnnotations;

#endregion

namespace VirtoCommerce.Web.Models.FormModels
{
    public class LoginFormModel : BaseCustomerFormModel
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
        #endregion
    }
}