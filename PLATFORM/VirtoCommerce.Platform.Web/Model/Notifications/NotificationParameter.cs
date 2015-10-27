using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtoCommerce.Platform.Core.Notifications;

namespace VirtoCommerce.Platform.Web.Model.Notifications
{
	public class NotificationParameter
	{
		public string ParameterName { get; set; }

		/// <summary>
		/// Parameter description, can be used for display detailed information about parameter
		/// </summary>
		public string ParameterDescription { get; set; }

		/// <summary>
		/// Code template for notification parameter for template resolver
		/// </summary>
		public string ParameterCodeInView { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDictionary { get; set; }

        public bool IsArray { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public NotificationParameterValueType Type { get; set; }
    }
}