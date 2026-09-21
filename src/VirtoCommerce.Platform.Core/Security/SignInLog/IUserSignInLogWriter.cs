namespace VirtoCommerce.Platform.Core.Security.SignInLog;

/// <summary>
/// Non-blocking entry point for audit rows. Implementations must never throw and never
/// block the caller — a failing audit log must not fail a sign-in.
/// </summary>
public interface IUserSignInLogWriter
{
    void Write(UserSignInLog record);
}
