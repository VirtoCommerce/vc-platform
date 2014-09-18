using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using System.Globalization;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Catalogs.Factories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Wizard
{
    public class EditorialReviewViewModel : ViewModelDetailAndWizardBase<EditorialReview>, IEditorialReviewViewModel
    {
        #region Dependencies

        private readonly INavigationManager _navManager;

        #endregion

        #region Fields

        private readonly Item _originalCatalogItem;

        #endregion

        /// <summary>
        /// public. For viewing
        /// </summary>
        public EditorialReviewViewModel(ICatalogEntityFactory entityFactory, INavigationManager navManager,
                                        EditorialReview item, bool isWizardMode)
            : base(entityFactory, item, isWizardMode)
        {
            _navManager = navManager;
            _originalCatalogItem = OriginalItem.CatalogItem;

            ViewTitle = new ViewTitleBase()
            {
                Title = "Editorial Review",
                SubTitle = (item != null && item.CatalogItem != null && !string.IsNullOrEmpty(item.CatalogItem.Name))
                               ? string.Format("ER: {0}".Localize(), item.CatalogItem.Name).ToUpper(CultureInfo.InvariantCulture)
                               : ""
            };

            if (!IsWizardMode)
            {
                ReviewStateSetActiveCommand = new DelegateCommand(() =>
                    {
                        InnerItem.ReviewState = ReviewState.Active.GetHashCode();
                        RaiseCanExecuteChanged();
                    }, () => InnerItem.ReviewState != ReviewState.Active.GetHashCode());
                ReviewStateSetDraftCommand = new DelegateCommand(() =>
                    {
                        InnerItem.ReviewState = ReviewState.Draft.GetHashCode();
                        RaiseCanExecuteChanged();
                    }, () => InnerItem.ReviewState != ReviewState.Draft.GetHashCode());

                OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));
            }
        }

        public DelegateCommand ReviewStateSetActiveCommand { get; private set; }
        public DelegateCommand ReviewStateSetDraftCommand { get; private set; }

        #region ViewModelBase overrides

        public override string DisplayName
        {
            get
            {
                return string.Format("ER: {0}".Localize(), InnerItem.CatalogItem.Name);
            }
        }

        public override string IconSource
        {
            get
            {
                return "Icon_EditorialReview";
            }
        }

        private NavigationItem _navigationData;
        public override NavigationItem NavigationData
        {
            get
            {
                // after closing navigate back to parent Item:
                return _navigationData ??
                       (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.EditorialReviewId), InnerItem.CatalogItem.ItemId, NavigationNames.MenuName, this));
            }
        }


        #endregion

        #region ViewModelDetailAndWizardBase Members

        public override string ExceptionContextIdentity { get { return string.Format("Editorial Review ({0})", DisplayName); } }

        protected override void GetRepository()
        {
        }

        protected override bool IsValidForSave()
        {
            return InnerItem.Validate();
        }

        /// <summary>
        /// Return RefusedConfirmation for Cancel Confirm dialog
        /// </summary>
        protected override RefusedConfirmation CancelConfirm()
        {
            return new RefusedConfirmation
            {
                Content = string.Format("Save changes to Editorial Review '{0}'?".Localize(), DisplayName),
                Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
            };
        }

        protected override void LoadInnerItem()
        {
            OnUIThread(() => InnerItem = OriginalItem.DeepClone(EntityFactory as IKnownSerializationTypes));
        }

        protected override void DoSaveChanges()
        {
            if (HasPermission())
            {
                OnUIThread(() =>
                {
                    if (InnerItem.Content == "<P />" || InnerItem.Content == "<SPAN />" || InnerItem.Content == "<DIV />")
                        InnerItem.Content = "";

                    OriginalItem.InjectFrom(InnerItem);
                    OriginalItem.CatalogItem = _originalCatalogItem;
                    // no real saving to repository; saving to parent item
                    if (!OriginalItem.CatalogItem.EditorialReviews.Contains(OriginalItem))
                    {
                        OriginalItem.CatalogItem.EditorialReviews.Add(OriginalItem);
                    }
                });
            }
        }

        #endregion

        #region IWizardStep Members

        public override bool IsValid
        {
            get
            {
                var result = InnerItem.Validate();

                var isInnerItemEmpty = InnerItem.Priority == 1
                        && string.IsNullOrEmpty(InnerItem.Content)
                        && string.IsNullOrEmpty(InnerItem.Comments);

                if (!result)
                {
                    result = isInnerItemEmpty;
                }

                if (result)
                {
                    if (isInnerItemEmpty && InnerItem.CatalogItem.EditorialReviews.Contains(InnerItem))
                    {
                        InnerItem.CatalogItem.EditorialReviews.Remove(InnerItem);
                    }
                    else if (!isInnerItemEmpty && !InnerItem.CatalogItem.EditorialReviews.Contains(InnerItem))
                    {
                        InnerItem.CatalogItem.EditorialReviews.Add(InnerItem);
                    }
                }

                return result;
            }
        }

        public override string Comment
        {
            get
            {
                return "Fill Editorial Review information in order to create Review. Or you can leave all fields empty to skip this step.".Localize();
            }
        }

        public override string Description
        {
            get
            {
                return "Editorial Review".Localize();
            }
        }

        #endregion

        #region private members

        private void RaiseCanExecuteChanged()
        {
            ReviewStateSetActiveCommand.RaiseCanExecuteChanged();
            ReviewStateSetDraftCommand.RaiseCanExecuteChanged();
        }

        #endregion

    }
}
