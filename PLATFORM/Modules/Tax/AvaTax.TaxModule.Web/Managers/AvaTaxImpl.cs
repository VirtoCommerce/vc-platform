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
            private readonly string _companyCode;
            private readonly string _serviceUrl;

            public AvaTaxImpl(string username, string password, string serviceUrl, string companyCode, string code, string description, string logoUrl)
            {
                _username = username;
                _password = password;
                _code = code;
                _description = description;
                _logoUrl = logoUrl;
                _serviceUrl = serviceUrl;
                _companyCode = companyCode;
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

            public string ServiceUrl
            {
                get { return _serviceUrl; }
            }

            public string CompanyCode
            {
                get { return _companyCode; }
            }
        }
}