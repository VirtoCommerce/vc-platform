using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Common.Expressions
{
	public class ExpressionElement
	{
		public ExpressionElement()
		{
			Id = this.GetType().Name;
		}
		public string Id { get; set; }
		public string Name { get; set; }
		public bool IsValid { get; set; }
		public string Error { get; set; }
	}
}
