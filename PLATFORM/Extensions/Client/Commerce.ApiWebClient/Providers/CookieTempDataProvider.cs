using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace VirtoCommerce.ApiWebClient.Providers
{
    using VirtoCommerce.ApiClient;
    using VirtoCommerce.ApiClient.Session;

    public class CookieTempDataProvider : ITempDataProvider
    {

        const string CookieName = "TempData";

        public void SaveTempData(
            ControllerContext controllerContext,
            IDictionary<string, object> values)
        {
            // convert the temp data dictionary into json
            var value = Serialize(values);
            // sign and encrypt the data via the asp.net machine key
            value = CustomerSession.EncryptCookie(value);
            // issue the cookie
            IssueCookie(controllerContext, value);
        }

        public IDictionary<string, object> LoadTempData(
            ControllerContext controllerContext)
        {
            // get the cookie
            var value = GetCookieValue(controllerContext);

            if (value != null)
            {
                // verify and decrypt the value via the asp.net machine key
                // decompress to json
                value = CustomerSession.DecryptCookie(value);
                // convert the json back to a dictionary
                return Deserialize(value);
            }

            return new Dictionary<string, object>();
        }
        string GetCookieValue(ControllerContext controllerContext)
        {
            var c = controllerContext.HttpContext.Request.Cookies[CookieName];
            if (c != null)
            {
                return c.Value;
            }
            return null;
        }

        private void IssueCookie(ControllerContext controllerContext, string value)
        {
            var c = new HttpCookie(CookieName, value)
            {
                // don't allow javascript access to the cookie
                HttpOnly = true,
                // set the path so other apps on the same server don't see the cookie
                Path = controllerContext.HttpContext.Request.ApplicationPath,
                // ideally we're always going over SSL, but be flexible for non-SSL apps
                Secure = controllerContext.HttpContext.Request.IsSecureConnection
            };

            if (value == null)
            {
                // if we have no data then issue an expired cookie to clear the cookie
                c.Expires = DateTime.Now.AddMonths(-1);
            }

            if (value != null || controllerContext.HttpContext.Request.Cookies[CookieName] != null)
            {
                // if we have data, then issue the cookie
                // also, if the request has a cookie then we need to issue the cookie
                // which might act as a means to clear the cookie 
                controllerContext.HttpContext.Response.Cookies.Add(c);
            }
        }

        string Serialize(IDictionary<string, object> data)
        {
            if (data == null || data.Keys.Count == 0) return null;

            var ser = new JavaScriptSerializer();
            return ser.Serialize(data);
        }

        private IDictionary<string, object> Deserialize(string data)
        {
            if (String.IsNullOrWhiteSpace(data)) return null;

            var ser = new JavaScriptSerializer();
            return ser.Deserialize<IDictionary<string, object>>(data);
        }
    }
}
