using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtoCommerce.OrderModule.Model
{
	public interface IRecalculationRule
	{
		void Recalculate(Order order);
	}
}
