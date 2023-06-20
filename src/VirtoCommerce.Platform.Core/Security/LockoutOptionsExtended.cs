namespace VirtoCommerce.Platform.Core.Security
{
    public class LockoutOptionsExtended
    {
        public bool AutoAccountsLockoutJobEnabled { get; set; } = false;
        public int LockoutMaximumDaysFromLastLogin { get; set; } = 365;
        public string CronAutoAccountsLockoutJob { get; set; } = "0 0 * * *";
    }
}
