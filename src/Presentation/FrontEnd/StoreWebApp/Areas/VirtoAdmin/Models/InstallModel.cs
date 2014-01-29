using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Web;
using VirtoCommerce.Foundation.Data.Infrastructure;
using InstallResource = VirtoCommerce.Web.Areas.VirtoAdmin.Resources.Resource;

namespace VirtoCommerce.Web.Areas.VirtoAdmin.Models
{
    public class InstallModel
    {
        public enum SubmitAction
        {
            Save,
            CreateDb,
            Restart
        }

        [Required(ErrorMessageResourceName = "DataSourceRequiredError", ErrorMessageResourceType = typeof(InstallResource))]
        [Display(Name="DataSource", ResourceType = typeof(InstallResource))]
        public string DataSource { get; set; }

        [Required(ErrorMessageResourceName = "InitialCatalogRequiredError", ErrorMessageResourceType = typeof(InstallResource))]
        [Display(Name="InitialCatalog", ResourceType = typeof(InstallResource))]
        public string InitialCatalog { get; set; }

        [Required(ErrorMessageResourceName = "UserNameRequiredError", ErrorMessageResourceType = typeof(InstallResource))]
        [Display(Name="DbUserName", ResourceType = typeof(InstallResource))]
        public string DbUserName { get; set; }

        [Required(ErrorMessageResourceName = "UserPasswordRequiredError", ErrorMessageResourceType = typeof(InstallResource))]
        [Display(Name="DbUserPassword", ResourceType = typeof(InstallResource))]
        public string DbUserPassword { get; set; }

        [Display(Name = "DbAdminUser", ResourceType = typeof(InstallResource))]
        public string DbAdminUser { get; set; }

        [Display(Name="DbAdminPassword", ResourceType = typeof(InstallResource))]
        public string DbAdminPassword { get; set; }

        [Display(Name="SetupSampleData", ResourceType = typeof(InstallResource))]
        public bool SetupSampleData { get; set; }

        public bool DbAdminRequired { get; set; }

        public string StatusMessage { get; set; }

        public string ErrorMessage { get; set; }

        public bool HasErrorMessage
        {
            get
            {
                return !string.IsNullOrEmpty(ErrorMessage);
            }
        }

        public bool HasStatusMessage 
        {
            get
            {
                return !string.IsNullOrEmpty(StatusMessage);
            }
        }

        public SubmitAction Action { get; set; }

        public void ClearMessages()
        {
            ErrorMessage = "";
            StatusMessage = "";
            HttpContext.Current.Session["log"] = null;
        }

        public SqlConnectionStringBuilder ConnectionStringBuilder
        {
            get
            {
                return new SqlConnectionStringBuilder(ConnectionHelper.SqlConnectionString)
                {
                    DataSource = this.DataSource,
                    InitialCatalog = this.InitialCatalog,
                    UserID = this.DbUserName,
                    Password = this.DbUserPassword,
                    ConnectTimeout = 420,
                    MultipleActiveResultSets = true
                };
            }
        }

        public bool IsLogExists
        {
            get
            {
                var log = HttpContext.Current.Session["log"] as string;
                return !string.IsNullOrWhiteSpace(log);
            }
        }
    }
}