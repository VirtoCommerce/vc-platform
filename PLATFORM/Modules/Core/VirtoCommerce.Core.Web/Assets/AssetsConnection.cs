using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Framework.Web.Asset;

namespace VirtoCommerce.CoreModule.Web.Assets
{
    public class AssetsConnection : IAssetsConnection
    {
        public AssetsConnection(string connectionString)
        {
            OriginalConnectionString = connectionString;

            var parameters = StringToDictionary(connectionString, ";", "=");
            Provider = parameters["provider"];
            parameters.Remove("provider");
            ConnectionString = DictionaryToString(parameters, ";", "=");
        }

        #region IAssetsConnection Members

        public string OriginalConnectionString { get; private set; }
        public string Provider { get; private set; }
        public string ConnectionString { get; private set; }

        #endregion


        private static Dictionary<string, string> StringToDictionary(string connectionString, string pairSeparator, string valueSeparator)
        {
            return connectionString.Split(new[] { pairSeparator }, StringSplitOptions.RemoveEmptyEntries)
                .Select(pair => pair.Split(new[] { valueSeparator }, 2, StringSplitOptions.RemoveEmptyEntries))
                .ToDictionary(parts => parts[0], parts => parts.Length == 2 ? parts[1] : string.Empty, StringComparer.OrdinalIgnoreCase);
        }

        private static string DictionaryToString(Dictionary<string, string> dictionary, string pairSeparator, string valueSeparator)
        {
            var builder = new StringBuilder();

            foreach (var pair in dictionary)
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
