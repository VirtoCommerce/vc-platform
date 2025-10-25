using Hangfire.MySql;
using Hangfire.PostgreSql;
using Hangfire.Pro.Redis;
using Hangfire.SqlServer;

namespace VirtoCommerce.Platform.Hangfire
{
    public class HangfireOptions
    {
        public HangfireJobStorageType JobStorageType { get; set; } = HangfireJobStorageType.Memory;
        public int AutomaticRetryCount { get; set; } = 1;
        public int? WorkerCount { get; set; }
        public bool UseHangfireServer { get; set; } = true;
        public SqlServerStorageOptions SqlServerStorageOptions { get; set; } = new();
        public MySqlStorageOptions MySqlStorageOptions { get; set; } = new();
        public PostgreSqlStorageOptions PostgreSqlStorageOptions { get; set; } = new();
        public RedisStorageOptions RedisStorageOptions { get; set; } = new();
    }

    public enum HangfireJobStorageType
    {
        Memory,
        SqlServer,
        Database,
        Redis,
    }
}
