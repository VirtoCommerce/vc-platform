using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Data.Infrastructure
{
	[AttributeUsageAttribute(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public class NotificationParameterAttribute : Attribute
	{
		private string _description;

		public NotificationParameterAttribute(string description)
		{
			_description = description;
		}
		
		public virtual string Description
		{
			get { return _description; }
		}
	}
}
