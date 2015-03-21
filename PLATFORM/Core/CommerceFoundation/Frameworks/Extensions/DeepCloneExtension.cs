using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using VirtoCommerce.Foundation.Frameworks;

namespace VirtoCommerce.Foundation.Frameworks.Extensions
{
	public static class DeepCloneExtension
	{
		public static T DeepClone<T>(this T obj, IKnownSerializationTypes knownTypes)
		{
			if (knownTypes == null)
			{
				throw new ArgumentNullException("knownTypes");
			}

			T cloned = default(T);
			var serializer = new DataContractSerializer(typeof(T), knownTypes.GetKnownTypes(), maxItemsInObjectGraph: 0x7FFF,
			                                            ignoreExtensionDataObject : false,
			                                            preserveObjectReferences : true,
			                                            dataContractSurrogate : null);
			using (var ms = new System.IO.MemoryStream())
			{
				serializer.WriteObject(ms, obj);
				ms.Seek(0, System.IO.SeekOrigin.Begin);
				cloned = (T)serializer.ReadObject(ms);
			}
			return cloned;
		}
	}
}
