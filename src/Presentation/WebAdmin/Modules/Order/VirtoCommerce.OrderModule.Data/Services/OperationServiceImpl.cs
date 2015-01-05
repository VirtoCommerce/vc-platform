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
	public class OperationServiceImpl : IOperationService
	{
		private IInventoryService _inventoryService;
		public OperationServiceImpl(IInventoryService inventoryService)
		{
			_inventoryService = inventoryService;
		}
		#region IOperationService Members

		public Operation GetById(string id)
		{
			throw new NotImplementedException();
		}

		public Operation Create(Operation operation)
		{
			throw new NotImplementedException();
		}

		public void Update(Operation[] operations)
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
