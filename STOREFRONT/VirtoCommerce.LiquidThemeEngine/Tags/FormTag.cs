using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DotLiquid;
using DotLiquid.Exceptions;

namespace VirtoCommerce.LiquidThemeEngine.Tags
{
    /// <summary>
    /// https://docs.shopify.com/themes/liquid-documentation/tags/theme-tags#form
    /// Creates an HTML <form> element with all the necessary attributes (action, id, etc.) and <input> 
    /// to submit the form successfully.
    /// </summary>
    public class FormTag : Block
    {
        private static readonly Regex Syntax = new Regex(string.Format(@"^({0})\s*,?\s*({1}*)\s*", Liquid.QuotedFragment, Liquid.VariableSignature), RegexOptions.Compiled);
        private string _formName;
        private static Dictionary<string, string> _formsMap = new Dictionary<string, string>();

        static FormTag()
        {
            //Shopify know form types

            //Generates a form for activating a customer account on the activate_account.liquid template.
            _formsMap["activate_customer_password"] = "~/account/activate";
            //Generates a form for submitting an email through the Liquid contact form.
            _formsMap["contact"] = "~/contact";
            //Generates a form for submitting an email through the Liquid contact form.
            _formsMap["create_customer"] = "~/account/register";
            //Generates a form for creating or editing customer account addresses on the addresses.liquid template. When creating a new address, include the parameter customer.new_address. When editing an existing address, include the parameter address.
            _formsMap["customer_address"] = "~/account/addresses";
            //Generates a form for logging into Customer Accounts on the login.liquid template.
            _formsMap["customer_login"] = "~/account/login";
            //Generates a form for recovering a lost password on the login.liquid template.
            _formsMap["recover_customer_password"] = "~/account/forgotpassword";
            //Generates a form for setting a new password on the reset_password.liquid template.
            _formsMap["reset_customer_password"] = "~/account/resetpassword";
        }

        #region Public Methods and Operators
        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            var syntaxMatch = Syntax.Match(markup);

            if (syntaxMatch.Success)
            {
                _formName = syntaxMatch.Groups[1].Value.ToLowerInvariant();
            }
            else
            {
                throw new SyntaxException("Form tag syntax error");
            }

            base.Initialize(tagName, markup, tokens);
        }

        public override void Render(Context context, TextWriter result)
        {
            var formName = ((context[this._formName] ?? null) ?? this._formName).ToString();
            string actionUrl;
            if (_formsMap.TryGetValue(formName, out actionUrl))
            {
                var themeEngine = (ShopifyLiquidThemeEngine)Template.FileSystem;
                var actionAbsoluteUrl = themeEngine.UrlBuilder.ToAppAbsolute(actionUrl, themeEngine.WorkContext.CurrentStore, themeEngine.WorkContext.CurrentLanguage);
                result.WriteLine("<form accept-charset=\"UTF-8\" action=\"{0}\" method=\"post\" id=\"{1}\">", actionAbsoluteUrl, formName);
                this.RenderAll(this.NodeList, context, result);
                result.WriteLine("</form>");
            }
            else
            {
                throw new SyntaxException(String.Format("Unknow form type {0}", _formName));
            }

        }
        #endregion
    }
}
