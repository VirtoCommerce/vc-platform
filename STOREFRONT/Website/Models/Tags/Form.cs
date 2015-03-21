#region
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

    #region
    #endregion

    public class Form : Block
    {
        #region Static Fields
        private static readonly Regex Syntax = new Regex(string.Format(@"^({0})", Liquid.QuotedFragment));
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

            var forms = context["Forms"] as SubmitForm[];

            var form = forms.SingleOrDefault(f => f.Id == template);

            result.WriteLine(
                "<form accept-charset=\"UTF-8\" action=\"{0}\" method=\"post\">",
                form != null ? form.ActionLink : "");
            result.WriteLine("<input name=\"form_type\" type=\"hidden\" value=\"{0}\" />", template);

            context["form"] = form;
            this.RenderAll(this.NodeList, context, result);
            result.WriteLine("</form>");
        }
        #endregion
    }
}