using System.Collections.ObjectModel;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Tiles
{
    public interface IListTileItem
    {
        ObservableCollection<string> InfoList { get; }
    }
}
