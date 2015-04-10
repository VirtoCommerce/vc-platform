using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Domain.Marketing.Model;

namespace VirtoCommerce.MarketingModule.Web.Model
{
	public class Property
	{
		public string Name { get; set; }
		public object Value { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public PropertyValueType ValueType { get; set; }
	}
}
