#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using PropertyChanged;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.ManagementClient.Catalog.Model;
using ObjectModel = VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using Omu.ValueInjecter;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

#endregion

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
	[ImplementPropertyChanged]
	public abstract class SeoViewModelBase : ViewModelBase, ISeoViewModel
	{
		#region Dependencies

		protected readonly IRepositoryFactory<IAppConfigRepository> _appConfigRepositoryFactory;
		private readonly IAppConfigEntityFactory _appConfigEntityFactory;
		private readonly string _defaultLanguage;
		private readonly string _keywordValue;
		private readonly ObjectModel.SeoUrlKeywordTypes _keywordType;
		private readonly List<string> _availableLanguages;
		
		#endregion

		#region Constructor
		
		protected SeoViewModelBase(
			IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory, 
			IAppConfigEntityFactory appConfigEntityFactory, 
			string defaultLanguage, 
			IEnumerable<string> languages,
			string keywordValue,
			ObjectModel.SeoUrlKeywordTypes keywordType)
		{
			_appConfigRepositoryFactory = appConfigRepositoryFactory;
			_appConfigEntityFactory = appConfigEntityFactory;
			_availableLanguages = languages.ToList();
			_defaultLanguage = defaultLanguage;
			_keywordValue = keywordValue;
			_keywordType = keywordType;

			InitCommands();
		}

		#endregion
		
		#region Public properties

		public string NavigateUrl
		{
			get
			{
				return string.IsNullOrEmpty(CurrentSeoKeyword.BaseUrl)
					                ? string.Empty
									: string.Format("{0}/{1}", CurrentSeoKeyword.BaseUrl, string.IsNullOrEmpty(CurrentSeoKeyword.Keyword) ? string.IsNullOrEmpty(SeoKeywords.First(x => x.Language.Equals(_defaultLanguage, StringComparison.OrdinalIgnoreCase)).Keyword) ? CurrentSeoKeyword.KeywordValue : SeoKeywords.First(x => x.Language.Equals(_defaultLanguage, StringComparison.OrdinalIgnoreCase)).Keyword : CurrentSeoKeyword.Keyword).ToLower();
			}
		}

		public bool IsValid
		{
			get
			{
				var retVal = SeoKeywords.All(keyword => keyword.Validate()) && ValidateKeywords();
				if (!retVal)
					SeoLocalesFilterCommand.Execute(SeoKeywords.First(x => x.Errors.Any()));

				return retVal;
			}
		}

		public string Description
		{
			get
			{
				return "Enter SEO information.".Localize();
			}
		}

		public SeoUrlKeyword CurrentSeoKeyword { get; set; }

		#endregion

		#region ISeoViewModel
		
		public List<SeoUrlKeyword> SeoKeywords { get; protected set; }

		public void ChangeKeywordValue(string newCode)
		{
			SeoKeywords.ForEach(x =>
				{
					x.KeywordValue = newCode;
				});
		}

		public virtual void SaveSeoKeywordsChanges()
		{
			//if any SEO keyword modified update or add it
			if (SeoKeywords.Any(x => x.IsChanged))
			{
				using (var appConfigRepository = _appConfigRepositoryFactory.GetRepositoryInstance())
				{
					SeoKeywords.Where(x => x.IsChanged && x.Validate()).ToList().ForEach(keyword =>
					{
						if (string.IsNullOrEmpty(keyword.Keyword))
						{
							//redundant true is a workaround for service error
							var keywordToRemove =
								appConfigRepository.SeoUrlKeywords.Where(
									seoKeyword => true && seoKeyword.SeoUrlKeywordId.Equals(keyword.SeoUrlKeywordId)).FirstOrDefault();
							if (keywordToRemove != null)
							{
								appConfigRepository.Remove(keywordToRemove);
							}
						}
						else
						{
							//redundant true is a workaround for service error
							var originalKeyword =
								appConfigRepository.SeoUrlKeywords.Where(
									seoKeyword => true &&
									              seoKeyword.SeoUrlKeywordId.Equals(keyword.SeoUrlKeywordId))
								                   .FirstOrDefault();

							if (originalKeyword != null)
							{
								originalKeyword.Title = keyword.Title;
								originalKeyword.MetaDescription = keyword.MetaDescription;
								originalKeyword.Keyword = keyword.Keyword;
								originalKeyword.ImageAltDescription = keyword.ImageAltDescription;
								originalKeyword.KeywordValue = keyword.KeywordValue;
								appConfigRepository.Update(originalKeyword);
							}
							else
							{
								var addKeyword = _appConfigEntityFactory.CreateEntity<ObjectModel.SeoUrlKeyword>();
								addKeyword.IsActive = true;
								addKeyword.InjectFrom(keyword);
								appConfigRepository.Add(addKeyword);
							}
						}
					});

					appConfigRepository.UnitOfWork.Commit();
					SeoKeywords.ForEach(y => y.IsChanged = false);
					OnPropertyChanged("NavigateUrl");
				}
			}
		}
		
		#endregion

		#region Use custom properties

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

		#endregion

		#region Commands

		public DelegateCommand<SeoUrlKeyword> SeoLocalesFilterCommand { get; private set; }
		public DelegateCommand NavigateToUrlCommand { get; private set; }
		public DelegateCommand UpdateCustomProperties { get; private set; }
		
		#endregion

		#region virtual and abstract methods
		
		protected virtual void InitializePropertiesForViewing()
		{
			SeoKeywords = new List<SeoUrlKeyword>();
			using (var _appConfigRepository = _appConfigRepositoryFactory.GetRepositoryInstance())
			{
				_appConfigRepository.SeoUrlKeywords.Where(
					keyword =>
					keyword.KeywordValue.Equals(_keywordValue, StringComparison.InvariantCultureIgnoreCase) && keyword.KeywordType.Equals((int) _keywordType) && keyword.IsActive)
									.ToList().ForEach(seo =>
										{
											var newSeo = new SeoUrlKeyword(seo);
											newSeo.BaseUrl = BuildBaseUrl(newSeo);
											newSeo.PropertyChanged += CurrentSeoKeyword_PropertyChanged;
											SeoKeywords.Add(newSeo);
										});
			}

			_availableLanguages.ForEach(locale => 
				{
					if (!SeoKeywords.Any(keyword => keyword.Language.Equals(locale, StringComparison.InvariantCultureIgnoreCase)))
					{
						var newSeoKeyword = new SeoUrlKeyword(locale, (int)_keywordType, _keywordValue);
						newSeoKeyword.BaseUrl = BuildBaseUrl(newSeoKeyword);
						newSeoKeyword.PropertyChanged += CurrentSeoKeyword_PropertyChanged;
						SeoKeywords.Add(newSeoKeyword);
					}
				});

			SeoLocalesFilterCommand.Execute(SeoKeywords.FirstOrDefault(x => x.Language.Equals(_defaultLanguage, StringComparison.InvariantCultureIgnoreCase)));
		}

		protected abstract string BuildBaseUrl(SeoUrlKeyword keyword);

		#endregion

		#region Auxilliary methods

		private void InitCommands()
		{
			SeoLocalesFilterCommand = new DelegateCommand<SeoUrlKeyword>(RaiseSeoLocaleChange);
			NavigateToUrlCommand = new DelegateCommand(RaiseNavigateToUrl);
			UpdateCustomProperties = new DelegateCommand(RaiseUpateCustomProperties);
		}

		private void RaiseUpateCustomProperties()
		{
			_useCustomImageText = !string.IsNullOrEmpty(CurrentSeoKeyword.ImageAltDescription);
			_useCustomMetaDescription = !string.IsNullOrEmpty(CurrentSeoKeyword.MetaDescription);
			_useCustomTitle = !string.IsNullOrEmpty(CurrentSeoKeyword.Title);
			OnPropertyChanged("UseCustomTitle");
			OnPropertyChanged("UseCustomMetaDescription");
			OnPropertyChanged("UseCustomImageText");
		}

		private void RaiseSeoLocaleChange(SeoUrlKeyword currentKeyword)
		{
			CurrentSeoKeyword = currentKeyword;
			RaiseUpateCustomProperties();
			OnPropertyChanged("NavigateUrl");
		}

		void CurrentSeoKeyword_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			CurrentSeoKeyword.Validate();
		}
		
		private void RaiseNavigateToUrl()
		{
			System.Diagnostics.Process.Start(NavigateUrl);
		}

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
																  !x.SeoUrlKeywordId.Equals(keyword.SeoUrlKeywordId, StringComparison.InvariantCultureIgnoreCase) &&
																  x.Keyword.Equals(keyword.Keyword, StringComparison.InvariantCultureIgnoreCase) && x.KeywordType == keyword.KeywordType && x.Language.Equals(keyword.Language, StringComparison.InvariantCultureIgnoreCase) && x.IsActive)
														   .Count();

							if (count > 0)
							{
								keyword.SetError("Keyword", string.Format("{0} with the same Keyword and Language already exists".Localize(), _keywordType.ToString()), false);
								if (keyword.SeoUrlKeywordId.Equals(CurrentSeoKeyword.SeoUrlKeywordId, StringComparison.InvariantCultureIgnoreCase))
									OnPropertyChanged("CurrentSeoKeyword");
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
