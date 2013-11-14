using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Security.ViewModel.Interfaces
{
    public interface IPasswordChangeViewModel: IViewModel
    {
        bool IsValid { get; }
        // bool Validate();

        string OldPassword { get; set; }
        string Password { get; set; }
    }
}
