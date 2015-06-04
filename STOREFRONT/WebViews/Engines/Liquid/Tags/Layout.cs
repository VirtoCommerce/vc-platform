#region
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using DotLiquid;
using DotLiquid.Exceptions;
using VirtoCommerce.Web.Views.Engines.Liquid.Extensions;

#endregion

namespace VirtoCommerce.Web.Views.Engines.Liquid.Tags
{

    #region
    #endregion

    public class Layout : Block
    {
        #region Static Fields
        private static readonly Regex Syntax = new Regex(string.Format(@"^({0})", DotLiquid.Liquid.QuotedFragment));
        #endregion

        #region Fields
        private string _templateName;
        #endregion

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
            var template = context[this._templateName].ToNullOrString();
            if (template == null)
            {
                template = this._templateName;
            }

            context.Registers["layout"] = template;
        }
        #endregion

        #region Methods
        protected override void AssertMissingDelimitation()
        {
        }

        protected override void Parse(List<string> tokens)
        {
        }
        #endregion
    }
}