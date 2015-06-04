using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Net;
using System.Threading;

namespace VirtoCommerce.Foundation.Data.Azure.Common
{
    public class WebRoleWarmUp
    {
        private Thread _worker;

        readonly string[] _pages = { "/", "/cart/" };

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            _worker = new Thread(WarmUp) {IsBackground = true};
            _worker.Start();
        }

        /// <summary>
        /// Warms up.
        /// </summary>
        public void WarmUp()
        {
            while (true)
            {
                try
                {
                    foreach (var page in _pages)
                    {
                        var endpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["HttpIn"];
                        var address = String.Format("{0}://{1}:{2}{3}",
                            endpoint.Protocol,
                            endpoint.IPEndpoint.Address,
                            endpoint.IPEndpoint.Port,
                            page);
                        var webClient = new WebClient();
                        webClient.DownloadString(address);
                        System.Diagnostics.Trace.TraceInformation(string.Format("Pinged {0}", page));
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Trace.TraceInformation(string.Format("Warm Up Error: {0}", e.Message));
                }
                Thread.Sleep(300000); // every 5 min
            }
        }
    }
}
