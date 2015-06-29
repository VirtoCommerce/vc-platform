using Newtonsoft.Json;

namespace AvaTaxCalcREST
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;

    public partial class JsonAddressSvc
    {
        private static string accountNum;
        private static string license;
        private static string svcURL;

        public JsonAddressSvc(string accountNumber, string licenseKey, string serviceURL)
        {
            accountNum = accountNumber;
            license = licenseKey;
            svcURL = serviceURL.TrimEnd('/') + "/1.0/";
        }

        public ValidateResult Validate(Address address)
        {
            // Convert input address data to query string
            string querystring = string.Empty;
            querystring += (address.Line1 == null) ? string.Empty : "Line1=" + address.Line1.Replace(" ", "+");
            querystring += (address.Line2 == null) ? string.Empty : "&Line2=" + address.Line2.Replace(" ", "+");
            querystring += (address.Line3 == null) ? string.Empty : "&Line3=" + address.Line3.Replace(" ", "+");
            querystring += (address.City == null) ? string.Empty : "&City=" + address.City.Replace(" ", "+");
            querystring += (address.Region == null) ? string.Empty : "&Region=" + address.Region.Replace(" ", "+");
            querystring += (address.PostalCode == null) ? string.Empty : "&PostalCode=" + address.PostalCode.Replace(" ", "+");
            querystring += (address.Country == null) ? string.Empty : "&Country=" + address.Country.Replace(" ", "+");

            // Call the service
            var webAddress = new Uri(svcURL + "address/validate?" + querystring);
            var request = WebRequest.Create(webAddress) as HttpWebRequest;
            request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(accountNum + ":" + license)));
            request.Method = "GET";

            var result = new ValidateResult();

            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    // Get the stream containing content returned by the server.
                    var responseStream = response.GetResponseStream();
                    // Open the stream using a StreamReader for easy access.
                    using (var reader = new StreamReader(responseStream))
                    {
                        result = JsonConvert.DeserializeObject<ValidateResult>(reader.ReadToEnd());
                    }
                }
            }
            catch (WebException ex)
            {
                using (var response = ex.Response)
                {
                    using (var data = response.GetResponseStream())
                    {
                        // Open the stream using a StreamReader for easy access.
                        using (var reader = new StreamReader(data))
                        {
                            result = JsonConvert.DeserializeObject<ValidateResult>(reader.ReadToEnd());
                        }
                    }
                }
            }
            
            return result;
        }
    }
}