#region Usings

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Stores.Model;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Implementations;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Interfaces;
using Omu.ValueInjecter;
using VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Security.ViewModel.Interfaces;

#endregion

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Implementations
{
	public class StoreSeoViewModel : SeoViewModelBase
	{
		#region Dependencies

		private readonly ILoginViewModel _loginViewModel;
		private readonly Store _store;
		
		#endregion

		public StoreSeoViewModel(ILoginViewModel loginViewModel, IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory, IAppConfigEntityFactory appConfigEntityFactory, Store item, IEnumerable<string> languages)
			: base(appConfigRepositoryFactory, appConfigEntityFactory, item.DefaultLanguage, languages, item.StoreId, SeoUrlKeywordTypes.Store)
		{
			_loginViewModel = loginViewModel;
			_store = item;

			InitializePropertiesForViewing();
		}
		
		protected override string BuildBaseUrl(Model.SeoUrlKeyword keyword)
		{
			var stringBuilder = new StringBuilder();

			var storeUrl = string.IsNullOrEmpty(_store.Url) ? _store.SecureUrl : _store.Url;

			if (!string.IsNullOrEmpty(storeUrl))
				stringBuilder.AppendFormat("{0}{1}{2}", storeUrl, storeUrl.EndsWith("/") ? null : "/", keyword.Language.ToLower());
			else
				stringBuilder.AppendFormat("{0}{1}{2}", _loginViewModel.CurrentUser.BaseUrl, _loginViewModel.CurrentUser.BaseUrl.EndsWith("/") ? null : "/", keyword.Language.ToLower());
			return stringBuilder.ToString();
		}					
	}
}
