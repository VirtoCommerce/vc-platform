using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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

        [Required(ErrorMessage = "Database server name is required.")]
        [DisplayName("Database server")]
        public string DataSource { get; set; }

        [Required(ErrorMessage = "Database name is required.")]
        [DisplayName("Database name")]
        public string InitialCatalog { get; set; }

        [Required(ErrorMessage = "Database user name is required.")]
        [DisplayName("User name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Database user password is required.")]
        [DisplayName("Password")]
        public string UserPassword { get; set; }

        [Required(ErrorMessage = "Database admin user name is required.")]
        [DisplayName("Database admin user")]
        public string SaUser { get; set; }

        [Required(ErrorMessage = "Database admin user password is required.")]
        [DisplayName("Database admin password")]
        public string SaPassword { get; set; }

        [DisplayName("Generate sample data")]
        public bool SetupSampleData { get; set; }

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

        public string TraceFileName { get; set; }

        public SubmitAction Action { get; set; }

        public void ClearMessages()
        {
            ErrorMessage = "";
            StatusMessage = "";
        }
    }
}