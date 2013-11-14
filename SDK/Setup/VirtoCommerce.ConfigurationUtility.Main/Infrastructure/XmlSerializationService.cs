using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VirtoCommerce.ConfigurationUtility.Main.Infrastructure
{
    public static class XmlSerializationService<T> where T : class
    {
        private static XmlSerializer _serializer = new XmlSerializer(typeof(T));

        public static T Deserialize(string serializedObject)
        {
            using (var sr = new StringReader(serializedObject))
            {
                return (T)_serializer.Deserialize(sr);
            }
        }

        public static string Serialize(T objectToSerialize)
        {
            using (var sw = new StringWriter())
            {
                _serializer.Serialize(sw, objectToSerialize);
                return sw.ToString();
            }
        }
    }
}
