using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Security.Model;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace VirtoCommerce.Platform.Security
{
    public static class SecurityErrorDescriber
    {
        public static TokenLoginResponse LoginFailed() => new()
        {
            Error = Errors.InvalidGrant,
            Code = nameof(LoginFailed).PascalToKebabCase(),
            ErrorDescription = "Login attempt failed. Please check your credentials."
        };

        public static TokenLoginResponse UserIsLockedOut() => new()
        {
            Error = Errors.InvalidGrant,
            Code = nameof(UserIsLockedOut).PascalToKebabCase(),
            ErrorDescription = "Your account has been locked. Please contact support for assistance."
        };

        public static TokenLoginResponse UserIsTemporaryLockedOut() => new()
        {
            Error = Errors.InvalidGrant,
            Code = nameof(UserIsLockedOut).PascalToKebabCase(),
            ErrorDescription = "Your account has been temporarily locked. Please try again after some time."
        };

        public static TokenLoginResponse PasswordExpired() => new()
        {
            Error = Errors.InvalidGrant,
            Code = nameof(PasswordExpired).PascalToKebabCase(),
            ErrorDescription = "Your password has been expired and must be changed.",
        };

        public static TokenLoginResponse PasswordLoginDisabled() => new()
        {
            Error = Errors.InvalidGrant,
            Code = nameof(PasswordLoginDisabled).PascalToKebabCase(),
            ErrorDescription = "The username/password login is disabled."
        };

        public static TokenLoginResponse TokenInvalid() => new()
        {
            Error = Errors.InvalidGrant,
            Code = nameof(TokenInvalid).PascalToKebabCase(),
            ErrorDescription = "The token is no longer valid."
        };

        public static TokenLoginResponse SignInNotAllowed() => new()
        {
            Error = Errors.InvalidGrant,
            Code = nameof(SignInNotAllowed).PascalToKebabCase(),
            ErrorDescription = "The user is no longer allowed to sign in."
        };

        public static TokenLoginResponse InvalidClient() => new()
        {
            Error = Errors.InvalidClient,
            Code = nameof(InvalidClient).PascalToKebabCase(),
            ErrorDescription = "The client application was not found in the database."
        };

        public static TokenLoginResponse UnsupportedGrantType() => new()
        {
            Error = Errors.UnsupportedGrantType,
            Code = nameof(UnsupportedGrantType).PascalToKebabCase(),
            ErrorDescription = "The specified grant type is not supported."
        };
    }
}
