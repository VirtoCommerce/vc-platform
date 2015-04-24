using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Data.Infrastructure
{
	[AttributeUsageAttribute(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public class ParentAttribute : Attribute
	{
	}
}
