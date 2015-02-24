using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using VirtoCommerce.Foundation.Data.Stores;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;


namespace VirtoCommerce.CoreModule.Web.Repositories
{
	public class FoundationFulfillmentRepositoryImpl : EFStoreRepository, IFoundationFulfillmentRepository
	{
		public FoundationFulfillmentRepositoryImpl(string nameOrConnectionString)
			: this(nameOrConnectionString, null)
		{
		}
		public FoundationFulfillmentRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
			: base(nameOrConnectionString, null, interceptors)
		{

		}
	}

}
