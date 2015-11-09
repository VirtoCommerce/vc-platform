using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/form
    /// The form object is used within the form tag. It contains attributes of its parent form.
    /// </summary>
    public class Form : Drop
    {
        public Form()
        {
            PostedSuccessfully = true;
        }
        /// <summary>
        /// Returns an array of strings if the form was not submitted successfully. The strings returned depend on which fields of the form were left empty or contained errors. 
        /// </summary>
        public FormErrors Errors { get; set; }

        /// <summary>
        /// Returns true if the form was submitted successfully, or false if the form contained errors. All forms but the address form set that property. The address form is always submitted successfully.
        /// </summary>
        public bool? PostedSuccessfully { get; set; }
    }
}
