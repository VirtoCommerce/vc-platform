#region
using System.ComponentModel.DataAnnotations;

#endregion

namespace VirtoCommerce.Web.Models.FormModels
{
    public class ForgotPasswordFormModel : BaseCustomerFormModel
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
        #endregion
    }
}