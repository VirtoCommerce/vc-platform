using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Domain.Customer.Model;

namespace VirtoCommerce.Domain.Customer.Services
{
	public interface IContactService
	{
		Contact GetById(string id);
		Contact Create(Contact contact);
		void Update(Contact[] contacs);
		void Delete(string[] ids);
	}
}

