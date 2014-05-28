
namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{

    public class ViewTitleBase : NotifyPropertyChanged
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private string _subTitle;
        public string SubTitle
        {
            get { return _subTitle; }
            set
            {
                _subTitle = value;
                OnPropertyChanged();
            }
        }
    }
}
