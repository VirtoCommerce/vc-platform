
using System.Collections.Generic;
using System.Linq;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure.Tiles
{
    public sealed class TileManager
    {
        private readonly List<ITileItem> _tileItems = new List<ITileItem>();

        public void AddTile(ITileItem tileItem)
        {
            _tileItems.Add(tileItem);
        }

        public List<ITileItem> GetAllTiles()
        {
            return _tileItems;
        }

        public List<ITileItem> GetTilesByIdModule(string idModule)
        {
			return _tileItems.Where(x => x.IdModule == idModule).OrderBy(x => x.Order).ToList();
        }

        public ITileItem GetTile(string idTile)
        {
            return _tileItems.FirstOrDefault(x => x.IdTile == idTile);
        }

        public void Refresh()
        {
            foreach (var tileItem in _tileItems)
            {
                if (tileItem.Refresh != null)
                {
                    tileItem.Refresh(tileItem);
                }
            }
        }
    }
 
}
