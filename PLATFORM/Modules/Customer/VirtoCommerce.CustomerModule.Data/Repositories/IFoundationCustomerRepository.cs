using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Customers.Repositories;
using foundation = VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.CustomerModule.Data.Repositories
{
	public interface IFoundationCustomerRepository : ICustomerRepository
	{
		foundation.Contact GetContactById(string id);
		foundation.Organization GetOrganizationById(string id);
	}
}
