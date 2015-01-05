using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Domain.Order.Model;

namespace VirtoCommerce.Domain.Order.Services
{
	public interface IOperationService 
	{
		Operation GetById(string id);
		Operation Create(Operation operation);
		void Update(Operation[] operations);
		void Delete(string[] ids);
	}
}
