using Microsoft.Practices.Unity;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using VirtoCommerce.Foundation.AppConfig;
using VirtoCommerce.Foundation.Frameworks.Logging;
using VirtoCommerce.Foundation.Frameworks.Logging.Factories;

namespace VirtoCommerce.Foundation.Data.Infrastructure
{
	public class OperationLogContext : EFRepositoryBase, IOperationLogRepository
	{
		private const string TableName = "__OperationLogs";

		public OperationLogContext()
		{
		}

		public OperationLogContext(string nameOrConnectionString)
			: base(nameOrConnectionString)
		{
			Database.SetInitializer(new ValidateDatabaseInitializer<OperationLogContext>());
			this.Configuration.AutoDetectChangesEnabled = true;
			this.Configuration.ProxyCreationEnabled = false;
		}

		[InjectionConstructor]
		public OperationLogContext(ILogOperationFactory entityFactory)
			: base(AppConfigConfiguration.Instance.Connection.SqlConnectionStringName, factory: entityFactory)
		{
			Database.SetInitializer(new ValidateDatabaseInitializer<OperationLogContext>());
			this.Configuration.AutoDetectChangesEnabled = true;
			this.Configuration.ProxyCreationEnabled = false;
		}


		public OperationLogContext(DbConnection existingConnection) : base(existingConnection)
		{
			Database.SetInitializer(new ValidateDatabaseInitializer<OperationLogContext>());
			this.Configuration.AutoDetectChangesEnabled = true;
			this.Configuration.ProxyCreationEnabled = false;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
			modelBuilder.Entity<OperationLog>().ToTable(TableName);            
			base.OnModelCreating(modelBuilder);
		}

		public IQueryable<OperationLog> OperationLogs
		{
			get { return GetAsQueryable<OperationLog>(); }
		}

		public override int SaveChanges()
		{
			return base.SaveChanges();
		}
	}
}