using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using VirtoCommerce.Foundation.Data.Infrastructure;
using InstallResource = VirtoCommerce.Web.Areas.VirtoAdmin.Resources.Resource;

namespace VirtoCommerce.Web.Areas.VirtoAdmin.Models
{
    public class InstallModel
    {
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
    }
}