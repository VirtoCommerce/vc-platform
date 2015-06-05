
using AvaTax.TaxModule.Web.Services;

namespace AvaTax.TaxModule.Web.Managers
{
    
        public class AvaTaxImpl : ITax
        {
            private readonly string _username;
            private readonly string _password;
            private readonly string _code;
            private readonly string _description;
            private readonly string _logoUrl;

            public AvaTaxImpl(string username, string password, string code, string description, string logoUrl)
            {
                _username = username;
                _password = password;
                _code = code;
                _description = description;
                _logoUrl = logoUrl;
            }

            public string Username
            {
                get { return _username; }
            }

            public string Password
            {
                get { return _password; }
            }

            public string Code
            {
                get { return _code; }
            }

            public string Description
            {
                get { return _description; }
            }

            public string LogoUrl
            {
                get { return _logoUrl; }
            }
        }
}