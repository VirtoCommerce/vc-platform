using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using VirtoCommerce.Platform.Data.Infrastructure;
using VirtoCommerce.Platform.Data.Infrastructure.Interceptors;
using VirtoCommerce.CoreModule.Data.Model;

namespace VirtoCommerce.CoreModule.Data.Repositories
{
	public class CommerceRepositoryImpl : EFRepositoryBase, IСommerceRepository
	{
		public CommerceRepositoryImpl()
		{
		}

		public CommerceRepositoryImpl(string nameOrConnectionString)
			: this(nameOrConnectionString, null)
		{
		}
		public CommerceRepositoryImpl(string nameOrConnectionString, params IInterceptor[] interceptors)
			: base(nameOrConnectionString, null, interceptors)
		{

		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<FulfillmentCenter>().HasKey(x => x.Id).Property(x => x.Id)
											.HasColumnName("FulfillmentCenterId");

			MapEntity<FulfillmentCenter>(modelBuilder, toTable: "FulfillmentCenter");

			base.OnModelCreating(modelBuilder);
		}

		#region IСommerceRepository Members

		public IQueryable<FulfillmentCenter> FulfillmentCenters
		{
			get { return GetAsQueryable<FulfillmentCenter>(); }
		}

		#endregion
	}

}
