namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Tiles
{
    public class IconTileItem : TileItem, IIconTileItem 
    {
        private string _tileIconSource;

        public string TileIconSource
        {
            get { return _tileIconSource; }
            set
            {
                _tileIconSource = value;
                OnPropertyChanged();
            }
        }

    }
}
