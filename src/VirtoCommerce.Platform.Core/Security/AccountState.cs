namespace VirtoCommerce.Platform.Core.Security
{
    /// <summary>
    /// Obsolete. Left due to compatibility issues. Will be removed. Instead of, use: ApplicationUser.EmailConfirmed, ApplicationUser.LockoutEnd.
    /// </summary>
    public enum AccountState
    {
        PendingApproval,
        Approved,
        Rejected
    }
}
