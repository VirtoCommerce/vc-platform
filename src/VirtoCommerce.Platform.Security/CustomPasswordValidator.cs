using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VirtoCommerce.Platform.Core.Common;
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

        protected readonly IScopedServiceFactory<ISecurityRepository> _scopedRepositoryFactory;

        [Obsolete("Use _scopedRepositoryFactory instead.", DiagnosticId = "VC0014", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        protected readonly Func<ISecurityRepository> _repositoryFactory;
        protected readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        protected readonly PasswordOptionsExtended _passwordOptions;

        [Obsolete("Use the constructor that takes IScopedServiceFactory<T> instead.", DiagnosticId = "VC0014", UrlFormat = "https://docs.virtocommerce.org/products/products-virto3-versions")]
        public CustomPasswordValidator(IdentityErrorDescriber errors, Func<ISecurityRepository> repositoryFactory, IPasswordHasher<ApplicationUser> passwordHasher, IOptions<PasswordOptionsExtended> passwordOptions)
            : this(errors, new DelegateScopedServiceFactory<ISecurityRepository>(repositoryFactory), passwordHasher, passwordOptions)
        {
        }

        [ActivatorUtilitiesConstructor]
        public CustomPasswordValidator(IdentityErrorDescriber errors, IScopedServiceFactory<ISecurityRepository> repositoryFactory, IPasswordHasher<ApplicationUser> passwordHasher, IOptions<PasswordOptionsExtended> passwordOptions)
            : base(errors)

        {
            _scopedRepositoryFactory = repositoryFactory;
#pragma warning disable VC0014 // Kept for derived validators that still read the legacy field; it hands out the scoped service without owning the scope, exactly as the old Func did.
            _repositoryFactory = () => repositoryFactory.Create().Service;
#pragma warning restore VC0014
            _passwordHasher = passwordHasher;
            _passwordOptions = passwordOptions.Value;
        }

        public override async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user, string password)
        {
            var result = await base.ValidateAsync(manager, user, password);

            if (result.Succeeded)
            {
                using var scopedRepository = _scopedRepositoryFactory.Create();
                var repository = scopedRepository.Service;
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
