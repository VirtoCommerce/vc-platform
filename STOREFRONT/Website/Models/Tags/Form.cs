#region

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using DotLiquid;
using DotLiquid.Exceptions;
using VirtoCommerce.Web.Views.Engines.Liquid.Extensions;
#endregion

namespace VirtoCommerce.Web.Models.Tags
{
    public class Form : Block
    {
        #region Static Fields
        private static readonly Regex Syntax = new Regex(string.Format(@"^({0})\s*,?({1}*)\s*", Liquid.QuotedFragment, Liquid.VariableSignature));
        #endregion

        #region Fields
        private string _templateName;
        private string _formVariableName;
        #endregion

        #region Public Methods and Operators
        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            var syntaxMatch = Syntax.Match(markup);

            if (syntaxMatch.Success)
            {
                this._templateName = syntaxMatch.Groups[1].Value;
                if(syntaxMatch.Groups.Count == 3)
                    _formVariableName = syntaxMatch.Groups[2].Value;
            }
            else
            {
                throw new SyntaxException("LayoutTagSyntaxException");
            }

            base.Initialize(tagName, markup, tokens);
        }

        public override void Render(Context context, TextWriter result)
        {
            var template = context[this._templateName].ToNullOrString() ?? this._templateName;

            var id = template;
            if (!String.IsNullOrEmpty(_formVariableName))
            {
                var formObject = context[this._formVariableName] as IFormNaming;
                if (formObject != null)
                    id = formObject.FormName;
            }

            var forms = context["Forms"] as SubmitForm[];

            var form = forms.SingleOrDefault(f => f.Id == template);

            result.WriteLine(
                "<form accept-charset=\"UTF-8\" action=\"{0}\" method=\"post\" id=\"{1}\">",
                form != null ? form.ActionLink : "", id);

            result.WriteLine("<input name=\"form_type\" type=\"hidden\" value=\"{0}\" />", template);

            context["form"] = form;
            this.RenderAll(this.NodeList, context, result);
            result.WriteLine("</form>");
        }
        #endregion
    }
}