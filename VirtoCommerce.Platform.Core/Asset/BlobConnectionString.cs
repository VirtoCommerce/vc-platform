using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Asset
{
    /// <summary>
    /// Represent blob resources connection string which can contains complex part as Provider, basepath etc
    /// </summary>
    public class BlobConnectionString
    {
        private BlobConnectionString()
        {
        }
        public string Provider { get; private set; }
        /// <summary>
        /// Chroot path. Base for blob resources
        /// </summary>
        public string RootPath { get; private set; }
        /// <summary>
        /// Blob public base url which blobs can be gets
        /// </summary>
        public string PublicUrl { get; private set; }

        /// <summary>
        /// Rest part of connection string without Provider, RootPath and PublicUrl parameters
        /// </summary>
        public string ConnectionString { get; private set; }

        public static BlobConnectionString Parse(string inputString)
        {
            var retVal = new BlobConnectionString();
            var pairs = ToDictionary(inputString, ";", "=");
            if (pairs.ContainsKey("provider"))
            {
                retVal.Provider = pairs["provider"];
                pairs.Remove("provider");
            }
            if (pairs.ContainsKey("rootPath"))
            {
                retVal.RootPath = pairs["rootPath"];
                pairs.Remove("rootPath");
            }
            if (pairs.ContainsKey("publicUrl"))
            {
                retVal.PublicUrl = pairs["publicUrl"];
                pairs.Remove("publicUrl");
            }
            retVal.ConnectionString = ToString(pairs, ";", "=");
            return retVal;
        }

        private static Dictionary<string, string> ToDictionary(string sourceString, string pairSeparator, string valueSeparator)
        {
            return sourceString.Split(new[] { pairSeparator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(new[] { valueSeparator }, 2, StringSplitOptions.RemoveEmptyEntries))
                .ToDictionary(x => x[0], x => x.Length == 2 ? x[1] : string.Empty, StringComparer.OrdinalIgnoreCase);
        }

        public static string ToString(IEnumerable<KeyValuePair<string, string>> pairs, string pairSeparator, string valueSeparator)
        {
            var builder = new StringBuilder();

            foreach (var pair in pairs)
            {
                if (builder.Length > 0)
                {
                    builder.Append(pairSeparator);
                }
                builder.Append(pair.Key);
                builder.Append(valueSeparator);
                builder.Append(pair.Value);
            }

            return builder.ToString();
        }
    }
}
