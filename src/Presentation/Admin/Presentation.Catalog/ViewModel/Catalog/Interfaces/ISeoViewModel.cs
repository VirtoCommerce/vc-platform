using System.Collections.Generic;
using VirtoCommerce.ManagementClient.Catalog.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces
{
	public interface ISeoViewModel : IViewModel
	{
		List<SeoUrlKeyword> SeoKeywords { get; }
		bool IsValid { get; }
		void ChangeKeywordValue(string newCode);
		void SaveSeoKeywordsChanges();
	}
}
