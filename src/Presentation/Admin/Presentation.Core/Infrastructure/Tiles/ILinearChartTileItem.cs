
using System.Collections.Generic;
using System.Windows.Media;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Tiles
{
    public interface ILinearChartTileItem
    {
        string ColumnSeriesName1 { get; }
        Dictionary<string, int> SeriasArrays1 { get; }
        SolidColorBrush SeriasBackground1 { get; }

        string ColumnSeriesName2 { get; }
        Dictionary<string, int> SeriasArrays2 { get; }
        SolidColorBrush SeriasBackground2 { get; }
    }
}
