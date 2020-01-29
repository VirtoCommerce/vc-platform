namespace VirtoCommerce.Platform.Web.Hangfire
{
    public class HangfireOptions
    {
        public HangfireJobStorageType JobStorageType { get; set; } = HangfireJobStorageType.Memory;
        public int? WorkerCount { get; set; }
    }

    public enum HangfireJobStorageType
    {
        Memory,
        SqlServer,

    }
}
