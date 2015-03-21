using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Store.Model
{
	public class StoreSetting
	{
		public string Name { get; set; }
		public object Value { get; set; }
		public string Locale { get; set; }
		public SettingValueType ValueType { get; set; }
	}
}
