using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace VirtoCommerce.Foundation.Frameworks.Common
{
	public static class GenericXmlSerializer
	{
		#region DeserializeFromString
		/// <summary>
		/// Gets the object.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static T DeserializeFromString<T>(string value)
		{
			if (string.IsNullOrEmpty(value))
				return default(T);
			XmlSerializer xmlsz = new XmlSerializer(typeof(T));
			using (MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(value)))
			{
				return (T)xmlsz.Deserialize(ms);
			}
		}

		#endregion

		#region SerializeToXmlString
		/// <summary>
		/// Gets the string.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static string SerializeToXmlString<T>(T value)
		{
			if (value == null)
				return string.Empty;

			XmlSerializer xmlsz = new XmlSerializer(typeof(T));
			using (MemoryStream ms = new MemoryStream())
			{
				xmlsz.Serialize(ms, value);
				return System.Text.Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Length);
			}
		}
		#endregion
	}

}
