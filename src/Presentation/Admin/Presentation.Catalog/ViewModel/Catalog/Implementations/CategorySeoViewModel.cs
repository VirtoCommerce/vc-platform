using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Security.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
	public sealed class CategorySeoViewModel : SeoViewModelBase, ICategorySeoViewModel
	{
		#region Dependencies

		private readonly IRepositoryFactory<IStoreRepository> _storeRepositoryFactory;
		private readonly ICatalogOutlineBuilder _catalogBuilder;
		private readonly ILoginViewModel _loginViewModel;
		private readonly CategoryBase _category;
		private readonly CatalogBase _catalog;

		#endregion

		public CategorySeoViewModel(ILoginViewModel loginViewModel, ICatalogOutlineBuilder catalogBuilder, IRepositoryFactory<IStoreRepository> storeRepositoryFactory, IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory, IAppConfigEntityFactory appConfigEntityFactory, Category item, IEnumerable<string> languages, CatalogBase parentCatalog)
			: base(appConfigRepositoryFactory, appConfigEntityFactory, parentCatalog.DefaultLanguage, languages, item.CategoryId, SeoUrlKeywordTypes.Category)
		{
			_storeRepositoryFactory = storeRepositoryFactory;
			_catalogBuilder = catalogBuilder;
			_loginViewModel = loginViewModel;
			_category = item;
			_catalog = parentCatalog;

			InitializePropertiesForViewing();
		}
		
		protected override string BuildBaseUrl(Model.SeoUrlKeyword keyword)
		{
			var stringBuilder = new StringBuilder();
			var categoryOutline = _catalogBuilder.BuildCategoryOutline(_catalog.CatalogId, _category);
			if (categoryOutline != null)
			{
				using (var storeRepo = _storeRepositoryFactory.GetRepositoryInstance())
				{
					var store = storeRepo.Stores.Where(x => x.Catalog.Equals(categoryOutline.CatalogId)).FirstOrDefault();
					if (store != null)
					{
						var storeUrl = string.IsNullOrEmpty(store.Url) ? store.SecureUrl : store.Url;

						if (!string.IsNullOrEmpty(storeUrl))
							stringBuilder.AppendFormat("{0}{1}{2}", storeUrl, storeUrl.EndsWith("/") ? null : "/", keyword.Language.ToLowerInvariant());
						else
						{
							stringBuilder.AppendFormat("{0}{1}{2}/", _loginViewModel.CurrentUser.BaseUrl, _loginViewModel.CurrentUser.BaseUrl.EndsWith("/") ? null : "/", keyword.Language.ToLowerInvariant());

							using (var seoRepo = _appConfigRepositoryFactory.GetRepositoryInstance())
							{
								var storeSeo = seoRepo.SeoUrlKeywords.Where(
									x => x.KeywordValue == store.StoreId && x.Language == keyword.Language)
													  .FirstOrDefault() ??
											   seoRepo.SeoUrlKeywords.Where(
												   x => x.KeywordValue == store.StoreId && x.Language == store.DefaultLanguage)
													  .FirstOrDefault();
								if (storeSeo != null)
								{
									stringBuilder.AppendFormat("{0}", storeSeo.Keyword);
								}
							}
						}
					}
				}

				if (!string.IsNullOrEmpty(stringBuilder.ToString()) && categoryOutline.Categories.Any(cat => cat.CategoryId != keyword.KeywordValue))
				{
					using (var seoRepo = _appConfigRepositoryFactory.GetRepositoryInstance())
					{
						categoryOutline.Categories.ForEach(cat =>
							{
								if (cat.CategoryId != keyword.KeywordValue)
								{
									var storeSeo = seoRepo.SeoUrlKeywords.Where(
										x => x.KeywordValue == cat.CategoryId && x.Language == keyword.Language)
									                      .FirstOrDefault() ??
									               seoRepo.SeoUrlKeywords.Where(
										               x => x.KeywordValue.Equals(cat.CategoryId, StringComparison.InvariantCultureIgnoreCase) && x.Language.Equals(_catalog.CatalogId, StringComparison.InvariantCultureIgnoreCase))
									                      .FirstOrDefault();
									stringBuilder.AppendFormat("/{0}", storeSeo != null ? storeSeo.Keyword : cat.CategoryId);
								}

							});
					}
				}
			}

			return stringBuilder.ToString();
		}
	}
}
