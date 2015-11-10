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
    public class LayoutTag : Block
    {
        private static readonly Regex Syntax = new Regex(string.Format(@"^({0})", DotLiquid.Liquid.QuotedFragment));
        private string _templateName;

        #region Public Methods and Operators
        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            var syntaxMatch = Syntax.Match(markup);

            if (syntaxMatch.Success)
            {
                this._templateName = syntaxMatch.Groups[1].Value;
            }
            else
            {
                throw new SyntaxException("LayoutTagSyntaxException");
            }

            base.Initialize(tagName, markup, tokens);
        }

        public override void Render(Context context, TextWriter result)
        {
            var template = (context[this._templateName] ?? String.Empty).ToString();
            if (String.IsNullOrEmpty(template))
            {
                template = this._templateName;
            }

            context.Registers["layout"] = template;
        }

        protected override void AssertMissingDelimitation()
        {
        }

        protected override void Parse(List<string> tokens)
        {
        }
        #endregion
    }
}

