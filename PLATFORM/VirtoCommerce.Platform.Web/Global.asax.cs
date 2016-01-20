using System;
using System.Web;
using Common.Logging;

namespace VirtoCommerce.Platform.Web
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
		}

        protected void Application_Error(Object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
       
            //Log exception
            var log = LogManager.GetLogger("default");
            log.Error(exception);
        }
    }
}
