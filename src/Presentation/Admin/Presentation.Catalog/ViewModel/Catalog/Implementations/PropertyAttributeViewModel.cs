using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
    public class PropertyAttributeViewModel : ViewModelBase, IPropertyAttributeViewModel
    {

        public PropertyAttributeViewModel(PropertyAttribute item)
        {
            InnerItem = item;
        }

        #region IPropertyAttributeViewModel

        public PropertyAttribute InnerItem { get; private set; }

        public bool Validate()
        {
            return InnerItem.Validate();
        }

        #endregion
    }
}
