using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Web.LoadTests
{
    [TestClass]
    public class PerformanceUnitTests
    {
        [TestMethod]
        public void LoadAllStores()
        {
            //for (int index = 0; index < 500; index++)
            //{
            //    var client = ClientContext.Clients.CreateStoreClient();
            //    var stores = client.GetStoresAsync().Result;
            //}
        }

        [TestMethod]
        public void RequestCategories()
        {
            for (var index = 0; index < 100; index++)
            {
                RequestPage("samplestore/vendorvirtual-tv-video");
            }
        }

        private void RequestPage(string page)
        {
            WebRequest request = WebRequest.Create(String.Format("http://demo.local/{0}", page));
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            Console.WriteLine(responseFromServer);
            // Clean up the streams and the response.
            reader.Close();
            response.Close();
        }
    }
}
