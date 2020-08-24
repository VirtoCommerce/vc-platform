namespace VirtoCommerce.Platform.Hangfire.Suspend
{
    /// <summary>
    /// Should be implemented to suspend Hangfire jobs 
    /// </summary>
    public interface IHangfireStartSuspend
    {
        public bool Suspend { get; set; }
        public int DelayNextStateForSeconds { get; set; }
    }
}
