using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace VirtoCommerce.Domain.Search
{
    /// <summary>
    /// Contains connection parameters to connecting to the search service.
    /// </summary>
    public class SearchConnection : ISearchConnection
    {
        private Dictionary<string, string> _Parameters = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchConnection"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string. Can be something like this: "server=localhost:9200;scope=default", can have additional parameters separated by commas</param>
        public SearchConnection(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString) || connectionString.Equals("SearchConnectionString", StringComparison.OrdinalIgnoreCase))
            {
                connectionString = "server=~/app_data/Virto/search;scope=default;provider=lucene";
            }

            _Parameters = ParseString(connectionString);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchConnection"/> class.
        /// </summary>
        /// <param name="dataSource">The data source.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="provider">The provider.</param>
        public SearchConnection(string dataSource, string scope, string provider = "default", string accessKey = "")
        {
            _Parameters = new Dictionary<string, string>();
            DataSource = dataSource;
            Scope = scope;
            Provider = provider;
            AccessKey = accessKey;
        }

        private Dictionary<string, string> ParseString(string s)
        {
            var nvc = new Dictionary<string, string>();

            // remove anything other than query string from url
            if (s.Contains("?"))
            {
                s = s.Substring(s.IndexOf('?') + 1);
            }

            foreach (string vp in Regex.Split(s, ";"))
            {
                string[] singlePair = Regex.Split(vp, "=");
                if (singlePair.Length == 2)
                {
                    nvc.Add(singlePair[0], singlePair[1]);
                }
                else
                {
                    // only one key with no value specified in query string
                    nvc.Add(singlePair[0], string.Empty);
                }
            }

            return nvc;
        }

        public string DataSource
        {
            get { if (_Parameters != null) return _Parameters["server"];
            
                throw new ArgumentNullException("DataSource must be specified using server parameter for the search connection string");
            }
            private set
            {
                _Parameters.Add("server", value);
            }
        }

        public string AccessKey
        {
            get
            {
                if (_Parameters != null) return _Parameters["key"];

                throw new ArgumentNullException("Key must be specified using server parameter for the search connection string");
            }
            private set
            {
                _Parameters.Add("key", value);
            }
        }

        public string Scope
        {
            get { 
                if (_Parameters != null) return _Parameters["scope"];
                return "default";
            }
            private set
            {
                _Parameters.Add("scope", value);
            }
        }

        public string Provider
        {
            get
            {
                if (_Parameters != null && _Parameters.ContainsKey("provider")) return _Parameters["provider"];
                return "default";
            }
            private set
            {
                _Parameters.Add("provider", value);
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var parameter in _Parameters.Keys)
            {
                builder.AppendFormat("{2}{0}={1}", parameter, _Parameters[parameter], builder.Length > 0 ? ";" : String.Empty);
            }

            return builder.ToString();
        }
    }
}
