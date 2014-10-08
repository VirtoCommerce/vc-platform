using System;
using System.Diagnostics;
using System.IO;
using System.Web;
using Microsoft.AspNet.SignalR;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet;
using VirtoCommerce.PowerShell.SearchSetup.Cmdlet;
using VirtoCommerce.Web.Areas.VirtoAdmin.Models;

namespace VirtoCommerce.Web.Areas.VirtoAdmin.Helpers
{
    public class SetupWorker : Hub
    {
        public static void TraceMessage(string msg, params object[] format)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SetupWorker>();
            context.Clients.All.traceMessage(string.Format(msg,format));
        }

        /// <summary>
        /// Used to send full report
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="format"></param>
        public static void TraceMessageLine(string msg, params object[] format)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SetupWorker>();
            context.Clients.All.traceMessage(string.Format(msg, format), true);
        }

        /// <summary>
        /// Shows friendly message to user
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="format"></param>
        public static void SendFriendlyMessage(string msg, params object[] format)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<SetupWorker>();
            context.Clients.All.friendlyMessage(string.Format(msg, format));
        }


        public void DoWork(InstallModel model)
        {
            var traceListener = new SignalRTraceListener();
            Trace.Listeners.Add(traceListener);

            try
            {
                SetupDb(ConnectionHelper.SqlConnectionString, model.SetupSampleData);
                SendFriendlyMessage("Database successfully created.");
                UpdateIndex(model);
            }
            catch (Exception ex)
            {
                TraceMessageLine(ex.Message);
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
            dataFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath ?? "/", dataFolder);
            SendFriendlyMessage("Setting sample data path: {0}", dataFolder);
            // Configure database   
            SendFriendlyMessage("Creating database and system tables.");
            new PublishAppConfigDatabase().Publish(connectionString, null, installSamples, installSamples); // publish AppConfig first as it contains system tables
            SendFriendlyMessage("Creating 'Store' module tables.");
            new PublishStoreDatabase().Publish(connectionString, null, installSamples, installSamples);
            SendFriendlyMessage("Creating 'Catalog' module tables.");
            new PublishCatalogDatabase().Publish(connectionString, dataFolder, installSamples, installSamples);
            SendFriendlyMessage("Creating 'Import' module tables.");
            new PublishImportDatabase().Publish(connectionString, dataFolder, installSamples, installSamples);
            SendFriendlyMessage("Creating 'Customer' module tables.");
            new PublishCustomerDatabase().Publish(connectionString, null, installSamples, installSamples);
            SendFriendlyMessage("Creating 'Inventory' module tables.");
            new PublishInventoryDatabase().Publish(connectionString, null, installSamples, installSamples);
            SendFriendlyMessage("Creating 'Log' module tables.");
            new PublishLogDatabase().Publish(connectionString, null, installSamples, installSamples);
            SendFriendlyMessage("Creating 'Marketing' module tables.");
            new PublishMarketingDatabase().Publish(connectionString, null, installSamples, installSamples);
            SendFriendlyMessage("Creating 'Order' module tables.");
            new PublishOrderDatabase().Publish(connectionString, null, installSamples, installSamples);
            SendFriendlyMessage("Creating 'Review' module tables.");
            new PublishReviewDatabase().Publish(connectionString, null, installSamples, installSamples);
            SendFriendlyMessage("Creating 'Search' module tables.");
            new PublishSearchDatabase().Publish(connectionString, null, installSamples, installSamples);
            SendFriendlyMessage("Creating 'Security' module tables.");
            new PublishSecurityDatabase().Publish(connectionString, dataFolder, installSamples, installSamples);
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

            SendFriendlyMessage("Updating search index...");
            new UpdateSearchIndex().Index(searchConnection, model.ConnectionStringBuilder.ConnectionString, null, true);
        }
    }
}