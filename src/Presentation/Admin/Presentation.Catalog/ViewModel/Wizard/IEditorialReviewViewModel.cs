using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Wizard
{
    public interface IEditorialReviewViewModel : IWizardStep
    {
        EditorialReview InnerItem { get; }
    }
}
