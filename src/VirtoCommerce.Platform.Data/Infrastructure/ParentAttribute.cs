using System;

namespace VirtoCommerce.Platform.Data.Infrastructure
{
    [AttributeUsageAttribute(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public class ParentAttribute : Attribute
	{
	}
}
