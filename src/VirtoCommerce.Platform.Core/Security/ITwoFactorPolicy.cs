using System.Threading.Tasks;

namespace VirtoCommerce.Platform.Core.Security;

public interface ITwoFactorPolicy
{
    Task<bool> IsTwoFactorRequiredAsync(TwoFactorPolicyContext context);
}
