using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.Foundation.AppConfig.Factories;
using VirtoCommerce.Foundation.AppConfig.Model;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using Omu.ValueInjecter;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
	public class ItemSeoStepViewModel : ItemViewModel, IItemSeoStepViewModel
	{
		#region Dependencies

		private readonly IRepositoryFactory<IAppConfigRepository> _appConfigRepositoryFactory;
		private readonly IAppConfigEntityFactory _appConfigEntityFactory;
		
		#endregion

		public ItemSeoStepViewModel(IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory, IAppConfigEntityFactory appConfigEntityFactory, Item item, IEnumerable<string> languages)
			: base(null, null, null, null, item, null, null)
		{
			_appConfigRepositoryFactory = appConfigRepositoryFactory;
			_appConfigEntityFactory = appConfigEntityFactory;
			InnerItemCatalogLanguages = languages.ToList();
			SeoLocalesFilterCommand = new DelegateCommand<string>(RaiseSeoLocalesFilter);
		}
				
		#region IWizardStep Members

		public override bool IsValid
		{
			get
			{
				return ValidateKeywords() && (!_seoModified || SeoKeywords.All(keyword => keyword.Validate() || (string.IsNullOrEmpty(keyword.Keyword) && string.IsNullOrEmpty(keyword.ImageAltDescription) && string.IsNullOrEmpty(keyword.Title) && string.IsNullOrEmpty(keyword.MetaDescription))));
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

			//attach property changed
			CurrentSeoKeyword.PropertyChanged += CurrentSeoKeyword_PropertyChanged;

			FilterSeoLanguage = locale;
		}
		
		void CurrentSeoKeyword_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			_seoModified = true;
			OnViewModelPropertyChangedUI(null, null);
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
									seoKeyword =>
									seoKeyword.KeywordValue.Equals(keyword.KeywordValue) && seoKeyword.Language.Equals(keyword.Language) && seoKeyword.IsActive)
												   .FirstOrDefault();

							if (originalKeyword != null)
							{
								originalKeyword.InjectFrom(keyword);
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

				OnPropertyChanged("UseCustomTitle");
				OnPropertyChanged("UseCustomMetaDescription");
				OnPropertyChanged("UseCustomImageText");
			}
		}
		public DelegateCommand<string> SeoLocalesFilterCommand { get; private set; }

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
