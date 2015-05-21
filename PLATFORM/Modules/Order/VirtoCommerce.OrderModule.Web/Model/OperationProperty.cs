using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.OrderModule.Web.Model
{
	public class OperationProperty
	{
		public string Name { get; set; }
		public string Value { get; set; }
		public string Locale { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public PropertyValueType ValueType { get; set; }
	}
}
