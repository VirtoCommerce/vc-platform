using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Inventory.Services;
using VirtoCommerce.Domain.Order.Model;
using VirtoCommerce.Domain.Order.Services;

namespace VirtoCommerce.OrderModule.Data.Services
{
	public class CustomerOrderServiceImpl : ICustomerOrderService
	{
		private IInventoryService _inventoryService;
		public CustomerOrderServiceImpl(IInventoryService inventoryService)
		{
			_inventoryService = inventoryService;
		}

		#region ICustomerOrderService Members

		public CustomerOrder GetById(string id)
		{
			throw new NotImplementedException();
		}

		public CustomerOrder Create(CustomerOrder order)
		{
			throw new NotImplementedException();
		}

		public void Update(CustomerOrder[] orders)
		{
			throw new NotImplementedException();
		}

		public void Delete(string[] ids)
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
