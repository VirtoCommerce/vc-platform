using System;
using System.Data.Entity;
using System.Data.Entity.Migrations.Infrastructure;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using VirtoCommerce.Foundation.AppConfig;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.Foundation.Search;
using VirtoCommerce.OrderWorkflow;
using VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet;
using VirtoCommerce.PowerShell.SearchSetup.Cmdlet;
using VirtoCommerce.Web.Areas.VirtoAdmin.Helpers;
using VirtoCommerce.Web.Areas.VirtoAdmin.Models;
using VirtoCommerce.Web.Areas.VirtoAdmin.Resources;
using VirtoCommerce.Web.Client.Extensions.Filters;

namespace VirtoCommerce.Web.Areas.VirtoAdmin.Controllers
{
    public class InstallController : Controller
    {
        public ActionResult Index()
        {
            if (AppConfigConfiguration.Instance.Setup.IsCompleted)
            {
                return Success();
            }
            var model = new InstallModel();
            var csBuilder = new SqlConnectionStringBuilder(GetSetupConnectionString());
            model.DataSource = csBuilder.DataSource;
            model.InitialCatalog = csBuilder.InitialCatalog;
            model.DbUserName = csBuilder.UserID;
            model.DbUserPassword = csBuilder.Password;
            model.SetupSampleData = true;

            return View(model);
        }

        public ActionResult Success()
        {
            var successModel = new SuccessModel();

            var url = string.Format(
                "{0}://{1}{2}{3}",
                (Request.IsSecureConnection) ? "https" : "http",
                Request.Url.Host,
                (Request.Url.Port == 80) ? "" : ":" + Request.Url.Port.ToString(),
                VirtualPathUtility.ToAbsolute("~/"));

            successModel.Website = String.Format("{0}", url);
            return this.View("Success", successModel);
        }

        public ActionResult Complete()
        {
            AppConfigConfiguration.Instance.Setup.IsCompleted = true;
            HttpRuntime.UnloadAppDomain();
            return Success();
        }



