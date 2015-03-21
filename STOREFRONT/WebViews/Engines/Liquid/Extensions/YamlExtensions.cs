#region
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using YamlDotNet.RepresentationModel;
using YamlDotNet.RepresentationModel.Serialization;

#endregion

namespace VirtoCommerce.Web.Views.Engines.Liquid.Extensions
{

    #region
    #endregion

    public static class YamlExtensions
    {
        #region Static Fields
        private static readonly Regex r = new Regex(@"(?s:^---(.*?)---)");
        #endregion

        #region Public Methods and Operators
        public static string ExcludeHeader(this string text)
        {
            var m = r.Matches(text);
            if (m.Count == 0)
            {
                return text;
            }

            return text.Replace(m[0].Groups[0].Value, "").Trim();
        }

        public static string ToYaml<T>(this T model)
        {
            var stringWriter = new StringWriter();
            var serializer = new Serializer();
            serializer.Serialize(stringWriter, model, typeof(T));
            return stringWriter.ToString();
        }

        public static IDictionary<string, object> YamlHeader(this string text, bool skipHeader = false)
        {
            StringReader input;
            var results = new Dictionary<string, object>();

            if (!skipHeader)
            {
                var m = r.Matches(text);
                if (m.Count == 0)
                {
                    return results;
                }

                input = new StringReader(m[0].Groups[1].Value);
            }
            else
            {
                input = new StringReader(text);
            }

            var yaml = new YamlStream();
            yaml.Load(input);

            if (yaml.Documents.Count == 0)
            {
                return results;
            }

            var root = yaml.Documents[0].RootNode;

            var collection = root as YamlMappingNode;
            if (collection != null)
            {
                foreach (var entry in collection.Children)
                {
                    var node = entry.Key as YamlScalarNode;
                    if (node != null)
                    {
                        results.Add(node.Value, GetValue(entry.Value));
                    }
                }
            }

            return results;
        }
        #endregion

        #region Methods
        private static object GetValue(YamlNode value)
        {
            var collection = value as YamlMappingNode;
            if (collection != null)
            {
                var results = new Dictionary<string, object>();
                foreach (var entry in collection.Children)
                {
                    var node = entry.Key as YamlScalarNode;
                    if (node != null)
                    {
                        results.Add(node.Value, GetValue(entry.Value));
                    }
                }

                return results;
            }

            var list = value as YamlSequenceNode;
            if (list != null)
            {
                var listResults = new List<string>();
                foreach (var entry in list.Children)
                {
                    var node = entry as YamlScalarNode;
                    if (node != null)
                    {
                        listResults.Add(node.Value);
                    }
                }
                return listResults;
            }

            return value.ToString();
        }
        #endregion
    }
}