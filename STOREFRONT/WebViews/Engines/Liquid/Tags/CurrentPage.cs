#region
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using DotLiquid;
using DotLiquid.Exceptions;
using DotLiquid.Util;

#endregion

namespace VirtoCommerce.Web.Views.Engines.Liquid.Tags
{

    #region
    #endregion

    /// <summary>
    ///     CurrentPage sets the page used by the Pagination tag in your template.
    ///     {% current_page = 10 %}
    /// </summary>
    public class CurrentPage : Tag
    {
        #region Static Fields
        private static readonly Regex Syntax = R.B(R.Q(@"\s*=\s*(.*)\s*"), DotLiquid.Liquid.VariableSignature);
        #endregion

        #region Fields
        private int _page;
        #endregion

        #region Public Methods and Operators
        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            var syntaxMatch = Syntax.Match(markup);
            if (syntaxMatch.Success)
            {
                if (syntaxMatch.Groups.Count > 0)
                {
                    if (!Int32.TryParse(syntaxMatch.Groups[1].Value, out this._page))
                    {
                        this._page = 1;
                    }
                }
            }
            else
            {
                throw new SyntaxException("CurrentPageTagSyntaxException");
            }

            base.Initialize(tagName, markup, tokens);
        }

        public override void Render(Context context, TextWriter result)
        {
            context.Stack(() => { context.Registers["current_page"] = this._page; });
        }
        #endregion
    }
}