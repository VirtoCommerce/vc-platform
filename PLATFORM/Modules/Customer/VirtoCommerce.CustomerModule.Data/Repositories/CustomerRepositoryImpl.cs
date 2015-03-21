using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;
using VirtoCommerce.Foundation.Data.Stores;
using System.Data.Entity;
using VirtoCommerce.Foundation.Data.Customers;
using VirtoCommerce.Foundation.Customers.Model;

namespace VirtoCommerce.CustomerModule.Data.Repositories
{
	public class FoundationCustomerRepositoryImpl : EFCustomerRepository, IFoundationCustomerRepository
	{
		public FoundationCustomerRepositoryImpl(string nameOrConnectionString)
			: this(nameOrConnectionString, null)
		{
		}
		public FoundationCustomerRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
			: base(nameOrConnectionString, null, interceptors)
		{

		}

		#region IFoundationCustomerRepository Members

		public Contact GetContactById(string id)
		{
			var query = Members.Where(x => x.MemberId == id)
							   .OfType<Contact>()
							   .Include(x => x.Notes)
							   .Include(x => x.Emails)
							   .Include(x => x.ContactPropertyValues)
							   .Include(x => x.Addresses)
							   .Include(x => x.Phones)
							   .Include(x => x.MemberRelations.Select(y=>y.Ancestor));

			return query.FirstOrDefault();
		}

		public Organization GetOrganizationById(string id)
		{
			var query = Members.Where(x => x.MemberId == id)
							   .OfType<Organization>()
							   .Include(x => x.Notes)
							   .Include(x => x.Emails)
							   .Include(x => x.Addresses)
							   .Include(x => x.Phones)
							   .Include(x => x.MemberRelations);

			return query.FirstOrDefault();

		}
	
		#endregion
	}

}
