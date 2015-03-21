using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using coreModel = VirtoCommerce.Domain.Store.Model;
namespace VirtoCommerce.StoreModule.Web.Model
{
	public class StoreSetting
	{
		public string Name { get; set; }
		public string Value { get; set; }
		public string Locale { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public coreModel.SettingValueType ValueType { get; set; }
	}
}
