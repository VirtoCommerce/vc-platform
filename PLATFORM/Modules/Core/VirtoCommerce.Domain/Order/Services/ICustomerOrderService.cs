using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Order.Model;

namespace VirtoCommerce.Domain.Order.Services
{
	public interface ICustomerOrderService 
	{
		CustomerOrder GetById(string id, CustomerOrderResponseGroup respGroup);
		CustomerOrder Create(CustomerOrder order);
		CustomerOrder CreateByShoppingCart(string cartId);
		void Update(CustomerOrder[] orders);
		void Delete(string[] ids);
	}
}
