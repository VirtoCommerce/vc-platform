using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace VirtoCommerce.ManagementClient.Main.Infrastructure
{
	public interface INewsTileItem
	{
		IEnumerable<SyndicationItem> NewsList { get; set; }
	}
}
