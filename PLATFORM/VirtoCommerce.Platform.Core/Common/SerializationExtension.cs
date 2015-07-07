using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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
		public static string SerializeXML<T>(this T source) where T : class, new()
		{
			if(source == null)
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
		/// <param name="XML">The source string</param>
		/// <returns>The deserialized object, or null if unsuccessful.</returns>
		public static T DeserializeXML<T>(this string xml) where T : class, new()
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
	}
}
