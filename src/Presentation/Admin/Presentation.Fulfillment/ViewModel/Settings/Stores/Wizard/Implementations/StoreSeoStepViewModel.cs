using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

namespace VirtoCommerce.ManagementClient.Fulfillment.ViewModel.Settings.Stores.Wizard.Implementations
{
	public class StoreSeoStepViewModel : StoreViewModel, IStoreSeoStepViewModel
	{
		#region Dependencies

		private readonly IRepositoryFactory<IAppConfigRepository> _appConfigRepositoryFactory;
		private readonly IAppConfigEntityFactory _appConfigEntityFactory;
		
		#endregion

		public StoreSeoStepViewModel(IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory, IAppConfigEntityFactory appConfigEntityFactory, Store item)
			: base(null, null, item)
		{
			_appConfigRepositoryFactory = appConfigRepositoryFactory;
			_appConfigEntityFactory = appConfigEntityFactory;
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
				return "Enter store SEO information.";
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
				SeoKeywords.FirstOrDefault(keyword => keyword.Language.Equals(locale, StringComparison.InvariantCultureIgnoreCase));

			if (CurrentSeoKeyword == null)
			{
				CurrentSeoKeyword = _appConfigEntityFactory.CreateEntity<SeoUrlKeyword>();
				CurrentSeoKeyword.Language = locale;
				CurrentSeoKeyword.IsActive = true;
				CurrentSeoKeyword.KeywordType = (int)SeoUrlKeywordTypes.Store;
				CurrentSeoKeyword.KeywordValue = InnerItem.StoreId;
				SeoKeywords.Add(CurrentSeoKeyword);
			}

			//attach property changed
			CurrentSeoKeyword.PropertyChanged += CurrentSeoKeyword_PropertyChanged;

			FilterSeoLanguage = locale;
		}
		
		void CurrentSeoKeyword_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			_seoModified = true;
		}

		public void UpdateSeoKeywords()
		{
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

		private void ResetProperties()
		{
			_useCustomImageText = false;
			_useCustomMetaDescription = false;
			_useCustomTitle = false;
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

		#endregion

		protected override void InitializePropertiesForViewing()
		{
			InnerItemStoreLanguages = InnerItem.Languages.Select(x => x.LanguageCode).ToList();

			using (var _appConfigRepository = _appConfigRepositoryFactory.GetRepositoryInstance())
			{
				SeoKeywords =
					_appConfigRepository.SeoUrlKeywords.Where(
						keyword =>
						keyword.KeywordValue.Equals(InnerItem.StoreId) && keyword.KeywordType.Equals((int)SeoUrlKeywordTypes.Store))
										.ToList();

			}

			InnerItemStoreLanguages.ForEach(locale => 
				{
					if (!SeoKeywords.Any(keyword => keyword.Language.Equals(locale)))
					{
						var newSeoKeyword = new SeoUrlKeyword { Language = locale, IsActive = true, KeywordType = (int)SeoUrlKeywordTypes.Store, KeywordValue = InnerItem.StoreId, Created = DateTime.UtcNow };
						SeoKeywords.Add(newSeoKeyword);
					}
				});
			
			// filter values by locale
			SeoLocalesFilterCommand.Execute(InnerItem.DefaultLanguage);
		}

		#region Auxilliary methods

		private bool ValidateKeywords()
		{
			bool retVal = true;
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
								keyword.SetError("Keyword", "Store with the same Keyword and Language already exists", true);
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
