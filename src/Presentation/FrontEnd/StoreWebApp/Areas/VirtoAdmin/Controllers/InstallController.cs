using System;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Foundation.Data.Infrastructure;
using VirtoCommerce.PowerShell.DatabaseSetup.Cmdlet;
using VirtoCommerce.Web.Areas.VirtoAdmin.Models;

namespace VirtoCommerce.Web.Areas.VirtoAdmin.Controllers
{
    public class InstallController : Web.Controllers.ControllerBase
    {
        public ActionResult Index()
        {
            var model = new InstallModel();
            var csBuilder = new SqlConnectionStringBuilder(ConnectionHelper.SqlConnectionString);
            model.DataSource = csBuilder.DataSource;
            model.InitialCatalog = csBuilder.InitialCatalog;
            model.UserName = csBuilder.UserID;
            model.UserPassword = csBuilder.Password;
            model.SaUser = "sa";
            return View(model);
        }

        [HttpPost]
        public ActionResult Start(InstallModel model)
        {
            try
            {
                SetupDb(model);
            }
            catch (Exception err)
            {
                model.StatusMessage = err.Message;
            }
            
            return View("Index", model);
        }

        public ActionResult Restart()
        {
            HttpRuntime.UnloadAppDomain();
            return this.Redirect("~/");
        }

        private void SetupDb(InstallModel model)
        {
            // Initialise connection string builder with SA user. It is needed for 
            // database setup
            var csBuilder = new SqlConnectionStringBuilder(ConnectionHelper.SqlConnectionString)
            {
                DataSource = model.DataSource,
                InitialCatalog = model.InitialCatalog,
                UserID = model.SaUser,
                Password = model.SaPassword
            };

            var connectionString = csBuilder.ConnectionString;
            var installSamples = true;//model.SetupSampleData;

            var dataFolder = @"App_Data\Virto\SampleData\Database";
            dataFolder = Path.Combine(System.Web.HttpContext.Current.Request.PhysicalApplicationPath ?? "/", dataFolder);

            // Configure database            
            new PublishAppConfigDatabase().Publish(connectionString, null, installSamples); // publish AppConfig first as it contains system tables
            new PublishStoreDatabase().Publish(connectionString, null, installSamples);
            new PublishCatalogDatabase().Publish(connectionString, dataFolder, installSamples);
            new PublishImportDatabase().Publish(connectionString, dataFolder, installSamples);
            new PublishCustomerDatabase().Publish(connectionString, null, installSamples);
            new PublishInventoryDatabase().Publish(connectionString, null, installSamples);
            new PublishLogDatabase().Publish(connectionString, null, installSamples);
            new PublishMarketingDatabase().Publish(connectionString, null, installSamples);
            new PublishOrderDatabase().Publish(connectionString, null, installSamples);
            new PublishReviewDatabase().Publish(connectionString, null, installSamples);
            new PublishSearchDatabase().Publish(connectionString, null, installSamples);
            new PublishSecurityDatabase().Publish(connectionString, dataFolder, installSamples);
            
            AddUserToDatabase(connectionString, model.UserName, model.UserPassword);
            
            // save connection string with user credential which is dedicated to web application
            csBuilder.UserID = model.UserName;
            csBuilder.Password = model.UserPassword;
            ConnectionHelper.SqlConnectionString = csBuilder.ConnectionString;
        }

        private void AddUserToDatabase(string connectionString, string userId, string password, string dbRole = "db_owner")
        {
            using (var dbConn = new SqlConnection(connectionString) )
            {
                dbConn.Open();
                var databaseName = dbConn.Database;
                dbConn.ChangeDatabase("master");
                ExecuteSQL(dbConn, "CREATE LOGIN [{0}] WITH PASSWORD = '{1}'", userId, password);

                dbConn.ChangeDatabase(databaseName);
                ExecuteSQL(dbConn, "CREATE USER [{0}] FOR LOGIN {0}", userId);
                ExecuteSQL(dbConn, "EXEC sp_addrolemember '{1}', '{0}'", userId, dbRole);
            }
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