        [HttpPost]
        [ValidateAjax]
        public JsonResult Index(InstallModel model)
        {
            CustomValidateModel(model);

            if (!ModelState.IsValid)
            {
                var errorModel =
                       from x in ModelState.Keys
                       where ModelState[x].Errors.Count > 0
                       select new
                       {
                           key = x,
                           errors = ModelState[x].Errors.
                                                  Select(y => y.ErrorMessage).
                                                  ToArray()
                       };

                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(errorModel);
            }

            var traceListener = new SignalRTraceListener();
            Trace.Listeners.Add(traceListener);

            try
            {
                CreateDb(model);
                UpdateIndex(model);

            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return Json(new { Success = false, ex.Message });
            }
            finally
            {
                Trace.Listeners.Remove(traceListener);
            }

            return Json(new { Success = true });

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

        private void CustomValidateModel(InstallModel model)
        {
            if (model.DbAdminRequired)
            {
                if (string.IsNullOrWhiteSpace(model.DbAdminPassword))
                {
                    ModelState.AddModelError("DbAdminPassword", Resource.DbAdminPasswordRequiredError);
                }
                if (string.IsNullOrWhiteSpace(model.DbAdminUser))
                {
                    ModelState.AddModelError("DbAdminUser", Resource.DbAdminUserRequiredError);
                }
            }
        }

        private string GetSetupConnectionString()
        {
            // var setupFile = MvcApplication.SetupFile;
            var connectionString = ConnectionHelper.SqlConnectionString;

            /*      if (System.IO.File.Exists(setupFile))
                  {
                      var setupContent = System.IO.File.ReadAllText(setupFile);
                      var mm=Regex.Match(setupContent, @"(?<=\s*SqlConnectionString:\s+)[^\s].*[^\r\n]");

                      if (mm.Success)
                      {
                          connectionString = mm.Value;
                      }

                      ConnectionHelper.SqlConnectionString = connectionString;
                  }*/
            return connectionString;
        }

        private void CreateDb(InstallModel model)
        {
            SetupDb(model);
            SetupWorker.DisplayInfoMessage("Database successfully created.");

        }

        private string PrepareDb(InstallModel model)
        {
            var csBuilder = model.ConnectionStringBuilder;
            SetupWorker.SendMessage("Checking connection availability. Connection string: {0}", csBuilder.ConnectionString);

            var success = CheckDb(csBuilder.ConnectionString);

            if (success)
            {
                return csBuilder.ConnectionString;
            }

            if (!string.IsNullOrEmpty(model.DbAdminUser))
            {
                Trace.TraceInformation("Trying to connect with administrator user {0}.", model.DbAdminUser);
                // let's try with admin user
                csBuilder.UserID = model.DbAdminUser;
                csBuilder.Password = model.DbAdminPassword;
                success = CheckDb(csBuilder.ConnectionString);

            }

            if (!success)
            {
                Trace.TraceInformation("Trying to connect with integrated user.");
                // let's try with integrated user
                csBuilder.IntegratedSecurity = true;
                success = CheckDb(csBuilder.ConnectionString);
            }
            if (success)
            {
                AddUserToDatabase(csBuilder.ConnectionString, model.DbUserName, model.DbUserPassword);
            }
            else
            {
                model.DbAdminUser = "sa";
                model.DbAdminRequired = true;
                CustomValidateModel(model);
                throw new Exception(Resource.DbServerAdminRequiredException);
            }

            return csBuilder.ConnectionString;
        }

        public FileResult DownloadLog()
        {
            byte[] data = Encoding.UTF8.GetBytes(Session["log"] as string ?? "");
            return new FileContentResult(data, "text")
            {
                FileDownloadName = string.Format("vc_log_{0}", DateTime.Now.ToString("yyyyMMddHHmmss"))
            };
        }

        private void SetupDb(InstallModel model)
        {
            var csBuilder = model.ConnectionStringBuilder;
            var connectionString = PrepareDb(model);
            var installSamples = model.SetupSampleData;
            var dataFolder = @"App_Data\Virto\SampleData\Database";

            if (csBuilder.InitialCatalog.ToLower() == "master")
            {
                throw new Exception("'Master' is reserved for system database, please provide other database name.");
            }

            dataFolder = Path.Combine(System.Web.HttpContext.Current.Request.PhysicalApplicationPath ?? "/", dataFolder);
            ConnectionHelper.SqlConnectionString = csBuilder.ConnectionString;

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

            Trace.TraceInformation("Saving database connection string to web.config.");

            Trace.TraceInformation("Database created.");
        }

        private void AddUserToDatabase(string connectionString, string userId, string password, string dbRole = "db_owner")
        {
            using (var dbConn = new SqlConnection(connectionString))
            {
                Trace.TraceInformation("Creating user and adding it to database.");
                dbConn.Open();
                var databaseName = dbConn.Database;
                dbConn.ChangeDatabase("master");
                try
                {
                    ExecuteSQL(dbConn, "CREATE LOGIN [{0}] WITH PASSWORD = '{1}'", userId, password);
                }
                catch (Exception err)
                {
                    Trace.TraceWarning(err.Message);
                }

                dbConn.ChangeDatabase(databaseName);
                ExecuteSQL(dbConn, "CREATE USER [{0}] FOR LOGIN {0}", userId);
                ExecuteSQL(dbConn, "EXEC sp_addrolemember '{1}', '{0}'", userId, dbRole);

                dbConn.Close();
            }
        }

        private bool CheckDb(string connectionString)
        {
            bool result = false;
            try
            {
                var db = new DbContext(connectionString);
                db.Database.CreateIfNotExists();

                result = true;
            }
            catch (Exception err)
            {
                Trace.TraceWarning(err.Message);
            }

            return result;
        }

        private void ExecuteSQL(SqlConnection dbConn, string sqlCommand, params object[] args)
        {
            using (var cmd = dbConn.CreateCommand())
            {
                cmd.CommandText = string.Format(sqlCommand, args);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
