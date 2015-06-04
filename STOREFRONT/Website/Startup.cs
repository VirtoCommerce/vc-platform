#region
using Microsoft.Owin;
using Owin;
using VirtoCommerce.Web;

#endregion

[assembly: OwinStartup(typeof(Startup))]

namespace VirtoCommerce.Web
{
    public partial class Startup
    {
        #region Public Methods and Operators
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
        #endregion
    }
}