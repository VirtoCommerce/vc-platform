using System.Collections.ObjectModel;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Tiles
{
    public class ListTileItem : TileItem, IListTileItem
    {
        private ObservableCollection<string> _infoList = new ObservableCollection<string>();
        public ObservableCollection<string> InfoList
        {
            get { return _infoList; }
            set 
            {
                _infoList = value; 
                OnPropertyChanged(); 
            }
        }

    }
}
