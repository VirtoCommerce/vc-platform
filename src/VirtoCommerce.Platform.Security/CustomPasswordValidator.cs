using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Repositories;

namespace VirtoCommerce.Platform.Security
{
    /// <summary>
    /// Overriding https://github.com/dotnet/aspnetcore/blob/release/3.1/src/Identity/Extensions.Core/src/PasswordValidator.cs
    /// </summary>
    public class CustomPasswordValidator : PasswordValidator<ApplicationUser>
    {
        public const string RecentPasswordUsed = "RecentPasswordUsed";

        protected readonly Func<ISecurityRepository> _repositoryFactory;
        protected readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        protected readonly PasswordOptionsExtended _passwordOptions;

        public CustomPasswordValidator(IdentityErrorDescriber errors, Func<ISecurityRepository> repositoryFactory, IPasswordHasher<ApplicationUser> passwordHasher, IOptions<PasswordOptionsExtended> passwordOptions)
            : base(errors)

        {
            _repositoryFactory = repositoryFactory;
            _passwordHasher = passwordHasher;
            _passwordOptions = passwordOptions.Value;
        }

        public override async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user, string password)
        {
            var result = await base.ValidateAsync(manager, user, password);

            if (result.Succeeded)
            {
                using var repository = _repositoryFactory();
                var userPasswords = await repository.GetUserPasswordsHistoryAsync(user?.Id, _passwordOptions.PasswordHistory.GetValueOrDefault());

                if (userPasswords.Any(x => _passwordHasher.VerifyHashedPassword(user, x.PasswordHash, password) != PasswordVerificationResult.Failed))
                {
                    result = IdentityResult.Failed(new IdentityError
                    {
                        Code = RecentPasswordUsed
                    });
                }
            }
            return result;
        }
    }
}
