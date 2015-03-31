using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.Domain.Common.Expressions
{
	public class CompositeElement : ExpressionElement
	{
		public bool IsAllowChildren { get; set; }
		public IEnumerable<ExpressionElement> Children { get; set; }
		public IEnumerable<ExpressionElement> AvailableChildren { get; set; }
		
	}
}
