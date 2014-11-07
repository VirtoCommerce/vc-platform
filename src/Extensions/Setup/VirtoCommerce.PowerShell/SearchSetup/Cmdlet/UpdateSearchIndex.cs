using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Management.Automation;
using System.Reflection;
using VirtoCommerce.Caching.HttpCache;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Data.Catalogs;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Data.Reviews;
using VirtoCommerce.Foundation.Data.Search;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.CQRS;
using VirtoCommerce.Foundation.Frameworks.CQRS.Engines;
using VirtoCommerce.Foundation.Frameworks.CQRS.Factories;
using VirtoCommerce.Foundation.Frameworks.CQRS.Senders;
using VirtoCommerce.Foundation.Frameworks.CQRS.Serialization;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Frameworks.Logging;
using VirtoCommerce.Foundation.Frameworks.Logging.Factories;
using VirtoCommerce.Foundation.Reviews.Factories;
using VirtoCommerce.Foundation.Reviews.Repositories;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.Foundation.Search.CQRS;
using VirtoCommerce.Foundation.Search.Factories;
using VirtoCommerce.Foundation.Search.Repositories;
using VirtoCommerce.PowerShell.Cmdlet;
using VirtoCommerce.Search.Index;
using VirtoCommerce.Search.Providers.Elastic;

namespace VirtoCommerce.PowerShell.SearchSetup.Cmdlet
{
    using VirtoCommerce.Foundation.Catalogs;
    using VirtoCommerce.Search.Providers.Azure;
    using VirtoCommerce.Search.Providers.Lucene;

    [CLSCompliant(false)]
    [Cmdlet(VerbsData.Update, "Virto-Search-Index", SupportsShouldProcess = true)]
    public class UpdateSearchIndex : DomainCommand
    {
        #region Public Properties
        [Parameter(Mandatory = false, HelpMessage = "Connection string.")]
        [ValidateNotNullOrEmpty]
        public string Connection { get; set; }

        [Parameter(Mandatory = false, HelpMessage = "Database connection string.")]
        [ValidateNotNullOrEmpty]
        public string DbConnection { get; set; }

        [Alias("type")]
        [Parameter(Mandatory = false, HelpMessage = "Specify document type.")]
        public string DocumentType { get; set; }

        [Parameter(HelpMessage = "Rebuild the index")]
        public SwitchParameter Rebuild { get; set; }
        #endregion

        public virtual void Index(SearchConnection searchConnection, string dbConnectionString, string documentType, bool rebuild)
        {
            SafeWriteVerbose("Server: " + searchConnection);
            IServiceLocator serviceLocatorBackup = null;
            try
            {
                // backup existing LocatorProvider
                if (ServiceLocator.IsLocationProviderSet) serviceLocatorBackup = ServiceLocator.Current;

                var controller = GetLocalSearchController(searchConnection, dbConnectionString);

                SafeWriteVerbose("Preparing workload");
                controller.Prepare(scope: searchConnection.Scope, documentType: documentType, rebuild: rebuild);

                SafeWriteVerbose("Processing workload");
                controller.Process(scope: searchConnection.Scope, documentType: documentType);

                // Multi threaded processing
                //IndexProcess(searchConnection, dbConnectionString, documentType);
            }
            catch (Exception ex)
            {
                SafeThrowError(ex);
            }
            finally
            {
                // reseting original LocatorProvider
                if (serviceLocatorBackup != null)
                    ServiceLocator.SetLocatorProvider(() => serviceLocatorBackup);
            }
        }

        /*
        private void IndexProcess(SearchConnection searchConnection, string dbConnectionString,
                                        string documentType)
        {
            SafeWriteVerbose("Processing workload - 5 threads in parallel");
            var controller = GetLocalSearchController(searchConnection, dbConnectionString);
            Parallel.For(0, 5, (i, loopState) => controller.Process(scope: searchConnection.Scope, documentType: documentType));
        }
         */

