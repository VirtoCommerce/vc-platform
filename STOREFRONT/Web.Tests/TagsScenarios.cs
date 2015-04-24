using System;
using System.Text;
using DotLiquid;
using VirtoCommerce.Web.Models;
using VirtoCommerce.Web.Models.Tags;
using Xunit;

namespace Web.Tests
{
    public class TagsScenarios
    {
        [Fact]
        public void Can_parse_form_tag()
        {
            Template.RegisterTag<Form>("form");
            var form = new SubmitForm
                       {
                           Id = "customer_login",
                           FormType = "customer_login",
                           ActionLink = "~/account/login",
                           PasswordNeeded = true
                       };

            var address = new AddressTestForm();
            var allForms = new[]{ form };

            var builder = new StringBuilder();
            builder.AppendLine(String.Format("<form accept-charset=\"UTF-8\" action=\"{0}\" method=\"post\" id=\"{1}\">", form.ActionLink, form.Id));
            builder.AppendLine(String.Format("<input name=\"form_type\" type=\"hidden\" value=\"{0}\" />", form.Id));
            builder.AppendLine("</form>");
            var context = Hash.FromAnonymousObject(new { Forms = allForms, Address = address, Customer = new CustomerTestForm() });
            Helper.AssertTemplateResult(builder.ToString(), "{% form 'customer_login' %}{% endform %}", context);
            //'customer_address', customer.new_address 
            var builder2 = new StringBuilder();
            builder2.AppendLine(String.Format("<form accept-charset=\"UTF-8\" action=\"{0}\" method=\"post\" id=\"{1}\">", form.ActionLink, address.FormName));
            builder2.AppendLine(String.Format("<input name=\"form_type\" type=\"hidden\" value=\"{0}\" />", form.Id));
            builder2.AppendLine("</form>");

            Helper.AssertTemplateResult(builder2.ToString(), "{% form 'customer_login',address %}{% endform %}", context);

            var builder3 = new StringBuilder();
            builder3.AppendLine(String.Format("<form accept-charset=\"UTF-8\" action=\"{0}\" method=\"post\" id=\"{1}\">", form.ActionLink, address.FormName));
            builder3.AppendLine(String.Format("<input name=\"form_type\" type=\"hidden\" value=\"{0}\" />", form.Id));
            builder3.AppendLine("</form>");
            
            Helper.AssertTemplateResult(builder3.ToString(), "{% form 'customer_login', customer.new_address %}{% endform %}", context);
        }

        private class AddressTestForm : Drop
        {
            #region Implementation of IFormNaming

            public string FormName {
                get { return "address_form_123"; }
            }

            #endregion
        }

        private class CustomerTestForm : Drop
        {
            public string NewAddress
            {
                get { return "address_form_123"; }
            }
        }
    }

    public class Helper
    {
        public static void AssertTemplateResult(string expected, string template, Hash localVariables)
        {
            Assert.Equal(expected, Template.Parse(template).Render(localVariables));
        }

        public static void AssertTemplateResult(string expected, string template)
        {
            AssertTemplateResult(expected, template, null);
        }
    }
}
