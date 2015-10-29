using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Notifications
{
	public class NotificationParameter
	{
		public string ParameterName { get; set; }
		public string ParameterDescription { get; set; }
		public string ParameterCodeInView { get; set; }
        public bool IsDictionary { get; set; }
        public bool IsArray { get; set; }
        public NotificationParameterValueType Type { get; set; }
	}
}
