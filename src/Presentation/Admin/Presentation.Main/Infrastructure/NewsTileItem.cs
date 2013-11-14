using System.Collections.Generic;
using System.ServiceModel.Syndication;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Tiles;

namespace VirtoCommerce.ManagementClient.Main.Infrastructure
{
	public class NewsTileItem : TileItem, INewsTileItem
	{
		private IEnumerable<SyndicationItem> newsList;
		public IEnumerable<SyndicationItem> NewsList
		{
			get { return newsList; }
			set { newsList = value; OnPropertyChanged(); }
		}
	}
}
