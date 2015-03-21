#region
using System.ComponentModel.DataAnnotations;

#endregion

namespace VirtoCommerce.Web.Models.FormModels
{
    public class RegisterFormModel : BaseCustomerFormModel
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

        public string FirstName
        {
            get
            {
                return this.GetValue("first_name");
            }
            set
            {
                this.SetValue("first_name", value);
            }
        }

        public string LastName
        {
            get
            {
                return this.GetValue("last_name");
            }
            set
            {
                this.SetValue("last_name", value);
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