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

		#endregion

		public StoreSeoStepViewModel(IRepositoryFactory<IAppConfigRepository> appConfigRepositoryFactory, Store item)
			: base(null, null, item)
		{
			_appConfigRepositoryFactory = appConfigRepositoryFactory;
			SeoLocalesFilterCommand = new DelegateCommand<string>(RaiseSeoLocalesFilter);
		}
				
		#region IWizardStep Members

		public override bool IsValid
		{
			get
			{
				return !_seoModified || SeoKeywords.All(keyword => keyword.Validate() || (string.IsNullOrEmpty(keyword.Keyword) && string.IsNullOrEmpty(keyword.ImageAltDescription) && string.IsNullOrEmpty(keyword.Title) && string.IsNullOrEmpty(keyword.MetaDescription)));
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
				OnPropertyChanged("CurrentSeoKeyword");
			}
		}

		private bool _useDefaultMetaDescription = true;
		public bool UseDefaultMetaDescription
		{
			get { return _useDefaultMetaDescription && (CurrentSeoKeyword != null && string.IsNullOrEmpty(CurrentSeoKeyword.MetaDescription)); }
			set
			{
				_useDefaultMetaDescription = value;
				if (value && !string.IsNullOrEmpty(CurrentSeoKeyword.MetaDescription))
					CurrentSeoKeyword.MetaDescription = null;
				OnPropertyChanged("UseDefaultMetaDescription");
			}
		}

		private bool _useDefaultTitle = true;
		public bool UseDefaultTitle
		{
			get { return _useDefaultTitle && (CurrentSeoKeyword != null && string.IsNullOrEmpty(CurrentSeoKeyword.Title)); }
			set
			{
				_useDefaultTitle = value;
				if (value && !string.IsNullOrEmpty(CurrentSeoKeyword.Title))
					CurrentSeoKeyword.Title = null;
				OnPropertyChanged("UseDefaultTitle");
			}
		}

		private bool _useDefaultImageText = true;
		public bool UseDefaultImageText
		{
			get { return _useDefaultImageText && (CurrentSeoKeyword != null && string.IsNullOrEmpty(CurrentSeoKeyword.ImageAltDescription)); }
			set
			{
				_useDefaultImageText = value;
				if (value && !string.IsNullOrEmpty(CurrentSeoKeyword.ImageAltDescription))
					CurrentSeoKeyword.ImageAltDescription = null;
				OnPropertyChanged("UseDefaultImageText");
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
				CurrentSeoKeyword = new SeoUrlKeyword { Language = locale, IsActive = true, KeywordType = (int)SeoUrlKeywordTypes.Store, KeywordValue = InnerItem.StoreId, Created = DateTime.UtcNow};
				SeoKeywords.Add(CurrentSeoKeyword);
			}

			//attach property changed
			CurrentSeoKeyword.PropertyChanged += CurrentSeoKeyword_PropertyChanged;

			FilterSeoLanguage = locale;
			OnPropertyChanged("FilterSeoLanguage");

			_useDefaultTitle = true;
			OnPropertyChanged("UseDefaultTitle");

			_useDefaultMetaDescription = true;
			OnPropertyChanged("UseDefaultMetaDescription");

			_useDefaultImageText = true;
			OnPropertyChanged("UseDefaultImageText");
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
									seoKeyword =>
									seoKeyword.KeywordValue.Equals(keyword.KeywordValue) && seoKeyword.Language.Equals(keyword.Language))
												   .FirstOrDefault();

							if (originalKeyword != null)
							{
								originalKeyword.InjectFrom(keyword);
								appConfigRepository.Update(originalKeyword);
							}
							else
							{
								var addKeyword = new SeoUrlKeyword();
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
		public string FilterSeoLanguage { get; private set; }
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
	}
}
