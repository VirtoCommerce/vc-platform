using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json.Utilities;

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

        public static void SerializeJson<T>(this T source, Stream stream) where T : class, new()
        {
            source.SerializeJson<T>(stream, GetDefaultSerializer());
        }

        public static void SerializeJson<T>(this T source, Stream stream, JsonSerializer serializer) where T : class, new()
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            if (serializer == null)
            {
                throw new ArgumentNullException("serializer");
            }

            using (var streamWriter = new StreamWriter(stream, Encoding.UTF8, 1024, true) { AutoFlush = true })
            {
                using (JsonWriter writer = new JsonTextWriter(streamWriter))
                {
                    serializer.Serialize(writer, source);
                }
            }
        }

        public static T DeserializeJson<T>(this Stream stream) where T : class, new()
        {
            return stream.DeserializeJson<T>(GetDefaultSerializer());
        }

        public static T DeserializeJson<T>(this Stream stream, JsonSerializer serializer) where T : class, new()
        {
            if (stream == null)
            {
                throw new ArgumentNullException("stream");
            }
            if (serializer == null)
            {
                throw new ArgumentNullException("serializer");
            }
            using (var streamReader = new StreamReader(stream, Encoding.UTF8))
            {
                using (JsonReader reader = new JsonTextReader(streamReader))
                {
                    var result = serializer.Deserialize<T>(reader);
                    return result;
                }
            }
        }

        public static void CopyTo(this JsonSerializerSettings source, JsonSerializerSettings destination)
        {
            if (!source.Converters.IsNullOrEmpty())
            {
                for (var i = 0; i < source.Converters.Count; i++)
                {
                    destination.Converters.Insert(i, source.Converters[i]);
                }
            }

            destination.TypeNameHandling = source.TypeNameHandling;
            destination.MetadataPropertyHandling = source.MetadataPropertyHandling;
            destination.TypeNameAssemblyFormatHandling = source.TypeNameAssemblyFormatHandling;
            destination.PreserveReferencesHandling = source.PreserveReferencesHandling;
            destination.ReferenceLoopHandling = source.ReferenceLoopHandling;
            destination.MissingMemberHandling = source.MissingMemberHandling;
            destination.ObjectCreationHandling = source.ObjectCreationHandling;
            destination.NullValueHandling = source.NullValueHandling;
            destination.DefaultValueHandling = source.DefaultValueHandling;
            destination.ConstructorHandling = source.ConstructorHandling;
            destination.Context = source.Context;
            destination.CheckAdditionalContent = source.CheckAdditionalContent;
            destination.Error = source.Error;
            destination.ContractResolver = source.ContractResolver;
            destination.ReferenceResolverProvider = source.ReferenceResolverProvider;
            destination.TraceWriter = source.TraceWriter;
            destination.EqualityComparer = source.EqualityComparer;
            destination.SerializationBinder = source.SerializationBinder;
            destination.Formatting = source.Formatting;
            destination.DateFormatHandling = source.DateFormatHandling;
            destination.DateTimeZoneHandling = source.DateTimeZoneHandling;
            destination.DateParseHandling = source.DateParseHandling;
            destination.DateFormatString = source.DateFormatString;
            destination.FloatFormatHandling = source.FloatFormatHandling;
            destination.FloatParseHandling = source.FloatParseHandling;
            destination.StringEscapeHandling = source.StringEscapeHandling;
            destination.Culture = source.Culture;
            destination.MaxDepth = source.MaxDepth;
        }

        private static JsonSerializer GetDefaultSerializer()
        {
            var serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                TypeNameHandling = TypeNameHandling.Auto,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full

            };
            return serializer;
        }
    }
}
