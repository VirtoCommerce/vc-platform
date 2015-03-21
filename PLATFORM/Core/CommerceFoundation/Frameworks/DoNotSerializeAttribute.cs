using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VirtoCommerce.Foundation.Frameworks
{
	[AttributeUsage(AttributeTargets.Property)]
	public class DoNotSerializeAttribute : Attribute
	{
	}
}