        /// <summary>
        /// Execute the command.
        /// </summary>
        protected override void ProcessRecord()
        {
            try
            {
                SafeWriteVerbose("Version: " + Assembly.GetExecutingAssembly().GetFileVersion());
                base.ProcessRecord();
                Index(new SearchConnection(Connection), DbConnection, DocumentType, Rebuild);
                SafeWriteVerbose("Index updated!");
            }
            catch (Exception ex)
            {
                SafeWriteError(new ErrorRecord(ex, string.Empty, ErrorCategory.CloseError, null));
            }
        }

        #region Container Helpers
        private UnityContainer GetLocalContainer(SearchConnection searchConnection, string connectionString)
        {
            var container = new UnityContainer();
            container.RegisterType<IKnownSerializationTypes, CatalogEntityFactory>("catalog", new ContainerControlledLifetimeManager());
            container.RegisterInstance<IConsumerFactory>(new DomainAssemblyScannerConsumerFactory(container));
            container.RegisterType<IKnownSerializationTypes, DomainAssemblyScannerConsumerFactory>("scaned", new ContainerControlledLifetimeManager(), new InjectionConstructor(container));
            container.RegisterType<IConsumerFactory, DomainAssemblyScannerConsumerFactory>();
            container.RegisterType<IEngineProcess, SingleThreadConsumingProcess>();
            container.RegisterType<IMessageSerializer, DataContractMessageSerializer>();
            container.RegisterType<IQueueWriter, InMemoryQueueWriter>();
            container.RegisterType<IQueueReader, InMemoryQueueReader>();
            container.RegisterType<IMessageSender, DefaultMessageSender>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICatalogEntityFactory, CatalogEntityFactory>(new ContainerControlledLifetimeManager());
            container.RegisterType<IReviewEntityFactory, ReviewEntityFactory>(new ContainerControlledLifetimeManager());

            container.RegisterType<ICatalogService, CatalogService>();
            container.RegisterType<ISearchIndexBuilder, CatalogItemIndexBuilder>("catalogitem");
            container.RegisterType<ILogOperationFactory, LogOperationFactory>();
            container.RegisterType<ISearchEntityFactory, SearchEntityFactory>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISearchIndexController, SearchIndexController>();
            container.RegisterType<ICacheRepository, HttpCacheRepository>();

            if (string.Equals(searchConnection.Provider, SearchProviders.Lucene.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                // Lucene Search implementation
                container.RegisterType<ISearchProvider, LuceneSearchProvider>();
                container.RegisterType<ISearchQueryBuilder, LuceneSearchQueryBuilder>();
            }
            else if (string.Equals(searchConnection.Provider, SearchProviders.AzureSearch.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                // Azure Search implementation
                container.RegisterType<ISearchProvider, AzureSearchProvider>();
                container.RegisterType<ISearchQueryBuilder, AzureSearchQueryBuilder>();
            }
            else
            {
                container.RegisterType<ISearchProvider, ElasticSearchProvider>();
                container.RegisterType<ISearchQueryBuilder, ElasticSearchQueryBuilder>();
            }


            // register instances here
            container.RegisterInstance<ISearchConnection>(searchConnection);

            var catalogRepository = new EFCatalogRepository(connectionString);
            container.RegisterInstance<ICatalogRepository>(catalogRepository);
            container.RegisterType<ICatalogOutlineBuilder, CatalogOutlineBuilder>();
            container.RegisterInstance<IPricelistRepository>(catalogRepository);
            container.RegisterInstance<IOperationLogRepository>(new OperationLogContext(connectionString));
            container.RegisterInstance<IBuildSettingsRepository>(new EFSearchRepository(connectionString));

            var reviewRepository = new EFReviewRepository(connectionString);
            container.RegisterInstance<IReviewRepository>(reviewRepository);


            var indexingProgress = new ProgressRecord(1, "Indexing Progress", "Progress:");
            var observer = new ProgressObserver(this, indexingProgress);
            container.RegisterInstance<ISystemObserver>(observer);

            return container;
        }

        protected ISearchIndexController GetLocalSearchController(SearchConnection searchConnection, string connectionString)
        {
            var container = GetLocalContainer(searchConnection, connectionString);
            var locator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => locator);
            var controller = container.Resolve<ISearchIndexController>();

            return controller;
        }
        #endregion
    }
}