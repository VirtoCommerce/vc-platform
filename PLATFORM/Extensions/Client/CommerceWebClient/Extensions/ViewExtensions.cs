using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace VirtoCommerce.Web.Client.Extensions
{
    /// <summary>
    /// Class ViewExtensions.
    /// </summary>
    public static class ViewExtensions
    {
        /// <summary>
        /// Returns the validation summary.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <param name="excludePropertyErrors">if set to <c>true</c> [exclude property errors].</param>
        /// <param name="validationMessage">The validation message.</param>
        /// <returns>MvcHtmlString.</returns>
        public static MvcHtmlString MyValidationSummary(this HtmlHelper html, bool excludePropertyErrors, string validationMessage)
        {
            return !html.ViewData.ModelState.IsValid ? MvcHtmlString.Create(String.Format("<ul class=\"messages\"><li class=\"error-msg\"><ul><li><span>{0}</span></li></ul></li></ul>", html.ValidationSummary(excludePropertyErrors, validationMessage))) : MvcHtmlString.Empty;
        }
    }
}