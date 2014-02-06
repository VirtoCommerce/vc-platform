using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using VirtoCommerce.Foundation.AppConfig;
using VirtoCommerce.Foundation.Data.Infrastructure;
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
            var csBuilder = new SqlConnectionStringBuilder(ConnectionHelper.SqlConnectionString);
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
                (Request.Url.Port == 80) ? "" : ":" + Request.Url.Port,
                VirtualPathUtility.ToAbsolute("~/"));

            successModel.Website = String.Format("{0}", url);
            return View("Success", successModel);
        }

        public ActionResult Complete()
        {
            AppConfigConfiguration.Instance.Setup.IsCompleted = true;
            HttpRuntime.UnloadAppDomain();
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
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
                var connectionString = PrepareDb(model);
                SetupWorker.SendFriendlyMessage("Saving database connection string to web.config.");
                //After saving connection string signalR needs some time to recycle iis
                ConnectionHelper.SqlConnectionString = connectionString;

            }
            catch (Exception ex)
            {
                SetupWorker.TraceMessageLine(ex.Message);
                return Json(new { Success = false, ex.Message });
            }
            finally
            {
                Trace.Listeners.Remove(traceListener);
            }

            return Json(new { Success = true });

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


        private string PrepareDb(InstallModel model)
        {
            var csBuilder = model.ConnectionStringBuilder;
            SetupWorker.SendFriendlyMessage("Checking connection availability. Connection string: {0}", csBuilder.ConnectionString);

            var success = CheckDb(csBuilder.ConnectionString);

            if (success)
            {
                return csBuilder.ConnectionString;
            }

            if (!string.IsNullOrEmpty(model.DbAdminUser))
            {
                SetupWorker.SendFriendlyMessage("Trying to connect with administrator user {0}.", model.DbAdminUser);
                // let's try with admin user
                csBuilder.UserID = model.DbAdminUser;
                csBuilder.Password = model.DbAdminPassword;
                success = CheckDb(csBuilder.ConnectionString);

            }

            if (!success)
            {
                SetupWorker.SendFriendlyMessage("Trying to connect with integrated user.");
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


            if (csBuilder.InitialCatalog.ToLower() == "master")
            {
                throw new Exception("'Master' is reserved for system database, please provide other database name.");
            }

            return csBuilder.ConnectionString;
        }

        private void AddUserToDatabase(string connectionString, string userId, string password, string dbRole = "db_owner")
        {
            using (var dbConn = new SqlConnection(connectionString))
            {
                SetupWorker.SendFriendlyMessage("Creating user and adding it to database.");
                dbConn.Open();
                var databaseName = dbConn.Database;
                dbConn.ChangeDatabase("master");
                try
                {
                    ExecuteSQL(dbConn, "CREATE LOGIN [{0}] WITH PASSWORD = '{1}'", userId, password);
                }
                catch (Exception err)
                {
                    SetupWorker.TraceMessageLine(err.Message);
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
                SetupWorker.TraceMessageLine(err.Message);
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
