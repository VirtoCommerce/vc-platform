using System.Threading;
using System.Web;

namespace VirtoCommerce.ApiClient.Session
{ 
    /// <summary>
    /// Provide access to current customer session info.
    /// </summary>
    public class CustomerSessionService : ICustomerSessionService
    {
        private string SESSION_KEY = "v-customersession";

        #region Customer Session
        public ICustomerSession CustomerSession
        {
            get
            {
                var key = SESSION_KEY;

                // Persist in thread
                if (HttpContext.Current == null)
                {
                    var ctxThread = Thread.GetData(Thread.GetNamedDataSlot(key));
                    if (ctxThread != null)
                        return (CustomerSession)ctxThread;

                    var ctx = new CustomerSession();
                    Thread.SetData(Thread.GetNamedDataSlot(key), ctx);
                    return ctx;
                }

                // Persist in HttpContext
                if (HttpContext.Current.Items[key] == null)
                {
                    var ctx = new CustomerSession();
                    HttpContext.Current.Items.Add(key, ctx);
                    return ctx;
                }
                else
                    return (CustomerSession)HttpContext.Current.Items[key];
            }
        }
        #endregion
    }
}
