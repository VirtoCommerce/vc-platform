using System.Linq;
using System.Data.Entity;
using VirtoCommerce.Foundation.Search.Repositories;
using VirtoCommerce.Foundation.Search.Factories;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Search.Model;
using Microsoft.Practices.Unity;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.Foundation.Data.Search
{
	public class EFSearchRepository : EFRepositoryBase, IBuildSettingsRepository
	{
		private readonly ISearchEntityFactory _entityFactory;

        public EFSearchRepository()
        {
        }

        public EFSearchRepository(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer<EFSearchRepository>(null);
        }

        [InjectionConstructor]
		public EFSearchRepository(ISearchEntityFactory entityFactory, IInterceptor[] interceptors = null)
			: base(SearchConfiguration.Instance.Connection.SqlConnectionStringName, interceptors: interceptors)
		{
			_entityFactory = entityFactory;

            Database.SetInitializer(new ValidateDatabaseInitializer<EFSearchRepository>());

			this.Configuration.AutoDetectChangesEnabled = true;
			this.Configuration.ProxyCreationEnabled = false;
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			MapEntity<BuildSettings>(modelBuilder, toTable: "BuildSetting");
			base.OnModelCreating(modelBuilder);
		}

		#region ISecurityRepository Members

		public IQueryable<BuildSettings> BuildSettings
		{
			get { return GetAsQueryable<BuildSettings>(); }
		}
		#endregion
	}
}
