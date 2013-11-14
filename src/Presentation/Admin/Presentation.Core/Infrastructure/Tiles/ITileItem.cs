using System;
using Microsoft.Practices.Prism.Commands;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Tiles
{
    public interface ITileItem
    {
        string IdTile { get; set; }
        string IdModule { get; set; }
        string TileTitle { get; }
		double Width { get; }
		double Height { get; }
		int Order { get; }
        TileColorSchemas IdColorSchema { get; }

        DelegateCommand NavigateCommand { get; set; }
        Action<ITileItem> Refresh { get; set; }
    }
}
