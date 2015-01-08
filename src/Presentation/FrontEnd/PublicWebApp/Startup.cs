using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PublicWebApp.Startup))]
namespace PublicWebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
