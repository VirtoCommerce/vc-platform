using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.MerchandisingModule.Web.Model
{
	public class PropertyDictionary : Dictionary<string, object>
	{
		public void Add(KeyValuePair<string, object> pair)
		{
			this.Add(pair.Key, pair.Value);
		}
	}
}
