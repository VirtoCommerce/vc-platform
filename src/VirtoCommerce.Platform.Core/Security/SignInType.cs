namespace VirtoCommerce.Platform.Core.Security;

/// <summary>
/// Kind of authentication that produced a <see cref="UserSignInLog"/> row.
/// String constants rather than an enum so modules can add their own types.
/// </summary>
public static class SignInType
{
    public const string Password = "Password";
    public const string External = "External";
    public const string Impersonation = "Impersonation";
    public const string ImpersonationRevert = "ImpersonationRevert";
    public const string ClientCredentials = "ClientCredentials";
    public const string Logout = "Logout";
}
