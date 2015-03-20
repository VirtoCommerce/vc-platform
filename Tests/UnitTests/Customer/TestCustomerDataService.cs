using System;
using System.Collections.Generic;
using System.Data.Services;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.Foundation.Security.Factories;
using VirtoCommerce.Foundation.Data.Customers;
using VirtoCommerce.Foundation.Customers.Factories;

namespace CommerceFoundation.UnitTests.Customer
{
	public class TestCustomerDataService : DataService<EFCustomerRepository>
	{

		public static void InitializeService(DataServiceConfiguration config)
		{
			config.SetEntitySetAccessRule("*", EntitySetRights.All);
			config.SetServiceOperationAccessRule("*", ServiceOperationRights.All);
			config.DataServiceBehavior.MaxProtocolVersion = System.Data.Services.Common.DataServiceProtocolVersion.V3;
			config.DataServiceBehavior.AcceptCountRequests = true;
			config.DataServiceBehavior.AcceptProjectionRequests = true;
			config.UseVerboseErrors = true;

		}


		protected override EFCustomerRepository CreateDataSource()
		{
			return new EFCustomerRepository("VirtoCommerce", new CustomerEntityFactory());
		}
	}
}
