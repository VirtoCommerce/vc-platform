using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace VirtoCommerce.Foundation.Frameworks.CQRS
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class MessageTypeAttribute : Attribute
	{
		public Type SupportMessageType { get; set; }
		public MessageTypeAttribute(Type supportMessageType)
		{
			SupportMessageType = supportMessageType;
		}
		
	}
}
