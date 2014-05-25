using System;
using Microsoft.Practices.Prism.Commands;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Tiles
{
    
	
	public enum TileColorSchemas
    {
        Schema1 = 1,
        Schema2 = 2,
        Schema3 = 4,
        Schema4 = 8,
    }

	public enum TileSize
	{
		Single = 150,
		Double = 310,
		Triple = 470,
	}


    public class TileItem : ViewModelBase, ITileItem
    {
        private string _idTile;
        public string IdTile
        {
            get { return _idTile; }
            set
            {
                _idTile = value;
                OnPropertyChanged();
            }
        }

        private string _idModule;
        public string IdModule
        {
            get { return _idModule; }
            set
            {
                _idModule = value;
                OnPropertyChanged();
            }
        }

        private string _tileTitle;
        public string TileTitle
        {
            get { return _tileTitle; }
            set
            {
                _tileTitle = value;
                OnPropertyChanged();
            }
        }

        public string TileCategory { get; set; }

        private TileColorSchemas _idColorSchema = TileColorSchemas.Schema1;
        public TileColorSchemas IdColorSchema
        {
            get { return _idColorSchema; }
            set
            {
                _idColorSchema = value;
                OnPropertyChanged();
            }
        }

		private double _width = (double)TileSize.Single;
		public double Width
        {
			get { return _width; }
            set
            {
				_width = value;
                OnPropertyChanged();
            }
        }

		private double _height = (double)TileSize.Single;
		public double Height
        {
			get { return _height; }
            set
            {
				_height = value;
                OnPropertyChanged();
            }
        }

        private int _order = 1;
        public int Order
        {
			get { return _order; }
            set
            {
				_order = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand NavigateCommand { get; set; }

        public Action<ITileItem> Refresh { get; set; }
    }

}
