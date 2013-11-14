using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
    public class AssociationGroupEditViewModel : ViewModelBase, IAssociationGroupEditViewModel
    {
        private readonly List<string> _selectedNames;

        public AssociationGroupEditViewModel(AssociationGroup item, IAppConfigRepository appConfigRepository, IEnumerable<object> selectedNames)
        {
            InnerItem = item;

            _selectedNames = selectedNames.Select(x => (string)x).Where(x => x != item.Name).ToList();

            NameSetting = appConfigRepository.Settings.Where(s => s.Name == "AssociationGroupTypes")
                                  .Expand(s => s.SettingValues)
                                  .FirstOrDefault();
            if (NameSetting != null)
            {
                var view = CollectionViewSource.GetDefaultView(NameSetting.SettingValues);
                view.Filter = FilterItems;
                view.Refresh();
            }
        }

        public Setting NameSetting { get; set; }
        
        public AssociationGroup InnerItem { get; private set; }

        public bool Validate()
        {
            return InnerItem.Validate();
        }

        private bool FilterItems(object item)
        {
            bool result;

            var settingVal = (SettingValue)item;
            result = !_selectedNames.Contains(settingVal.ShortTextValue);
            return result;
        }
    }
}
