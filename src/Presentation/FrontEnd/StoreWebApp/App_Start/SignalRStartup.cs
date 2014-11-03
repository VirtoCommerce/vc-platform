using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNet.SignalR.Tracing;
using Microsoft.Owin;
using Owin;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Web;

namespace VirtoCommerce.Web
{
    public partial class Startup
    {
        public void ConfigureSignlarR(IAppBuilder app)
        {
            var hubConfiguration = new HubConfiguration();
            // Any connection or hub wire up and configuration should go here
#if DEBUG
            hubConfiguration.EnableDetailedErrors = true;
#endif
            app.MapSignalR(hubConfiguration);
        }
    }
}