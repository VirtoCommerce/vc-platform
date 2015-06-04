using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Domain.Customer.Model;

namespace VirtoCommerce.CustomerModule.Web.Model
{
	public class Property
	{
		public string Name { get; set; }
		public string Value { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public PropertyValueType ValueType { get; set; }
	}
}
