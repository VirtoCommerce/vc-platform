#region
using System.Web;

#endregion

namespace VirtoCommerce.Web.Views.Engines.Liquid.Extensions
{

    #region
    #endregion

    public static class HttpContextExtensions
    {
        #region Public Methods and Operators
        public static string GetLocalPath(this HttpContext context)
        {
            return context == null ? null : context.Request.Url.LocalPath;
        }
        #endregion
    }
}