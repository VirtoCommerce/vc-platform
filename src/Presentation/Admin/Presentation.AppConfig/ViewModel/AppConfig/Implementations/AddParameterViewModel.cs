using VirtoCommerce.ManagementClient.AppConfig.ViewModel.AppConfig.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.AppConfig.Model;

namespace VirtoCommerce.ManagementClient.AppConfig.ViewModel.AppConfig.Implementations
{
	public class AddParameterViewModel : ViewModelBase, IAddParameterViewModel
    {

        #region Constructor

        public AddParameterViewModel(JobParameter item)
        {
			_selectedParameterValue = item;
        }

        #endregion

        #region Properties
		
        public bool IsValid
        {
            get { return IsInputValueValid(); }
        }
		
        #endregion

		public JobParameter _selectedParameterValue;
		public JobParameter InnerItem
		{
			get { return _selectedParameterValue; }
			set
			{
				_selectedParameterValue = value;
				OnPropertyChanged();
			}
		}

        #region Private Methods

        private bool IsInputValueValid()
        {
            bool result = true;

            return result;
        }

        #endregion

    }
}
