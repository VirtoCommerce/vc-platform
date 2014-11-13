using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;

namespace VirtoCommerce.Slab.EventSources
{
	public static class EventSourceExtension
	{
		public static void Write(this EventSource log, int eventCode, object context)
		{
			try
			{
				var type = context.GetType();
				var method =
					log.GetType().GetMethods().FirstOrDefault(x => x.GetCustomAttributes(typeof(EventAttribute), true).Any() && ((EventAttribute)x.GetCustomAttributes(typeof(EventAttribute), true).First()).EventId == eventCode);
				if (method != null)
				{
					var parameters = new List<object>();
					method.GetParameters().ToList().ForEach(value => parameters.Add(type.GetProperty(value.Name).GetValue(context)));
					method.Invoke(log, parameters.ToArray());
				}
			}
			catch (Exception)
			{
				//try log error
			}
			
		}
	}
}
