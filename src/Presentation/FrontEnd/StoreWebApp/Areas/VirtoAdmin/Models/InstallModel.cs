namespace VirtoCommerce.Web.Areas.VirtoAdmin.Models
{
    public class InstallModel
    {
        public string DataSource { get; set; }
        public string InitialCatalog { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string SaUser { get; set; }
        public string SaPassword { get; set; }
        public bool SetupSampleData { get; set; }

        public string StatusMessage { get; set; }
    }
}