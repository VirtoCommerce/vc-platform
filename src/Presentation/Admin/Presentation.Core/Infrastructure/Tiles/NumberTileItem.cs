namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Tiles
{
    public class NumberTileItem : TileItem, INumberTileItem 
    {
        private string _tileNumber;

        public string TileNumber
        {
            get { return _tileNumber; }
            set
            {
                _tileNumber = value;
                OnPropertyChanged();
            }
        }

    }
}
