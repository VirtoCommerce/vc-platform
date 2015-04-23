using DotLiquid;
using DotLiquid.Exceptions;
using DotLiquid.Util;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using VirtoCommerce.Web.Views.Engines.Liquid.Extensions;

namespace VirtoCommerce.Web.Models.Tags
{
    public class Form : Block
    {
        private static readonly Regex Syntax = R.B(R.Q(@"('{0}'),?\s?({0})?"), Liquid.QuotedFragment);

        private string _formId;
        private Variable _parameter;

        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            var syntaxMatch = Syntax.Match(markup);

            if (syntaxMatch.Success)
            {
                var template = syntaxMatch.Groups[1].Value.Trim('\'').Trim();

                _formId = template;

                if (template == "customer_address")
                {
                    if (markup.Trim() == "'customer_address', customer.new_address")
                    {
                        var variable = CreateVariable("new_address");
                        _formId = "new";
                    }
                    else if (markup.Trim() == "'customer_address', address")
                    {
                        _formId = "1";
                    }
                }

            //    _templateName = syntaxMatch.Groups[1].Value;

            //    if (markup.Trim() == "'customer_address', customer.new_address")
            //    {
            //        _formSuffix = "new";
            //    }
            //    else if (markup.Trim() == "'customer_address', address")
            //    {
            //        _formSuffix = "1";
            //    }
            //}
            //else
            //{
            //    throw new SyntaxException("LayoutTagSyntaxException");
            }

            base.Initialize(tagName, markup, tokens);
        }

        public override void Render(Context context, TextWriter result)
        {
            //var template = context[this._templateName].ToNullOrString();
            //if (template == null)
            //{
            //    template = this._templateName;
            //}

            var forms = context["Forms"] as SubmitForm[];

            var form = forms.FirstOrDefault(f => f.Id == _formId);

            if (form != null)
            {
                result.WriteLine(
                    "<form accept-charset=\"UTF-8\" action=\"{0}\" method=\"post\" id=\"{1}\">",
                    form != null ? form.ActionLink : "",
                    form.Properties.ContainsKey("id") ? form.Properties["id"] : form.Id);
                result.WriteLine("<input name=\"form_type\" type=\"hidden\" value=\"{0}\" />", form.FormType);

                context["form"] = form;
                this.RenderAll(this.NodeList, context, result);
                result.WriteLine("</form>");
            }
        }
    }
}