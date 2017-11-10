namespace VirtoCommerce.Platform.Web.Hangfire
{
    public class HangfireOptions
    {
        public bool StartServer { get; set; }
        public string JobStorageType { get; set; }
        public string DatabaseConnectionString { get; set; }
        public int? WorkerCount { get; set; }
    }
}
