using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Order.Model.Settings;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks.Extensions;

namespace VirtoCommerce.ManagementClient.Order.ViewModel.Settings
{
    public class GeneralLanguageViewModel : ViewModelBase, IGeneralLanguageViewModel
    {
        #region Fields

        private readonly List<GeneralLanguage> _selectedLanguages;

        #endregion

        #region Constructor

        public GeneralLanguageViewModel(IAppConfigRepository appConfigRepository, GeneralLanguage item,
            IEnumerable<object> selectedLanguages)
        {
            _selectedLanguages = selectedLanguages.Select(x => new GeneralLanguage(x)).Where(x => x.LanguageCode != item.LanguageCode).ToList();
            _innerItem = item;

            LanguageSetting = appConfigRepository.Settings.Where(s => s.Name.Contains("Lang"))
                                    .Expand(s => s.SettingValues)
                                    .FirstOrDefault();

            if (LanguageSetting != null)
            {
                var view = CollectionViewSource.GetDefaultView(LanguageSetting.SettingValues);
                view.Filter = FilterItems;
                view.Refresh();
            }

            InnerItem.PropertyChanged += InnerItem_PropertyChanged;
        }

        private bool FilterItems(object item)
        {
            bool result = true;

            var lang = item as SettingValue;

            var existLang = _selectedLanguages.SingleOrDefault(l => l.LanguageCode.Contains(lang.ShortTextValue));
            if (existLang != null)
            {
                result = false;
            }

            return result;
        }

        #endregion


        #region Properties

        private GeneralLanguage _innerItem;
        public GeneralLanguage InnerItem
        {
            get { return _innerItem; }
            set
            {
                _innerItem = value;
                OnPropertyChanged();
                OnPropertyChanged("IsValid");
            }
        }


        private Setting _languageSetting;
        public Setting LanguageSetting
        {
            get { return _languageSetting; }
            set
            {
                _languageSetting = value;
                OnPropertyChanged();
            }
        }


        public bool IsValid
        {
            get
            {
                bool result = false;

                result = !string.IsNullOrEmpty(InnerItem.LanguageCode) && !string.IsNullOrEmpty(InnerItem.DisplayName);

                return result;
            }
        }


        #endregion


        void InnerItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            InnerItem.PropertyChanged -= InnerItem_PropertyChanged;
            OnPropertyChanged("IsValid");
            InnerItem.PropertyChanged += InnerItem_PropertyChanged;
        }


    }
}
