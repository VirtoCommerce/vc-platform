using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using VirtoCommerce.Platform.Core.Settings;
using coreModel = VirtoCommerce.Domain.Store.Model;
namespace VirtoCommerce.StoreModule.Web.Model
{
	public class Setting
	{
		public string GroupName { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
		[JsonConverter(typeof(StringEnumConverter))]
		public SettingValueType ValueType { get; set; }
		public string[] AllowedValues { get; set; }
		public string DefaultValue { get; set; }
		public bool IsArray { get; set; }
		public string[] ArrayValues { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
	}
}
