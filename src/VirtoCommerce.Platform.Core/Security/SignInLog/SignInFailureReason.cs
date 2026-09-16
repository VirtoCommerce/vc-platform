namespace VirtoCommerce.Platform.Core.Security.SignInLog;

/// <summary>
/// Why a sign-in attempt failed. Only values reachable from an actual call site are defined —
/// do not add a reason that cannot occur.
/// </summary>
public static class SignInFailureReason
{
    public const string UserNotFound = "UserNotFound";
    public const string InvalidPassword = "InvalidPassword";
    public const string LockedOut = "LockedOut";
    public const string NotAllowed = "NotAllowed";
    public const string RequiresTwoFactor = "RequiresTwoFactor";
    public const string PasswordLoginDisabled = "PasswordLoginDisabled";
    public const string DuplicateEmail = "DuplicateEmail";
    public const string Forbidden = "Forbidden";
}
