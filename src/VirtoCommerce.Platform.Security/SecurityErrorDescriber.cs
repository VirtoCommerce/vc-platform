using System;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Security.Model;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace VirtoCommerce.Platform.Security
{
    [Obsolete("Use VirtoCommerce.Platform.Security.OpenIddict.SecurityErrorDescriber", DiagnosticId = "VC0008", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions/")]
    public static class SecurityErrorDescriber
    {
        public static TokenLoginResponse LoginFailed() => new()
        {
            Error = Errors.InvalidGrant,
            Code = nameof(LoginFailed).ToSnakeCase(),
            ErrorDescription = "Login attempt failed. Please check your credentials."
        };

        public static TokenLoginResponse UserIsLockedOut() => new()
        {
            Error = Errors.InvalidGrant,
            Code = nameof(UserIsLockedOut).ToSnakeCase(),
            ErrorDescription = "Your account has been locked. Please contact support for assistance."
        };

        public static TokenLoginResponse UserIsTemporaryLockedOut() => new()
        {
            Error = Errors.InvalidGrant,
            Code = nameof(UserIsLockedOut).ToSnakeCase(),
            ErrorDescription = "Your account has been temporarily locked. Please try again after some time."
        };

        public static TokenLoginResponse PasswordExpired() => new()
        {
            Error = Errors.InvalidGrant,
            Code = nameof(PasswordExpired).ToSnakeCase(),
            ErrorDescription = "Your password has been expired and must be changed.",
        };

        public static TokenLoginResponse PasswordLoginDisabled() => new()
        {
            Error = Errors.InvalidGrant,
            Code = nameof(PasswordLoginDisabled).ToSnakeCase(),
            ErrorDescription = "The username/password login is disabled."
        };

        public static TokenLoginResponse TokenInvalid() => new()
        {
            Error = Errors.InvalidGrant,
            Code = nameof(TokenInvalid).ToSnakeCase(),
            ErrorDescription = "The token is no longer valid."
        };

        public static TokenLoginResponse SignInNotAllowed() => new()
        {
            Error = Errors.InvalidGrant,
            Code = nameof(SignInNotAllowed).ToSnakeCase(),
            ErrorDescription = "The user is no longer allowed to sign in."
        };

        public static TokenLoginResponse InvalidClient() => new()
        {
            Error = Errors.InvalidClient,
            Code = nameof(InvalidClient).ToSnakeCase(),
            ErrorDescription = "The client application was not found in the database."
        };

        public static TokenLoginResponse UnsupportedGrantType() => new()
        {
            Error = Errors.UnsupportedGrantType,
            Code = nameof(UnsupportedGrantType).ToSnakeCase(),
            ErrorDescription = "The specified grant type is not supported."
        };
    }
}
