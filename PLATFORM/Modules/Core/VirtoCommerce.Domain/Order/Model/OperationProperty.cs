using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Order.Model
{
	public class OperationProperty
	{
		public string Name { get; set; }
		public object Value { get; set; }
		public string Locale { get; set; }
		public PropertyValueType ValueType { get; set; }
	}
}
