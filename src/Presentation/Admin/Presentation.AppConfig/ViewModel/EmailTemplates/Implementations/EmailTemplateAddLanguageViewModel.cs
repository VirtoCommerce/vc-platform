using System.Linq;
using System.Windows.Data;
using VirtoCommerce.ManagementClient.AppConfig.Infrastructure.Enumerations;
using VirtoCommerce.ManagementClient.AppConfig.ViewModel.EmailTemplates.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.EmailTemplates.Implementations
{
    public class EmailTemplateAddLanguageViewModel:ViewModelBase,IEmailTemplateAddLanguageViewModel
    {
        #region Fields

        private OperationType _operationType;

        #endregion

        #region Constructor

        public EmailTemplateAddLanguageViewModel(EmailTemplateLanguage item, string[] languageList,
                                                string[] selectedLanguageList,
                                                 OperationType operationType)
        {
            InnerItem = item;
            InnerItem.PropertyChanged -= InnerItem_PropertyChanged;

            LanguageCodeList = languageList;
            SelectedLanguageCodeList = selectedLanguageList;
            _operationType = operationType;


            var view = CollectionViewSource.GetDefaultView(LanguageCodeList);
            view.Filter = FilterItems;
            view.Refresh();

            InnerItem.PropertyChanged += InnerItem_PropertyChanged;
        }

       

        #endregion

        #region Properties

        private string[] _languageCodeList;
        public string[] LanguageCodeList
        {
            get { return _languageCodeList; }
            set
            {
                _languageCodeList = value;
                OnPropertyChanged();
            }
        }

        private string[] _selectedLanguageCodeList;

        public string[] SelectedLanguageCodeList
        {
            get { return _selectedLanguageCodeList; }
            set
            {
                _selectedLanguageCodeList = value;
                OnPropertyChanged();
            }
        }


        public bool IsValid
        {
            get
            {
                return InnerItem.Validate() && !string.IsNullOrEmpty(InnerItem.LanguageCode) &&
                       !string.IsNullOrEmpty(InnerItem.Subject);
            }
        }

        #endregion

        #region IEmailTemplateAddLanguageViewModel members

        private EmailTemplateLanguage _innerItem;
        public EmailTemplateLanguage InnerItem
        {
            get { return _innerItem; }
            set
            {
                _innerItem = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Private Methods

        private bool FilterItems(object item)
        {
            bool result = true;

            string languageCode = item.ToString();

            if (_operationType == OperationType.Add)
            {
                if (SelectedLanguageCodeList.Contains(languageCode))
                {
                    result = false;
                }
            }
            else
            {
                if (SelectedLanguageCodeList.Contains(languageCode) && InnerItem.LanguageCode != languageCode)
                {
                    result = false;
                }
            }

            return result;
        }

        void InnerItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged("IsValid");
        }

        #endregion

    }
}
