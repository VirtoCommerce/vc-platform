namespace AvaTaxCalcREST
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Xml.Serialization;

    public partial class AddressSvc
    {
        private static string accountNum;
        private static string license;
        private static string svcURL;

        public AddressSvc(string accountNumber, string licenseKey, string serviceURL)
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
            Uri webAddress = new Uri(svcURL + "address/validate.xml?" + querystring);
            HttpWebRequest request = WebRequest.Create(webAddress) as HttpWebRequest;
            request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(accountNum + ":" + license)));
            request.Method = "GET";

            ValidateResult result = new ValidateResult();
            try
            {
                WebResponse response = request.GetResponse();
                XmlSerializer r = new XmlSerializer(result.GetType());
                result = (ValidateResult)r.Deserialize(response.GetResponseStream());
                address = result.Address; // If the address was validated, take the validated address.
            }
            catch (WebException ex)
            {
                Stream responseStream = ((HttpWebResponse)ex.Response).GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string responseString = reader.ReadToEnd();

                // The service returns some error messages in JSON for authentication/unhandled errors.
                if (responseString.StartsWith("{")  || responseString.StartsWith("["))
                {
                    result = new ValidateResult();
                    result.ResultCode = SeverityLevel.Error;
                    Message msg = new Message();
                    msg.Severity = result.ResultCode;
                    msg.Summary = "The request was unable to be successfully serviced, please try again or contact Customer Service.";
                    msg.Source = "Avalara.Web.REST";
                    if (!((HttpWebResponse)ex.Response).StatusCode.Equals(HttpStatusCode.InternalServerError))
                    {
                        msg.Summary = "The user or account could not be authenticated.";
                        msg.Source = "Avalara.Web.Authorization"; 
                    }

                    result.Messages = new Message[1] { msg };
                }
                else
                {
                    XmlSerializer r = new XmlSerializer(result.GetType());
                    byte[] temp = Encoding.ASCII.GetBytes(responseString);
                    MemoryStream stream = new MemoryStream(temp);
                    result = (ValidateResult)r.Deserialize(stream); // Inelegant, but the deserializer only takes streams, and we already read ours out.
                }
            }

            return result;
        }
    }
}