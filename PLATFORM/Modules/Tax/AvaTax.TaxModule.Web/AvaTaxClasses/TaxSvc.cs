namespace AvaTaxCalcREST
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

   public class TaxSvc
    {
        private static string accountNum;
        private static string license;
        private static string svcURL;

        public TaxSvc(string accountNumber, string licenseKey, string serviceURL)
        {
            accountNum = accountNumber;
            license = licenseKey;
            svcURL = serviceURL.TrimEnd('/') + "/1.0/";
        }

        // This actually calls the service to perform the tax calculation, and returns the calculation result.
        public GetTaxResult GetTax(GetTaxRequest req)
        {
            // Convert the request to XML
            XmlSerializerNamespaces namesp = new XmlSerializerNamespaces();
            namesp.Add(string.Empty, string.Empty);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            XmlSerializer x = new XmlSerializer(req.GetType());
            StringBuilder sb = new StringBuilder();
            x.Serialize(XmlTextWriter.Create(sb, settings), req, namesp);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sb.ToString());

            // Call the service
            Uri address = new Uri(svcURL + "tax/get");
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(accountNum + ":" + license)));
            request.Method = "POST";
            request.ContentType = "text/xml";
            request.ContentLength = sb.Length;
            Stream newStream = request.GetRequestStream();
            newStream.Write(ASCIIEncoding.ASCII.GetBytes(sb.ToString()), 0, sb.Length);
            GetTaxResult result = new GetTaxResult();
            try
            {
                WebResponse response = request.GetResponse();
                XmlSerializer r = new XmlSerializer(result.GetType());
                result = (GetTaxResult)r.Deserialize(response.GetResponseStream());
            }
            catch (WebException ex)
            {
                XmlSerializer r = new XmlSerializer(result.GetType());
                result = (GetTaxResult)r.Deserialize((ex.Response).GetResponseStream());
            }

            return result;
        }

        public GeoTaxResult EstimateTax(decimal latitude, decimal longitude, decimal saleAmount)
        {
            // Call the service
            Uri address = new Uri(svcURL + "tax/" + latitude + "," + longitude + "/get.xml?saleamount=" + saleAmount);
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(accountNum + ":" + license)));
            request.Method = "GET";

            GeoTaxResult result = new GeoTaxResult();
            try
            {
                WebResponse response = request.GetResponse();
                XmlSerializer r = new XmlSerializer(result.GetType());
                result = (GeoTaxResult)r.Deserialize(response.GetResponseStream());
            }
            catch (WebException ex)
            {
                Stream responseStream = ((HttpWebResponse)ex.Response).GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string responseString = reader.ReadToEnd();

                // The service returns some error messages in JSON for authentication/unhandled errors.
                if (responseString.StartsWith("{") || responseString.StartsWith("[")) 
                {
                    result = new GeoTaxResult();
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
                    result = (GeoTaxResult)r.Deserialize(stream); // Inelegant, but the deserializer only takes streams, and we already read ours out.
                }
            }

            return result;        
        }

        public GeoTaxResult Ping()
        {
            return this.EstimateTax((decimal)47.627935, (decimal)-122.51702, (decimal)10);
        }

        // This calls CancelTax to void a transaction specified in taxreq
        public CancelTaxResult CancelTax(CancelTaxRequest cancelTaxRequest)
        {
            // Convert the request to XML
            XmlSerializerNamespaces namesp = new XmlSerializerNamespaces();
            namesp.Add(string.Empty, string.Empty);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            XmlSerializer x = new XmlSerializer(cancelTaxRequest.GetType());
            StringBuilder sb = new StringBuilder();
            x.Serialize(XmlTextWriter.Create(sb, settings), cancelTaxRequest, namesp);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sb.ToString());

            // Call the service
            Uri address = new Uri(svcURL + "tax/cancel");
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(accountNum + ":" + license)));
            request.Method = "POST";
            request.ContentType = "text/xml";
            request.ContentLength = sb.Length;
            Stream newStream = request.GetRequestStream();
            newStream.Write(ASCIIEncoding.ASCII.GetBytes(sb.ToString()), 0, sb.Length);
            CancelTaxResponse cancelResponse = new CancelTaxResponse();
            try
            {
                WebResponse response = request.GetResponse();
                XmlSerializer r = new XmlSerializer(cancelResponse.GetType());
                cancelResponse = (CancelTaxResponse)r.Deserialize(response.GetResponseStream());
            }
            catch (WebException ex)
            {
                XmlSerializer r = new XmlSerializer(cancelResponse.GetType());
                cancelResponse = (CancelTaxResponse)r.Deserialize((ex.Response).GetResponseStream());

                // If the error is returned at the cancelResponse level, translate it to the cancelResult.
                if (cancelResponse.ResultCode.Equals(SeverityLevel.Error)) 
                {
                    cancelResponse.CancelTaxResult = new CancelTaxResult();
                    cancelResponse.CancelTaxResult.ResultCode = cancelResponse.ResultCode;
                    cancelResponse.CancelTaxResult.Messages = cancelResponse.Messages;
                }
            }

            return cancelResponse.CancelTaxResult;
        }
    }
}