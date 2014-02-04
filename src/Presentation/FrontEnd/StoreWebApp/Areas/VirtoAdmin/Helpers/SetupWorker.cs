using System;
using System.Diagnostics;
using System.IO;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet;
using VirtoCommerce.PowerShell.SearchSetup.Cmdlet;
using VirtoCommerce.Web.Areas.VirtoAdmin.Models;

namespace VirtoCommerce.Web.Areas.VirtoAdmin.Helpers
{
    public class SetupWorker : Hub
    {
        public static void SendMessage(string msg, params object[] format)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SetupWorker>();
            context.Clients.All.traceMessage(string.Format(msg,format));
        }

        public static void SendMessageLine(string msg, params object[] format)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SetupWorker>();
            context.Clients.All.traceMessage(string.Format(msg, format), true);
        }

        public static void DisplayErrorMessage(string msg, string selector = ".validation-summary-errors")
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SetupWorker>();
            context.Clients.All.otherMessage(msg, selector);
        }

        public static void DisplayInfoMessage(string msg, string selector = ".message-Information")
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SetupWorker>();
            context.Clients.All.otherMessage(msg, selector);
        }

        public void DoWork(InstallModel model)
        {
            var traceListener = new SignalRTraceListener();
            Trace.Listeners.Add(traceListener);

            try
            {
                SetupDb(ConnectionHelper.SqlConnectionString, model.SetupSampleData);
                SendMessageLine("Database successfully created.");
                UpdateIndex(model);
            }
            catch (Exception ex)
            {
                SendMessageLine(ex.Message);
                Clients.All.failure(ex.Message);
                return;
            }
            finally
            {
                Trace.Listeners.Remove(traceListener);
            }

            Clients.All.success("Database created.");

        }

        private void SetupDb(string connectionString, bool installSamples)
        {
            var dataFolder = @"App_Data\Virto\SampleData\Database";
            dataFolder = Path.Combine(System.Web.HttpContext.Current.Request.PhysicalApplicationPath ?? "/", dataFolder);
            SendMessageLine("Setting sample data path: {0}", dataFolder);
            // Configure database   
            SendMessageLine("Creating database and system tables.");
            new PublishAppConfigDatabase().Publish(connectionString, null, installSamples); // publish AppConfig first as it contains system tables
            SendMessageLine("Creating 'Store' module tables.");
            new PublishStoreDatabase().Publish(connectionString, null, installSamples);
            SendMessageLine("Creating 'Catalog' module tables.");
            new PublishCatalogDatabase().Publish(connectionString, dataFolder, installSamples);
            SendMessageLine("Creating 'Import' module tables.");
            new PublishImportDatabase().Publish(connectionString, dataFolder, installSamples);
            SendMessageLine("Creating 'Customer' module tables.");
            new PublishCustomerDatabase().Publish(connectionString, null, installSamples);
            SendMessageLine("Creating 'Inventory' module tables.");
            new PublishInventoryDatabase().Publish(connectionString, null, installSamples);
            SendMessageLine("Creating 'Log' module tables.");
            new PublishLogDatabase().Publish(connectionString, null, installSamples);
            SendMessageLine("Creating 'Marketing' module tables.");
            new PublishMarketingDatabase().Publish(connectionString, null, installSamples);
            SendMessageLine("Creating 'Order' module tables.");
            new PublishOrderDatabase().Publish(connectionString, null, installSamples);
            SendMessageLine("Creating 'Review' module tables.");
            new PublishReviewDatabase().Publish(connectionString, null, installSamples);
            SendMessageLine("Creating 'Search' module tables.");
            new PublishSearchDatabase().Publish(connectionString, null, installSamples);
            SendMessageLine("Creating 'Security' module tables.");
            new PublishSecurityDatabase().Publish(connectionString, dataFolder, installSamples);
        }

        private void UpdateIndex(InstallModel model)
        {
            var searchConnection = new SearchConnection(ConnectionHelper.GetConnectionString("SearchConnectionString"));
            if (searchConnection.Provider.Equals(
                    SearchProviders.Lucene.ToString(), StringComparison.OrdinalIgnoreCase) && searchConnection.DataSource.StartsWith("~/"))
            {
                var dataSource = searchConnection.DataSource.Replace(
                    "~/", HttpRuntime.AppDomainAppPath + "\\");

                searchConnection = new SearchConnection(dataSource, searchConnection.Scope, searchConnection.Provider);
            }

            SendMessageLine("Updating search index...");
            new UpdateSearchIndex().Index(searchConnection, model.ConnectionStringBuilder.ConnectionString, null, true);
        }
    }
}