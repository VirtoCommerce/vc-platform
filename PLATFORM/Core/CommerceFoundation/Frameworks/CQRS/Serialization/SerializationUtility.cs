using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.Frameworks.CQRS.Messages;
using System.IO;

namespace VirtoCommerce.Foundation.Frameworks.CQRS.Serialization
{
	public static class SerializationUtility
	{
		public static byte[] Serialize(IMessageSerializer serializer, object instance)
		{
			using (var stream = new MemoryStream())
			{
				serializer.Serialize(instance, stream);
				stream.Seek(0, SeekOrigin.Begin);
				return stream.ToArray();
			}
		}

		public static T Deserialize<T>(IMessageSerializer serializer, byte[] buffer) where T : class
		{
			return Deserialize(typeof(T), serializer, buffer) as T;
		}


		public static object Deserialize(Type type, IMessageSerializer serializer, byte[] buffer)
		{
			using (var memory = new MemoryStream(buffer))
			{
				memory.Seek(0, SeekOrigin.Begin);
				return serializer.Deserialize(memory, type);
			}
		}

	}
}
