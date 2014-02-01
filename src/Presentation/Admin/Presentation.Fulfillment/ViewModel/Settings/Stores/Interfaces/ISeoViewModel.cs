using System.Collections.Generic;
using VirtoCommerce.ManagementClient.Fulfillment.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Interfaces
{
	public interface ISeoViewModel : IViewModel
	{
		List<SeoUrlKeyword> SeoKeywords { get; }
		bool IsValid { get; }
		void SaveSeoKeywordsChanges();
	}
}
