using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.OrderModule.Model;

namespace VirtoCommerce.OrderModule.Services
{
	public interface IOrderService : IGenericCrudService<Order>
	{
		Order GetById(string orderId, OrderResponseGroup respGroup);
	}
}
