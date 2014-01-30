using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using VirtoCommerce.Web;

[assembly: OwinStartup(typeof(SignalRStartup))]
namespace VirtoCommerce.Web
{
    public class SignalRStartup
    {
        public void Configuration(IAppBuilder app)
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