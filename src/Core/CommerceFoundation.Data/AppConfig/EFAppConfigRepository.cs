using Microsoft.Practices.Unity;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using VirtoCommerce.Foundation.AppConfig;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Data.Infrastructure.Interceptors;

namespace VirtoCommerce.Foundation.Data.AppConfig
{
    public class EFAppConfigRepository : EFRepositoryBase, IAppConfigRepository
    {
        public EFAppConfigRepository()
        {
        }

        public EFAppConfigRepository(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Configuration.AutoDetectChangesEnabled = true;
            Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer<EFAppConfigRepository>(null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EFAppConfigRepository"/> class.
        /// </summary>
        /// <param name="entityFactory">The entity factory.</param>
        /// <param name="interceptors">The interceptors.</param>
        [InjectionConstructor]
        public EFAppConfigRepository(IAppConfigEntityFactory entityFactory, IInterceptor[] interceptors = null)
            : this(AppConfigConfiguration.Instance.Connection.SqlConnectionStringName, entityFactory, interceptors)
        {
        }

        public EFAppConfigRepository(string connectionStringName, IAppConfigEntityFactory entityFactory, IInterceptor[] interceptors = null)
            : base(connectionStringName, entityFactory, interceptors: interceptors)
        {
            Database.SetInitializer(new ValidateDatabaseInitializer<EFAppConfigRepository>());

            Configuration.AutoDetectChangesEnabled = true;
            Configuration.ProxyCreationEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            MapEntity<Setting>(modelBuilder, toTable: "Setting");
            MapEntity<Sequence>(modelBuilder, toTable: "Sequence");
            MapEntity<SystemJob>(modelBuilder, toTable: "SystemJob");
            MapEntity<TaskSchedule>(modelBuilder, toTable: "TaskSchedule");
            MapEntity<Statistic>(modelBuilder, toTable: "Statistic");
            MapEntity<EmailTemplate>(modelBuilder, toTable: "EmailTemplate");
            MapEntity<EmailTemplateLanguage>(modelBuilder, toTable: "EmailTemplateLanguage");
            MapEntity<DisplayTemplateMapping>(modelBuilder, toTable: "DisplayTemplateMapping");
            MapEntity<ObjectLock>(modelBuilder, toTable: "ObjectLock");
            MapEntity<Localization>(modelBuilder, toTable: "Localization");
            MapEntity<SeoUrlKeyword>(modelBuilder, toTable: "SeoUrlKeyword");

            modelBuilder.Entity<EmailTemplate>().HasMany(c => c.EmailTemplateLanguages).WithRequired(p => p.EmailTemplate);

            base.OnModelCreating(modelBuilder);
        }

        #region IAppConfigRepository Members

        public IQueryable<Setting> Settings
        {
            get { return GetAsQueryable<Setting>(); }
        }

        public IQueryable<Sequence> Sequences
        {
            get { return GetAsQueryable<Sequence>(); }
        }

        public IQueryable<SystemJobLogEntry> SystemJobLogEntries
        {
            get { return GetAsQueryable<SystemJobLogEntry>(); }
        }

        public IQueryable<TaskSchedule> TaskSchedules
        {
            get { return GetAsQueryable<TaskSchedule>(); }
        }

        public IQueryable<SystemJob> SystemJobs
        {
            get { return GetAsQueryable<SystemJob>(); }
        }

        public IQueryable<Statistic> Statistics
        {
            get { return GetAsQueryable<Statistic>(); }
        }

        public IQueryable<EmailTemplate> EmailTemplates
        {
            get { return GetAsQueryable<EmailTemplate>(); }
        }

        public IQueryable<EmailTemplateLanguage> EmailTemplateLanguages
        {
            get { return GetAsQueryable<EmailTemplateLanguage>(); }
        }

        public IQueryable<DisplayTemplateMapping> DisplayTemplateMappings
        {
            get { return GetAsQueryable<DisplayTemplateMapping>(); }
        }

        public IQueryable<ObjectLock> ObjectLocks
        {
            get { return GetAsQueryable<ObjectLock>(); }
        }

        public IQueryable<Localization> Localizations
        {
            get { return GetAsQueryable<Localization>(); }
        }

        public IQueryable<SeoUrlKeyword> SeoUrlKeywords
        {
            get { return GetAsQueryable<SeoUrlKeyword>(); }
        }

        #endregion
    }
}
