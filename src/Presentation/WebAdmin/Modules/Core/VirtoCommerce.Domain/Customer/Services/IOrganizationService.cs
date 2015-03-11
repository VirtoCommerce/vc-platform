using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Customer.Model;

namespace VirtoCommerce.Domain.Customer.Services
{
	public interface IOrganizationService
	{
		IEnumerable<Organization> List();
		Organization GetById(string id);
		Organization Create(Organization organization);
		void Update(Organization[] organizations);
		void Delete(string[] ids);
	}
}
