#region
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DotLiquid;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using VirtoCommerce.Web.Views.Engines.Liquid.ViewEngine.Extensions;

#endregion

namespace VirtoCommerce.Web.Views.Engines.Liquid
{
    public class CommonFilters
    {
        #region Public Methods and Operators
        public static string Default(object input, object value)
        {
            if (input == null)
            {
                return value == null ? null : value.ToString();
            }

            return input.ToString();
        }

        public static string DefaultPagination(Hash input)
        {
            var defaultTemplate = Path.Combine(
                HttpContext.Current.Server.MapPath("~/App_Data/Global"),
                "snippets",
                "global_paging.liquid");
            var contents = String.Empty;
            if (File.Exists(defaultTemplate))
            {
                contents = File.ReadAllText(defaultTemplate);
            }

            var paginateObject = Hash.FromAnonymousObject(new { paginate = input });
            var template = Template.Parse(contents);
            var output = template.Render(paginateObject);

            return output;
        }


        public static string Json(object input)
        {
            if (input == null)
            {
                return null;
            }

            var contents = Hash.FromAnonymousObject(new { input });
            var serializedString = JsonConvert.SerializeObject(
                contents["input"],
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    ContractResolver = new RubyContractResolver(),
                });

            return serializedString;
        }

        public static string Md5(string input)
        {
            if (input == null)
            {
                return null;
            }

            byte[] hash;
            using (var md5 = MD5.Create())
            {
                hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
            }

            return ToHexString(hash);
        }

        public static string Pluralize(int input, string singular, string plural)
        {
            return input == 1 ? singular : plural;
        }

        public static string ScriptTag(string input)
        {
            return String.Format("<script src=\"{0}\" type=\"text/javascript\"></script>", input);
        }


        public static string Strip(string input)
        {
            return input.Trim();
        }

        public static string StylesheetTag(string input)
        {
            return String.Format("<link href=\"{0}\" rel=\"stylesheet\" type=\"text/css\" media=\"all\" />", input);
        }

        public static string ToHexString(byte[] hex)
        {
            if (hex == null)
            {
                return null;
            }
            if (hex.Length == 0)
            {
                return string.Empty;
            }
            var s = new StringBuilder();
            foreach (var b in hex)
            {
                s.Append(b.ToString("x2"));
            }
            return s.ToString();
        }
        #endregion

    }

    public class RubyContractResolver : DefaultContractResolver
    {
        #region Methods
        protected override string ResolvePropertyName(string propertyName)
        {
            return Template.NamingConvention.GetMemberName(propertyName);
        }
        #endregion
    }
}