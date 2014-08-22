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
	public sealed class ItemSeoViewModel : SeoViewModelBase, IItemSeoViewModel
	{
		#region Dependencies

		private readonly ICatalogOutlineBuilder _catalogBuilder;
		private readonly IRepositoryFactory<IStoreRepository> _storeRepositoryFactory;
		private readonly ILoginViewModel _loginViewModel;
		private readonly Item _item;
		
		#endregion

		public ItemSeoViewModel(ILoginViewModel loginViewModel, ICatalogOutlineBuilder catalogBuilder, IRepositoryFactory<IStoreRepository> storeRepositoryFactory, IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory, IAppConfigEntityFactory appConfigEntityFactory, Item item, IEnumerable<string> languages)
			: base(appConfigRepositoryFactory, appConfigEntityFactory, item.Catalog.DefaultLanguage, languages, item.ItemId, SeoUrlKeywordTypes.Item)
		{
			_storeRepositoryFactory = storeRepositoryFactory;
			_catalogBuilder = catalogBuilder;
			_loginViewModel = loginViewModel;
			_item = item;

			InitializePropertiesForViewing();
		}
		
		protected override string BuildBaseUrl(Model.SeoUrlKeyword keyword)
		{
			var stringBuilder = new StringBuilder();
			var categoryOutlines = _catalogBuilder.BuildCategoryOutline(_item.CatalogId, _item.ItemId);
			if (categoryOutlines != null && categoryOutlines.Any())
			{
				using (var storeRepo = _storeRepositoryFactory.GetRepositoryInstance())
				{
					var store = storeRepo.Stores.Where(x => x.Catalog.Equals(categoryOutlines.First().CatalogId)).FirstOrDefault();
					if (store != null)
					{
						var storeUrl = string.IsNullOrEmpty(store.Url) ? store.SecureUrl : store.Url;

						if (!string.IsNullOrEmpty(storeUrl))
							stringBuilder.AppendFormat("{0}{1}{2}", storeUrl, storeUrl.EndsWith("/") ? null : "/", keyword.Language.ToLower());
						else
						{
							stringBuilder.AppendFormat("{0}{1}{2}/", _loginViewModel.CurrentUser.BaseUrl, _loginViewModel.CurrentUser.BaseUrl.EndsWith("/") ? null : "/", keyword.Language.ToLower());

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

				if (!string.IsNullOrEmpty(stringBuilder.ToString()) && categoryOutlines.First().Categories.Any())
				{
					using (var seoRepo = _appConfigRepositoryFactory.GetRepositoryInstance())
					{
						categoryOutlines.First().Categories.ForEach(cat =>
						{
							var storeSeo = seoRepo.SeoUrlKeywords.Where(
								x => x.KeywordValue == cat.CategoryId && x.Language == keyword.Language)
													.FirstOrDefault() ??
											seoRepo.SeoUrlKeywords.Where(
												x => x.KeywordValue == cat.CategoryId && x.Language == _item.Catalog.DefaultLanguage)
													.FirstOrDefault();
							stringBuilder.AppendFormat("/{0}", storeSeo != null ? storeSeo.Keyword : cat.CategoryId);
						});
					}
				}
			}

			return stringBuilder.ToString();
		}
	}
}
