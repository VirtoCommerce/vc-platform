using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Services;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Stores.Repositories;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.ManagementClient.Security.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
	public class ItemSeoStepViewModel : ItemViewModel, IItemSeoStepViewModel
	{
		#region Dependencies

		private readonly IRepositoryFactory<IAppConfigRepository> _appConfigRepositoryFactory;
		private readonly IAppConfigEntityFactory _appConfigEntityFactory;
		private readonly ICatalogOutlineBuilder _catalogBuilder;
		private readonly IRepositoryFactory<IStoreRepository> _storeRepositoryFactory;
		private readonly ILoginViewModel _loginViewModel;
		
		#endregion

		public ItemSeoStepViewModel(ILoginViewModel loginViewModel, ICatalogOutlineBuilder catalogBuilder, IRepositoryFactory<IStoreRepository> storeRepositoryFactory, IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory, IAppConfigEntityFactory appConfigEntityFactory, Item item, IEnumerable<string> languages)
			: base(null, null, null, null, item, null, null)
		{
			_appConfigRepositoryFactory = appConfigRepositoryFactory;
			_appConfigEntityFactory = appConfigEntityFactory;
			_storeRepositoryFactory = storeRepositoryFactory;
			_catalogBuilder = catalogBuilder;
			_loginViewModel = loginViewModel;
			InnerItemCatalogLanguages = languages.ToList();
			SeoLocalesFilterCommand = new DelegateCommand<string>(RaiseSeoLocalesFilter);
			NavigateToUrlCommand = new DelegateCommand(RaiseNavigateToUrl);
		}

		private void RaiseNavigateToUrl()
		{
			System.Diagnostics.Process.Start(NavigateUri);
		}
				
		#region IWizardStep Members

		public override bool IsValid
		{
			get
			{
				return ValidateKeywords() && (!_seoModified || SeoKeywords.All(keyword => (string.IsNullOrEmpty(keyword.Keyword) && string.IsNullOrEmpty(keyword.ImageAltDescription) && string.IsNullOrEmpty(keyword.Title) && string.IsNullOrEmpty(keyword.MetaDescription)) || keyword.Validate()));
			}
		}

		public override bool IsLast
		{
			get
			{
				return false;
			}
		}

		public override string Description
		{
			get
			{
				return "Enter item SEO information.";
			}
		}
		#endregion

		#region SEO tab

		private readonly SeoUrlKeyword _originalKeyword = new SeoUrlKeyword();
		private string _navigateUri;
		public string NavigateUri
		{
			get
			{
				if (string.IsNullOrEmpty(_navigateUri))
				{
					_navigateUri = GetNavigateBaseUri();
				}
				_originalKeyword.InjectFrom(CurrentSeoKeyword);
				return !string.IsNullOrEmpty(_navigateUri) ? string.Format("{0}/{1}", _navigateUri, string.IsNullOrEmpty(_originalKeyword.Keyword) ? _originalKeyword.KeywordValue : _originalKeyword.Keyword) : null;
			}
		}

		private string GetNavigateBaseUri()
		{
			var stringBuilder = new StringBuilder();
			var categoryOutlines = _catalogBuilder.BuildCategoryOutlineWithDSClient(InnerItem.CatalogId, InnerItem.Code);
			if (categoryOutlines != null && categoryOutlines.Any())
			{
				using (var storeRepo = _storeRepositoryFactory.GetRepositoryInstance())
				{
					var store = storeRepo.Stores.Where(x => x.Catalog.Equals(categoryOutlines.First().CatalogId)).FirstOrDefault();
					if (store != null)
					{
						var storeUrl = string.IsNullOrEmpty(store.Url) ? store.SecureUrl : store.Url;

						if (!string.IsNullOrEmpty(storeUrl))
							stringBuilder.AppendFormat("{0}{1}{2}", storeUrl, storeUrl.EndsWith("/") ? null : "/", CurrentSeoKeyword.Language.ToLowerInvariant());
						else
						{
							stringBuilder.AppendFormat("{0}{1}{2}/", _loginViewModel.BaseUrl, _loginViewModel.BaseUrl.EndsWith("/") ? null : "/", CurrentSeoKeyword.Language.ToLowerInvariant());

							using (var seoRepo = _appConfigRepositoryFactory.GetRepositoryInstance())
							{
								var storeSeo = seoRepo.SeoUrlKeywords.Where(
									x => x.KeywordValue == store.StoreId && x.Language == CurrentSeoKeyword.Language)
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
								x => x.KeywordValue == cat.Code && x.Language == CurrentSeoKeyword.Language)
													.FirstOrDefault() ??
											seoRepo.SeoUrlKeywords.Where(
												x => x.KeywordValue == cat.Code && x.Language == InnerItem.Catalog.DefaultLanguage)
													.FirstOrDefault();
							stringBuilder.AppendFormat("/{0}", storeSeo != null ? storeSeo.Keyword : cat.Code);
						});
					}
				}
			}

			return stringBuilder.ToString();
		}

		public List<string> InnerItemStoreLanguages { get; private set; }

		public List<SeoUrlKeyword> SeoKeywords { get; private set; }

		private SeoUrlKeyword _currentSeoKeyword;
		public SeoUrlKeyword CurrentSeoKeyword
		{
			get { return _currentSeoKeyword; }
			set
			{
				_currentSeoKeyword = value;
				OnPropertyChanged();
				_navigateUri = null;
				OnPropertyChanged("NavigateUri");
			}
		}

		private bool _useCustomMetaDescription;
		public bool UseCustomMetaDescription
		{
			get { return _useCustomMetaDescription || !string.IsNullOrEmpty(CurrentSeoKeyword.MetaDescription); }
			set
			{
				_useCustomMetaDescription = value;
				if (!_useCustomMetaDescription && !string.IsNullOrEmpty(CurrentSeoKeyword.MetaDescription))
					CurrentSeoKeyword.MetaDescription = null;
				OnPropertyChanged();
			}
		}

		private bool _useCustomTitle;
		public bool UseCustomTitle
		{
			get { return _useCustomTitle || !string.IsNullOrEmpty(CurrentSeoKeyword.Title); }
			set
			{
				_useCustomTitle = value;
				if (!_useCustomTitle && !string.IsNullOrEmpty(CurrentSeoKeyword.Title))				
					CurrentSeoKeyword.Title = null;
				OnPropertyChanged();				
			}
		}

		private bool _useCustomImageText;
		public bool UseCustomImageText
		{
			get { return _useCustomImageText || !string.IsNullOrEmpty(CurrentSeoKeyword.ImageAltDescription); }
			set
			{
				_useCustomImageText = value;
				if (!_useCustomImageText && !string.IsNullOrEmpty(CurrentSeoKeyword.ImageAltDescription))
					CurrentSeoKeyword.ImageAltDescription = null;
				OnPropertyChanged();
			}
		}

		private void RaiseSeoLocalesFilter(string locale)
		{
			//detach property changed
			if (CurrentSeoKeyword != null)
				CurrentSeoKeyword.PropertyChanged -= CurrentSeoKeyword_PropertyChanged;

			CurrentSeoKeyword =
				SeoKeywords.FirstOrDefault(keyword => keyword.Language.Equals(locale, StringComparison.InvariantCultureIgnoreCase) && keyword.IsActive);

			if (CurrentSeoKeyword == null)
			{
				CurrentSeoKeyword = new SeoUrlKeyword { Language = locale, IsActive = true, KeywordType = (int)SeoUrlKeywordTypes.Item, KeywordValue = InnerItem.Code, Created = DateTime.UtcNow};
				SeoKeywords.Add(CurrentSeoKeyword);
			}
			
			FilterSeoLanguage = locale;

			//attach property changed
			CurrentSeoKeyword.PropertyChanged += CurrentSeoKeyword_PropertyChanged;
		}

		private void ResetProperties()
		{
			_useCustomImageText = false;
			_useCustomMetaDescription = false;
			_useCustomTitle = false;
		}
		
		void CurrentSeoKeyword_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			_seoModified = true;
			//OnViewModelPropertyChangedUI(null, null);
		}

		public void UpdateKeywordValueCode(string newCode)
		{
			SeoKeywords.ForEach(x => x.KeywordValue = newCode);
		}

		public void UpdateSeoKeywords()
		{
			//if item code changed - need to update SEO KeywordValue property, if any
			if (!OriginalItem.Code.Equals(InnerItem.Code))
			{
				if (SeoKeywords.Any(kw => !string.IsNullOrEmpty(kw.Keyword)))
				{
					if (CurrentSeoKeyword != null)
						CurrentSeoKeyword.PropertyChanged -= CurrentSeoKeyword_PropertyChanged;

					_seoModified = true;
					SeoKeywords.ForEach(keyword =>
					{
						if (!string.IsNullOrEmpty(keyword.Keyword))
							keyword.KeywordValue = InnerItem.Code;
					});

					if (CurrentSeoKeyword != null)
						CurrentSeoKeyword.PropertyChanged -= CurrentSeoKeyword_PropertyChanged;
				}
			}

			//if any SEO keyword modified update or add it
			if (_seoModified)
			{
				using (var appConfigRepository = _appConfigRepositoryFactory.GetRepositoryInstance())
				{
					SeoKeywords.ForEach(keyword =>
					{
						if (!string.IsNullOrEmpty(keyword.Keyword))
						{
							var originalKeyword =
								appConfigRepository.SeoUrlKeywords.Where(
									seoKeyword => true &&
									seoKeyword.SeoUrlKeywordId.Equals(keyword.SeoUrlKeywordId))
												   .FirstOrDefault();

							if (originalKeyword != null)
							{
								originalKeyword.InjectFrom<CloneInjection>(keyword);
								appConfigRepository.Update(originalKeyword);
							}
							else
							{
								var addKeyword = _appConfigEntityFactory.CreateEntity<SeoUrlKeyword>();
								addKeyword.InjectFrom(keyword);
								appConfigRepository.Add(addKeyword);
							}
						}
					});
					appConfigRepository.UnitOfWork.Commit();
				}
				OnPropertyChanged("NavigateUri");
				_seoModified = false;
			}
		}

		private bool _seoModified;

		private string _filterSeoLanguage;
		public string FilterSeoLanguage 
		{ 
			get
			{
				return _filterSeoLanguage;
			}
			private set
			{
				_filterSeoLanguage = value;
				OnPropertyChanged();
				ResetProperties();
				OnPropertyChanged("UseCustomTitle");
				OnPropertyChanged("UseCustomMetaDescription");
				OnPropertyChanged("UseCustomImageText");
			}
		}

		public DelegateCommand<string> SeoLocalesFilterCommand { get; private set; }
		public DelegateCommand NavigateToUrlCommand { get; private set; }

		#endregion

		protected override void InitializePropertiesForViewing()
		{
			using (var _appConfigRepository = _appConfigRepositoryFactory.GetRepositoryInstance())
			{
				SeoKeywords =
					_appConfigRepository.SeoUrlKeywords.Where(
						keyword =>
						keyword.KeywordValue.Equals(InnerItem.Code) && keyword.KeywordType.Equals((int)SeoUrlKeywordTypes.Item))
										.ToList();
			}

			InnerItemCatalogLanguages.ForEach(locale => 
				{
					if (!SeoKeywords.Any(keyword => keyword.Language.Equals(locale)))
					{
						var newSeoKeyword = new SeoUrlKeyword { Language = locale, IsActive = true, KeywordType = (int)SeoUrlKeywordTypes.Item, KeywordValue = InnerItem.Code, Created = DateTime.UtcNow };
						SeoKeywords.Add(newSeoKeyword);
					}
				});

			// filter values by locale
			SeoLocalesFilterCommand.Execute(InnerItem.Catalog.DefaultLanguage);
		}

		#region Auxilliary methods

		private bool ValidateKeywords()
		{
			var retVal = true;
			var keywords = SeoKeywords.Where(key => !string.IsNullOrEmpty(key.Keyword)).ToList();
			if (keywords.Any())
			{
				using (var appConfigRepository = _appConfigRepositoryFactory.GetRepositoryInstance())
				{
					keywords.ForEach(keyword =>
					{
						if (retVal)
						{
							var count = appConfigRepository.SeoUrlKeywords
														   .Where(x =>
																  x.SeoUrlKeywordId != keyword.SeoUrlKeywordId &&
																  x.Keyword == keyword.Keyword && x.KeywordType == keyword.KeywordType && x.Language == keyword.Language && x.IsActive == keyword.IsActive)
														   .Count();

							if (count > 0)
							{
								keyword.SetError("Keyword", "Item with the same Keyword and Language already exists", true);
								CurrentSeoKeyword = keyword;
								RaiseSeoLocalesFilter(keyword.Language);
								retVal = false;
							}
						}
					});
				}
			}

			return retVal;
		}
		#endregion
	}
}
