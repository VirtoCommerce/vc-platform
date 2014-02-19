using System.Collections.Generic;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Wizard;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces
{
	public interface ICategorySeoStepViewModel : IWizardStep
	{
		List<SeoUrlKeyword> SeoKeywords { get; }
		SeoUrlKeyword CurrentSeoKeyword { get; }
		void UpdateSeoKeywords();
		void UpdateKeywordValueCode(string newCode);
		string NavigateUri { get; }
		DelegateCommand NavigateToUrlCommand { get; }
	}
}
