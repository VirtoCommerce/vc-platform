namespace VirtoCommerce.ApiClient.DataContracts.Security
{
    public enum SignInStatus
    {
        Success = 0,
        LockedOut = 1,
        RequiresVerification = 2,
        Failure = 3
    }
}