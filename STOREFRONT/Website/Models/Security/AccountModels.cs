#region
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

#endregion

namespace VirtoCommerce.Web.Models.Security
{

    #region
    #endregion

    public abstract class BaseCustomerFormModel
    {
        #region Constructors and Destructors
        protected BaseCustomerFormModel()
        {
            this.Customer = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }
        #endregion

        #region Public Properties
        public Dictionary<string, string> Customer { get; set; }

        public string form_type { get; set; }
        #endregion

        #region Public Methods and Operators
        public string GetValue(string name)
        {
            return this.Customer.ContainsKey(name) ? this.Customer[name] : String.Empty;
        }
        #endregion

        #region Methods
        protected void SetValue(string name, string value)
        {
            if (this.Customer.ContainsKey(name))
            {
                this.Customer.Add(name, value);
            }
            else
            {
                this.Customer[name] = value;
            }
        }
        #endregion
    }

    public class ExternalLoginConfirmationViewModel
    {
        #region Public Properties
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        #endregion
    }

    public class ExternalLoginListViewModel
    {
        #region Public Properties
        public string ReturnUrl { get; set; }
        #endregion
    }

    public class SendCodeViewModel
    {
        #region Public Properties
        public ICollection<SelectListItem> Providers { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }

        public string SelectedProvider { get; set; }
        #endregion
    }

    public class VerifyCodeViewModel
    {
        #region Public Properties
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        public string Provider { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
        #endregion
    }
}