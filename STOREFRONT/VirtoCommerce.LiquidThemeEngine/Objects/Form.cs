using System.Collections.Generic;
using System.Linq;
using DotLiquid;

namespace VirtoCommerce.LiquidThemeEngine.Objects
{
    /// <summary>
    /// The form object is used within the form tag. It contains attributes of its parent form.
    /// </summary>
    /// <remarks>
    /// https://docs.shopify.com/themes/liquid-documentation/objects/form
    /// </remarks>
    public class Form : Drop
    {
        public Form()
        {
            PostedSuccessfully = true;
            Properties = new Dictionary<string, object>();
        }

        /// <summary>
        /// Returns the name of the author of the blog article comment.
        /// Exclusive to form tags with the "article" parameter.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Returns the content of the blog article comment.
        /// Exclusive to form tags with the "article" parameter.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Returns the email of the blog article comment's author.
        /// Exclusive to form tags with the "article" parameter.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Returns an array of strings if the form was not submitted successfully.
        /// The strings returned depend on which fields of the form were left empty or contained errors.
        /// </summary>
        public FormErrors Errors { get; set; }

        /// <summary>
        /// Returns true if the form was submitted successfully, or false if the form contained errors.
        /// All forms but the address form set that property.
        /// The address form is always submitted successfully.
        /// </summary>
        public bool? PostedSuccessfully { get; set; }

        /// <summary>
        /// Returns true if the form requires password, false otherwise
        /// </summary>
        public bool PasswordNeeded { get; set; }

        public IDictionary<string, object> Properties { get; set; }

        public override object BeforeMethod(string method)
        {
            var val = this.Properties.Where(x => x.Key == method).Select(x => x.Value).SingleOrDefault();
            return val;
        }
    }
}