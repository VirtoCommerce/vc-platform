using System.Collections.Generic;
using System.ComponentModel;
using System.Waf.Applications.Services;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Customers.Factories;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseLabels.Interfaces;
namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CaseLabels.Implementations
{
    public class LabelViewModel : ViewModelBase, ILabelViewModel
    {
        #region Dependencies

        private readonly ICustomerEntityFactory _entityFactory;
        private readonly Label _innerItem;
        private readonly IFileDialogService _fileDialogService;

        #endregion

        #region Constructor

        public LabelViewModel(Label item, ICustomerEntityFactory _customerEntityFactory, IFileDialogService fileDialogService)
        {
            _entityFactory = _customerEntityFactory;
            _innerItem = item.DeepClone(_entityFactory as IKnownSerializationTypes);
            _innerItem.PropertyChanged += _innerItem_PropertyChanged;

	        _fileDialogService = fileDialogService;

            CommandsInit();
        }

        #endregion
		
        #region Commands

        public DelegateCommand AddImageCommand { get; private set; }
        public DelegateCommand DeleteImageCommand { get; private set; }

        #endregion


        #region Commands Implementation

        private void AddImage()
        {

            IEnumerable<System.Waf.Applications.Services.FileType> fileTypes = new[] {
                new System.Waf.Applications.Services.FileType("all files", ".*"),
                new System.Waf.Applications.Services.FileType("jpg image", ".jpg"),
                new System.Waf.Applications.Services.FileType("bmp image", ".bmp"),
                new System.Waf.Applications.Services.FileType("png image", ".png")
            };

            var result = _fileDialogService.ShowOpenFileDialog(this, fileTypes);
            if (result.IsValid)
            {
                InnerItem.ImgUrl = result.FileName;
            }

        }

        private void DeleteImage()
        {
            InnerItem.ImgUrl = null;
        }

        #endregion

        #region LabelProperties

        public Label InnerItem
        {
            get
            {
                return _innerItem;
            }
        }

        #endregion

        #region publicPropeties

        public bool IsValid
        {
            get
            {
                return Validate();
            }
        }

        #endregion

        #region Private Methods

        private bool Validate()
        {
            return _innerItem.Validate();
        }

        private void CommandsInit()
        {
            AddImageCommand = new DelegateCommand(AddImage);
            DeleteImageCommand = new DelegateCommand(DeleteImage, () => !string.IsNullOrEmpty(InnerItem.ImgUrl));
        }

        #endregion

        #region Handlers

        void _innerItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _innerItem.PropertyChanged -= _innerItem_PropertyChanged;
            DeleteImageCommand.RaiseCanExecuteChanged();
            OnPropertyChanged("IsValid");
            _innerItem.PropertyChanged += _innerItem_PropertyChanged;
        }

        #endregion

    }
}
