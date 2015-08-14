using System;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace VirtoCommerce.Platform.Core.Common
{
    public static class SerializationExtension
    {
        /// <summary>
        /// Extension method that takes objects and serialized them.
        /// </summary>
        /// <typeparam name="T">The type of the object to be serialized.</typeparam>
        /// <param name="source">The object to be serialized.</param>
        /// <returns>A string that represents the serialized XML.</returns>
        public static string SerializeXml<T>(this T source) where T : class, new()
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            var serializer = new XmlSerializer(typeof(T));
            using (var writer = new StringWriter())
            {
                serializer.Serialize(writer, source);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Extension method to string which attempts to deserialize XML with the same name as the source string.
        /// </summary>
        /// <typeparam name="T">The type which to be deserialized to.</typeparam>
        /// <param name="xml">The source string</param>
        /// <returns>The deserialized object, or null if unsuccessful.</returns>
        public static T DeserializeXml<T>(this string xml) where T : class, new()
        {
            if (xml == null)
            {
                throw new ArgumentNullException("xml");
            }

            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StringReader(xml))
            {
                return (T)serializer.Deserialize(reader);

            }
        }

        private static JsonSerializer GetSerializer()
        {
            var serializer = new JsonSerializer
            {
                TypeNameHandling = TypeNameHandling.Auto,
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Full,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            return serializer;
        }

        public static void SerializeJson<T>(this T source, Stream backupStream) where T : class, new()
        {
            if (backupStream == null)
            {
                throw new ArgumentNullException("backupStream");
            }

            var serializer = GetSerializer();

            using (var streamWriter = new StreamWriter(backupStream, Encoding.UTF8, 1024, true) { AutoFlush = true })
            {
                using (JsonWriter writer = new JsonTextWriter(streamWriter))
                {
                    serializer.Serialize(writer, source);
                }
            }
        }

        public static T DeserializeJson<T>(this Stream backupStream) where T : class, new()
        {
            var serializer = GetSerializer();

            using (var streamReader = new StreamReader(backupStream, Encoding.UTF8))
            {
                using (JsonReader reader = new JsonTextReader(streamReader))
                {
                    var result = serializer.Deserialize<T>(reader);
                    return result;
                }
            }
        }
    }
}
