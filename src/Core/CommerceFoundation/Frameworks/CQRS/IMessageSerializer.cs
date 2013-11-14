using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace VirtoCommerce.Foundation.Frameworks.CQRS
{
	public  interface  IMessageSerializer
	{
		void Serialize(object message, Stream destinationStream);

		object Deserialize(Stream sourceStream, Type type);
	}
}
