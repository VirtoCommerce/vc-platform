using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.DynamicProperties
{
	public class DynamicObjectProperty : DynamicProperty
	{
		public string ObjectId { get; set; }
		public ICollection<DynamicPropertyObjectValue> Values { get; set; }
	}
}
