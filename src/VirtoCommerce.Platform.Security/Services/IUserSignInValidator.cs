using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Security.Model;

namespace VirtoCommerce.Platform.Security.Services
{
    public class SignInValidatorContext
    {
        public ApplicationUser User { get; set; }

        public string StoreId { get; set; }

        public bool DetailedErrors { get; set; }

        public bool IsSucceeded { get; set; }

        public bool IsLockedOut { get; set; }

        public IDictionary<string, object> AdditionalParameters { get; set; } = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
    }

    public interface IUserSignInValidator
    {
        public int Priority { get; set; }

        Task<IList<TokenLoginResponse>> ValidateUserAsync(SignInValidatorContext context);
    }
}
