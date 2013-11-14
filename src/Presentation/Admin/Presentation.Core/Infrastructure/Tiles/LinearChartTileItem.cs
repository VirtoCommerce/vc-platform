using System.Collections.Generic;
using System.Windows.Media;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Tiles
{
    public class LinearChartTileItem : TileItem, ILinearChartTileItem
    {
        private string _columnSeriesName1;

        public string ColumnSeriesName1
        {
            get { return _columnSeriesName1; }
            set
            {
                _columnSeriesName1 = value;
                OnPropertyChanged();
            }
        }

        private Dictionary<string, int> _seriasArrays1 = new Dictionary<string, int>();

        public Dictionary<string, int> SeriasArrays1
        {
            get { return _seriasArrays1; }
            set
            {
                _seriasArrays1 = value;
                OnPropertyChanged();
            }
        }

        private SolidColorBrush _seriasBackground1 = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        public SolidColorBrush SeriasBackground1
        {
            get { return _seriasBackground1; }
            set
            {
                _seriasBackground1 = value;
                OnPropertyChanged();
            }
        }


        private string _columnSeriesName2;

        public string ColumnSeriesName2
        {
            get { return _columnSeriesName2; }
            set
            {
                _columnSeriesName2 = value;
                OnPropertyChanged();
            }
        }

        private Dictionary<string, int> _seriasArrays2 = new Dictionary<string, int>();

        public Dictionary<string, int> SeriasArrays2
        {
            get { return _seriasArrays2; }
            set
            {
                _seriasArrays2 = value;
                OnPropertyChanged();
            }
        }

        private SolidColorBrush _seriasBackground2 = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        public SolidColorBrush SeriasBackground2
        {
            get { return _seriasBackground2; }
            set
            {
                _seriasBackground2 = value;
                OnPropertyChanged();
            }
        }

    }
}
