using Microsoft.Practices.Prism.Commands;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
    /// <summary>
    /// Represents the essence of the open track in the list of open items and make a quick navigation.
    /// </summary>
    public interface IOpenTracking
    {
        DelegateCommand OpenItemCommand { get; }

        /// <summary>
        /// is item selected and related item opened
        /// </summary>
        bool IsActive { get; set; }
    }
}
