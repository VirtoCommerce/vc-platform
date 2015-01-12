using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VirtoCommerce.Web.Startup))]
namespace VirtoCommerce.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
