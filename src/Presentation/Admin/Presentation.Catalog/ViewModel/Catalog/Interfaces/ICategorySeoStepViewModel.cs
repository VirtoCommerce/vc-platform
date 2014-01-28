using System.Collections.Generic;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces
{
	public interface IItemSeoStepViewModel : IWizardStep
	{
		List<SeoUrlKeyword> SeoKeywords { get; }
		SeoUrlKeyword CurrentSeoKeyword { get; }
		void UpdateSeoKeywords();
		void UpdateKeywordValueCode(string newCode);
	}
}
