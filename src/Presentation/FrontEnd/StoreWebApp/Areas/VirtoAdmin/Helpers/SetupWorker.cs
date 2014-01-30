using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using VirtoCommerce.Foundation.Data.AppConfig;
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
                Trace.TraceInformation("Database successfully created.");
                UpdateIndex(model);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
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

            // Configure database   
            Trace.TraceInformation("Creating database and system tables.");
            new PublishAppConfigDatabase().Publish(connectionString, null, installSamples); // publish AppConfig first as it contains system tables
            Trace.TraceInformation("Creating 'Store' module tables.");
            new PublishStoreDatabase().Publish(connectionString, null, installSamples);
            Trace.TraceInformation("Creating 'Catalog' module tables.");
            new PublishCatalogDatabase().Publish(connectionString, dataFolder, installSamples);
            Trace.TraceInformation("Creating 'Import' module tables.");
            new PublishImportDatabase().Publish(connectionString, dataFolder, installSamples);
            Trace.TraceInformation("Creating 'Customer' module tables.");
            new PublishCustomerDatabase().Publish(connectionString, null, installSamples);
            Trace.TraceInformation("Creating 'Inventory' module tables.");
            new PublishInventoryDatabase().Publish(connectionString, null, installSamples);
            Trace.TraceInformation("Creating 'Log' module tables.");
            new PublishLogDatabase().Publish(connectionString, null, installSamples);
            Trace.TraceInformation("Creating 'Marketing' module tables.");
            new PublishMarketingDatabase().Publish(connectionString, null, installSamples);
            Trace.TraceInformation("Creating 'Order' module tables.");
            new PublishOrderDatabase().Publish(connectionString, null, installSamples);
            Trace.TraceInformation("Creating 'Review' module tables.");
            new PublishReviewDatabase().Publish(connectionString, null, installSamples);
            Trace.TraceInformation("Creating 'Search' module tables.");
            new PublishSearchDatabase().Publish(connectionString, null, installSamples);
            Trace.TraceInformation("Creating 'Security' module tables.");
            new PublishSecurityDatabase().Publish(connectionString, dataFolder, installSamples);
        }

        private void UpdateIndex(InstallModel model)
        {
            var searchConnection = new SearchConnection(ConnectionHelper.GetConnectionString("SearchConnectionString"));
            if (searchConnection.Provider.Equals(
                    "lucene", StringComparison.OrdinalIgnoreCase) && searchConnection.DataSource.StartsWith("~/"))
            {
                var dataSource = searchConnection.DataSource.Replace(
                    "~/", HttpRuntime.AppDomainAppPath + "\\");

                searchConnection = new SearchConnection(dataSource, searchConnection.Scope, searchConnection.Provider);
            }

            new UpdateSearchIndex().Index(searchConnection, model.ConnectionStringBuilder.ConnectionString, null, true);
        }
    }
}