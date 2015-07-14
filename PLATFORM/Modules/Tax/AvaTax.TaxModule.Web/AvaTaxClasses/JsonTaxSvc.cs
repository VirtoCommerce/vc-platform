using System.Globalization;
using Newtonsoft.Json;

namespace AvaTaxCalcREST
{
    using System;
    using System.IO;
    using System.Net;
    using System.Text;

   public class JsonTaxSvc
    {
        private static string accountNum;
        private static string license;
        private static string svcURL;

        public JsonTaxSvc(string accountNumber, string licenseKey, string serviceURL)
        {
            accountNum = accountNumber;
            license = licenseKey;
            svcURL = serviceURL.TrimEnd('/') + "/1.0/";
        }

        // This actually calls the service to perform the tax calculation, and returns the calculation result.
        public GetTaxResult GetTax(GetTaxRequest req)
        {
            var jsonRequest = JsonConvert.SerializeObject(req);
            
            // Call the service
            var address = new Uri(svcURL + "tax/get");
            var request = WebRequest.Create(address) as HttpWebRequest;
            request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(accountNum + ":" + license)));
            request.Method = "POST";
            request.ContentType = "text/json";
            request.ContentLength = jsonRequest.Length;
            var newStream = request.GetRequestStream();
            newStream.Write(ASCIIEncoding.ASCII.GetBytes(jsonRequest), 0, jsonRequest.Length);
            
            var result = new GetTaxResult();
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    // Get the stream containing content returned by the server.
                    newStream = response.GetResponseStream();
                    // Open the stream using a StreamReader for easy access.
                    using (var reader = new StreamReader(newStream))
                    {
                        result = JsonConvert.DeserializeObject<GetTaxResult>(reader.ReadToEnd());
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                {
                    result.ResultCode = SeverityLevel.Error;
                    result.Messages = new[] { new Message { Severity = SeverityLevel.Error, Summary = ex.Message } };
                    return result;
                }

                using (var response = ex.Response)
                {
                    using (var data = response.GetResponseStream())
                    {
                        // Open the stream using a StreamReader for easy access.
                        using (var reader = new StreamReader(data))
                        {
                            result = JsonConvert.DeserializeObject<GetTaxResult>(reader.ReadToEnd());
                        }
                    }
                }
            }
            return result;
        }

        public GeoTaxResult EstimateTax(decimal latitude, decimal longitude, decimal saleAmount)
        {
            // Call the service
            var address = new Uri(svcURL + "tax/" + latitude.ToString(CultureInfo.InvariantCulture).Replace(',', '.') + "," + longitude.ToString(CultureInfo.InvariantCulture).Replace(',', '.') + "/get?saleamount=" + saleAmount);
            var request = WebRequest.Create(address) as HttpWebRequest;
            request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(accountNum + ":" + license)));
            request.Method = "GET";

            var result = new GeoTaxResult();
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    // Get the stream containing content returned by the server.
                    var responseStream = response.GetResponseStream();
                    // Open the stream using a StreamReader for easy access.
                    using (var reader = new StreamReader(responseStream))
                    {
                        result = JsonConvert.DeserializeObject<GeoTaxResult>(reader.ReadToEnd());
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                {
                    result.ResultCode = SeverityLevel.Error;
                    result.Messages = new[] { new Message { Severity = SeverityLevel.Error, Summary = ex.Message } };
                    return result;
                }

                using (var response = ex.Response)
                {
                    using (var data = response.GetResponseStream())
                    {
                        // Open the stream using a StreamReader for easy access.
                        using (var reader = new StreamReader(data))
                        {
                            result = JsonConvert.DeserializeObject<GeoTaxResult>(reader.ReadToEnd());
                        }
                    }
                }
            }

            return result;        
        }

        public GeoTaxResult Ping()
        {
            //As AvaTax doesn't have test connection method. They recommend use estimate tax as test connection.
            return EstimateTax((decimal)47.627935, (decimal)-122.51702, (decimal)10);
        }

        // This calls CancelTax to void a transaction specified in taxreq
        public CancelTaxResult CancelTax(CancelTaxRequest cancelTaxRequest)
        {
            var jsonRequest = JsonConvert.SerializeObject(cancelTaxRequest);

            // Call the service
            var address = new Uri(svcURL + "tax/cancel");
            var request = WebRequest.Create(address) as HttpWebRequest;
            request.Headers.Add(HttpRequestHeader.Authorization, "Basic " + Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(accountNum + ":" + license)));
            request.Method = "POST";
            request.ContentType = "text/json";
            request.ContentLength = jsonRequest.Length;
            var newStream = request.GetRequestStream();
            newStream.Write(ASCIIEncoding.ASCII.GetBytes(jsonRequest), 0, jsonRequest.Length);

            var cancelResponse = new CancelTaxResult();
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    // Get the stream containing content returned by the server.
                    newStream = response.GetResponseStream();
                    // Open the stream using a StreamReader for easy access.
                    using (var reader = new StreamReader(newStream))
                    {
                        cancelResponse = JsonConvert.DeserializeObject<CancelTaxResult>(reader.ReadToEnd());
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Response == null)
                {
                    cancelResponse.ResultCode = SeverityLevel.Error;
                    cancelResponse.Messages = new[] { new Message { Severity = SeverityLevel.Error, Summary = ex.Message } };
                    return cancelResponse;
                }

                using (WebResponse response = ex.Response)
                {
                    using (var data = response.GetResponseStream())
                    {
                        // Open the stream using a StreamReader for easy access.
                        using (var reader = new StreamReader(data))
                        {
                            cancelResponse = JsonConvert.DeserializeObject<CancelTaxResult>(reader.ReadToEnd());
                        }
                    }
                }
            }

            return cancelResponse;
        }
    }
}