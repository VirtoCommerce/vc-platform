using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FunctionalTests.TestHelpers
{
    public class TestDataService : IDisposable
    {
        private WebServiceHost _host;
        private readonly Uri _serviceUri;
        private static int _lastHostId = 1;

        public TestDataService(Type serviceType) : this("http://localhost:7123/VirtoCommerceTestDataService", serviceType)
        {
        }

        public TestDataService(string baseUrl, Type serviceType)
        {
            for (var i = 0; i < 100; i++)
            {
                var hostId = Interlocked.Increment(ref _lastHostId);
                this._serviceUri = new Uri(baseUrl + hostId);
                this._host = new DataServiceHost(serviceType, new Uri[] { this._serviceUri });


                var binding = new WebHttpBinding { MaxReceivedMessageSize = Int32.MaxValue };
                _host.AddServiceEndpoint(typeof(System.Data.Services.IRequestHandler), binding, _serviceUri);
                
                //var endpoing = new ServiceEndPoint
                //this._host.Description.Endpoints[0].Binding
                //this._host.AddServiceEndpoint(serviceType, binding, this._serviceUri);
                try
                {
                    this._host.Open();
                    break;
                }
                catch (Exception)
                {
                    this._host.Abort();
                    this._host = null;
                }
            }

            if (this._host == null)
            {
                throw new InvalidOperationException("Could not open a service even after 100 tries.");
            }
        }

        public void Dispose()
        {
            if (this._host == null) return;

            this._host.Close();
            this._host = null;
        }

        public Uri ServiceUri
        {
            get { return this._serviceUri; }
        }
    }

}
