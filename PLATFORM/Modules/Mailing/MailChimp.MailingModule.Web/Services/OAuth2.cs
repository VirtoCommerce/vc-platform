using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace MailChimp.MailingModule.Web.Services
{
    /// <summary>
    /// Model of what gets sent back from the OAuth2 servers
    /// </summary>
    public class AccessToken
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string scope { get; set; }
    }

    /// <summary>
    /// Model of the error from a 400 error.
    /// </summary>
    public class Error
    {
        public string error;
    }

    /// <summary>
    /// This is my attempt at an OAuth2-v10 client.
    /// Jamie R. Rytlewski - Sept 13, 2011
    /// </summary>
    public abstract class OAuth2
    {
        protected Dictionary<string, string> _variables = new Dictionary<string, string>();
        WebHeaderCollection _headers = new WebHeaderCollection();
        //static string _response_type = "code";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Passes in the configuration</param>
        protected OAuth2(Dictionary<string, string> configuration)
        {
            // Puts all the configuration variables into _variables.
            foreach (var kvp in configuration)
            {
                if (_variables.ContainsKey(kvp.Key))
                {
                    _variables[kvp.Key] = kvp.Value;
                }
                else
                {
                    _variables.Add(kvp.Key, kvp.Value);
                }
            }
        }

        /// <summary>
        /// Makes the request to the OAuth2 serer
        /// </summary>
        /// <param name="path">The path that you want to go</param>
        /// <param name="parameters">Parameters to send to the server</param>
        /// <param name="method">GET or POST</param>
        /// <returns>The text from the server</returns>
        public string makeRequest(string path, Dictionary<string, string> parameters, string method = "GET")
        {
            // Create a request using a URL that can receive a post.
            Stream dataStream;
            if (method == "GET")
            {
                path = getURI(path, parameters);
            }

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path);
            request.UserAgent = "oauth2-draft-v10";
            //request.Accept = "application/json";
            request.Headers = _headers;

            if (method == "POST")
            {
                // Set the Method property of the request to POST.
                request.Method = "POST";
                // Create POST data and convert it to a byte array.
                string postData = getQueryString(parameters);
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
            }
            string json = "";
            // Get the response.
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // Get the stream containing content returned by the server.
                    dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader for easy access.
                    using (StreamReader reader = new StreamReader(dataStream))
                    {
                        // Read the content.
                        json = reader.ReadToEnd();
                    }
                }
            }
            catch (WebException ex)
            {
                using (WebResponse response = ex.Response)
                {
                    using (Stream data = response.GetResponseStream())
                    {
                        throw new Exception(new StreamReader(data).ReadToEnd());
                    }
                }
            }
            return json;
        }

        /// <summary>
        /// Makes an API call
        /// </summary>
        /// <param name="path">The API Path Call</param>
        /// <param name="parameters">Parameters to Pass</param>
        /// <param name="method">GET or POST</param>
        /// <returns>The value from the server</returns>
        public string api(string path, Dictionary<string, string> parameters, string method = "GET")
        {
            var data = MakeOauth2Request(getURI(path, parameters), parameters, method);
            return data;
        }

        /// <summary>
        /// Gets Called from api
        /// </summary>
        /// <param name="path">The Path</param>
        /// <param name="parameters">Parameters to pass to server</param>
        /// <param name="method">GET or POST</param>
        /// <returns></returns>
        public string MakeOauth2Request(string path, Dictionary<string, string> parameters, string method = "GET")
        {
            var token = getAccessToken();
            if (!string.IsNullOrEmpty(getVariable("token_as_header")))
            {
                _headers.Add("Authorization", "OAuth " + token);
            }
            else
            {
                parameters.Add("oauth_token", token);
            }
            return makeRequest(path, parameters, method);
        }

        /// <summary>
        /// Get thes the URI to call based off the path, configurations and parameters.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public string getURI(string path, Dictionary<string, string> parameters)
        {
            var url = getVariable("services_uri") != null ? getVariable("services_uri") : getVariable("base_uri");
            if (!string.IsNullOrEmpty(path))
            {
                if (path.StartsWith("http"))
                {
                    url = path;
                }
                else
                {
                    url = url.TrimEnd('/') + "/" + path.TrimStart('/');
                }
            }
            if (parameters.Count > 0)
            {
                url += "?" + getQueryString(parameters);
            }
            return url;
        }

        /// <summary>
        /// Creates a querystring based off parameters
        /// </summary>
        /// <param name="parameters">Dictionary of parameters</param>
        /// <returns>The querystring</returns>
        protected string getQueryString(Dictionary<string, string> parameters)
        {
            var queryString = new StringBuilder();
            foreach (var i in parameters)
            {
                queryString.Append(String.Format("{0}={1}", i.Key, HttpUtility.UrlEncode(i.Value)) + "&");
            }
            return queryString.ToString().TrimEnd('&');
        }

        /// <summary>
        /// Gets an access Token from the session
        /// </summary>
        /// <returns></returns>
        public string getAccessToken()
        {
            var session = getSession();
            return session;
        }

        /// <summary>
        /// This gets the AccessToken from the Server
        /// </summary>
        /// <param name="code">The code from the server</param>
        /// <returns>the string response from the server</returns>
        public string getAccessTokenFromAuthorizationCode(string code)
        {
            var p = new Dictionary<string, string>() { { "grant_type", "authorization_code" }, { "client_id", _variables["client_id"] }, 
            { "client_secret", _variables["client_secret"] }, {"code", code}, {"redirect_uri", _variables["redirect_uri"]} };

            var json = makeRequest(_variables["access_token_uri"], p, "POST");

            return json;
        }

        /// <summary>
        /// This is where everything gets started
        /// </summary>
        /// <returns>The access token</returns>
        public string getSession()
        {
            var session = new Dictionary<string, string>();

            session = getSessionObject(null);
            session = validateSessionObject(session);

            if (session == null && _variables.ContainsKey("code"))
            {
                var json = getAccessTokenFromAuthorizationCode(_variables["code"]);
                var serializer = new JavaScriptSerializer();
                var a = serializer.Deserialize<AccessToken>(json);
                session = getSessionObject(a);
                session = validateSessionObject(session);
            }

            if (session == null)
            {
                session = getCookie();
                session = validateSessionObject(session);
            }
            setCookie(session);
            return session["access_token"];
        }

        /// <summary>
        /// Checks to see if the _variables Dictionary has the key and if so, update the value
        /// otherwise add the key/value
        /// </summary>
        /// <param name="key">Dictionary Key</param>
        /// <param name="value">Dictionary Value</param>
        protected void setVariable(string key, string value)
        {
            if (_variables.ContainsKey(key))
            {
                _variables[key] = value;
            }
            else
            {
                _variables.Add(key, value);
            }
        }

        /// <summary>
        /// Asks for the variable. If the variable exists in _variables return it, otherwise return null.
        /// </summary>
        /// <param name="name">Variable to get data from</param>
        /// <returns>Variable value or null</returns>
        protected string getVariable(string name)
        {
            if (_variables.ContainsKey(name))
            {
                return _variables[name];
            }
            return null;
        }

        /// <summary>
        /// Sets the Cookie based off the session variables being passed around
        /// </summary>
        /// <param name="session"></param>
        protected void setCookie(Dictionary<string, string> session)
        {
            var cookie = new HttpCookie("oauth2_" + getVariable("client_id"));
            if (session["expires_in"] != "0")
            {
                cookie.Expires = DateTime.Now.AddSeconds(Convert.ToInt32(session["expires_in"]));
            }
            cookie.Value = getQueryString(session);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Gets the Cookie!!
        /// </summary>
        /// <returns></returns>
        protected Dictionary<string, string> getCookie()
        {
            var cookie = HttpContext.Current.Request.Cookies["oauth2_" + getVariable("client_id")];
            if (cookie == null)
            {
                throw new NotImplementedException("need to get back to Auth page");
            }
            var l = HttpUtility.ParseQueryString(cookie.Value);
            var collection = new Dictionary<string, string>();
            foreach (var i in l.AllKeys)
            {
                collection.Add(i, l[i]);
            }
            return collection;
        }

        /// <summary>
        /// Gets the Session Object based off the Access Token
        /// </summary>
        /// <param name="a">Access Token</param>
        /// <returns>Session Variables</returns>
        protected Dictionary<string, string> getSessionObject(AccessToken a)
        {
            var session = new Dictionary<string, string>();
            if (a != null)
            {
                session.Add("access_token", a.access_token);
                session.Add("expires_in", a.expires_in);
                session.Add("refresh_token", a.scope);
                var sig = generateSignature(session, _variables["client_secret"]);
                session.Add("sig", sig);
                return session;
            }
            return null;
        }

        /// <summary>
        /// Validates the Session Object
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        protected Dictionary<string, string> validateSessionObject(Dictionary<string, string> p)
        {
            if (p == null)
            {
                return null;
            }
            var newP = new Dictionary<string, string>(p);
            newP.Remove("sig");
            var expectedSig = generateSignature(newP, _variables["client_secret"]);
            if (expectedSig != p["sig"])
            {
                p = null;
                throw new Exception("NOOOOO MATCH");
            }
            return p;
        }

        /// <summary>
        /// Generates a signature for testing based off parameters and the secret
        /// </summary>
        /// <param name="p"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        protected string generateSignature(Dictionary<string, string> p, string secret)
        {
            string base_string = "";
            foreach (var i in p)
            {
                base_string += i.Key + "=" + i.Value;
            }
            SHA1 sha = new SHA1CryptoServiceProvider();
            byte[] data = sha.ComputeHash(Encoding.UTF8.GetBytes(base_string));
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}