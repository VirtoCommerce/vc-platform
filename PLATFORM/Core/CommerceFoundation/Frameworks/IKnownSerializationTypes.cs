using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks
{
	public interface IKnownSerializationTypes
	{
		IEnumerable<Type> GetKnownTypes();
	}
}
