using VirtoCommerce.Platform.Core.Common;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace VirtoCommerce.Platform.Security.OpenIddict
{
    public static class SecurityErrorDescriber
    {
        public static TokenResponse LoginFailed() => new()
        {
            Error = Errors.InvalidGrant,
            Code = nameof(LoginFailed).ToSnakeCase(),
            ErrorDescription = "Login attempt failed. Please check your credentials."
        };

        public static TokenResponse UserIsLockedOut() => new()
        {
            Error = Errors.InvalidGrant,
            Code = nameof(UserIsLockedOut).ToSnakeCase(),
            ErrorDescription = "Your account has been locked. Please contact support for assistance."
        };

        public static TokenResponse UserIsTemporaryLockedOut() => new()
        {
            Error = Errors.InvalidGrant,
            Code = nameof(UserIsLockedOut).ToSnakeCase(),
            ErrorDescription = "Your account has been temporarily locked. Please try again after some time."
        };

        public static TokenResponse PasswordExpired() => new()
        {
            Error = Errors.InvalidGrant,
            Code = nameof(PasswordExpired).ToSnakeCase(),
            ErrorDescription = "Your password has been expired and must be changed.",
        };

        public static TokenResponse PasswordLoginDisabled() => new()
        {
            Error = Errors.InvalidGrant,
            Code = nameof(PasswordLoginDisabled).ToSnakeCase(),
            ErrorDescription = "The username/password login is disabled."
        };

        public static TokenResponse TokenInvalid() => new()
        {
            Error = Errors.InvalidGrant,
            Code = nameof(TokenInvalid).ToSnakeCase(),
            ErrorDescription = "The token is no longer valid."
        };

        public static TokenResponse SignInNotAllowed() => new()
        {
            Error = Errors.InvalidGrant,
            Code = nameof(SignInNotAllowed).ToSnakeCase(),
            ErrorDescription = "The user is no longer allowed to sign in."
        };

        public static TokenResponse InvalidClient() => new()
        {
            Error = Errors.InvalidClient,
            Code = nameof(InvalidClient).ToSnakeCase(),
            ErrorDescription = "The client application was not found in the database."
        };

        public static TokenResponse UnsupportedGrantType() => new()
        {
            Error = Errors.UnsupportedGrantType,
            Code = nameof(UnsupportedGrantType).ToSnakeCase(),
            ErrorDescription = "The specified grant type is not supported."
        };
    }
}
