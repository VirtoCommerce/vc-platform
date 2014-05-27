using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Reviews.Factories;
using VirtoCommerce.Foundation.Reviews.Repositories;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;
using VirtoCommerce.ManagementClient.Reviews.Model;
using VirtoCommerce.ManagementClient.Reviews.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Reviews.ViewModel.Implementations
{
	public class ReviewEditViewModel : ViewModelDetailBase<ReviewBase>, IReviewEditViewModel
	{
		#region Dependencies

		private readonly IRepositoryFactory<IReviewRepository> _repositoryFactory;
		private readonly INavigationManager _navManager;

		#endregion

		#region Constructor

		public ReviewEditViewModel(IRepositoryFactory<IReviewRepository> repositoryFactory, INavigationManager navManager,
			IReviewEntityFactory entityFactory, ReviewBase item)
			: base(entityFactory, item)
		{
			_repositoryFactory = repositoryFactory;
			_navManager = navManager;
			OpenItemCommand = new DelegateCommand(() => _navManager.Navigate(NavigationData));

			ViewTitle = new ViewTitleBase()
				{
                    Title = "Reviews",
					SubTitle = (item != null && !string.IsNullOrEmpty(item.Title)) ? item.Title.ToUpper(CultureInfo.InvariantCulture) : ""
				};
		}

		#endregion

		#region ViewModelBase overrides

		public override string DisplayName
		{
			get
			{
				return InnerItem.Title;
			}
		}

		public override string IconSource
		{
			get
			{
				return string.Empty;
			}
		}

		public override Brush ShellDetailItemMenuBrush
		{
			get
			{
				var result = (SolidColorBrush)Application.Current.TryFindResource("ReviewDetailItemMenuBrush");

				return result ?? base.ShellDetailItemMenuBrush;
			}
		}

		private NavigationItem _navigationData;
		public override NavigationItem NavigationData
		{
			get
			{
				return _navigationData ??
					   (_navigationData = new NavigationItem(GetNavigationKey(OriginalItem.ReviewBaseId), "CatalogHome",
														"ReviewsHome", this));
			}
		}


		#endregion

		#region IReviewEditViewModel Members

		public bool IsReview
		{
			get
			{
				return OriginalItem.ReviewType == ReviewType.Review;
			}
		}

		public bool IsReviewComment
		{
			get
			{
				return OriginalItem.ReviewType == ReviewType.Comment;
			}
		}

		#endregion

		#region ViewModelDetailBase

		public override string ExceptionContextIdentity { get { return string.Format("Review ({0})", DisplayName); } }

		protected override void GetRepository()
		{
			Repository = _repositoryFactory.GetRepositoryInstance();
		}

		protected override bool IsValidForSave()
		{
			return true;
		}

		protected override RefusedConfirmation CancelConfirm()
		{
			return new RefusedConfirmation
				{
                    Content = string.Format("Save changes to Review '{0}'?".Localize(), InnerItem.Title),
					Title = "Action confirmation".Localize(null, LocalizationScope.DefaultCategory)
				};
		}

		protected override void LoadInnerItem()
		{
			if (IsReview)
			{
				var item =
					(Repository as IReviewRepository).Reviews.Where(x => x.ReviewId == OriginalItem.ReviewBaseId).Expand(x => x.ReviewFieldValues).SingleOrDefault();
				OnUIThread(() => InnerItem = new Review(item));
			}
			else
			{
				var item =
					(Repository as IReviewRepository).ReviewComments.Where(x => x.ReviewCommentId == OriginalItem.ReviewBaseId).Expand(x => x.Review).SingleOrDefault();
				OnUIThread(() => InnerItem = new ReviewComment(item));
			}
		}

		protected override void DoSaveChanges()
		{
			try
			{
				if (InnerItem is Review)
				{
					var item =
						(Repository as IReviewRepository).Reviews.Where(review => review.ReviewId == InnerItem.ReviewBaseId)
							.Expand(x => x.ReviewFieldValues)
							.First();
					OnUIThread(() => item.InjectFrom<CloneInjection>(InnerItem));
					item.ReviewFieldValues.First(field => field.Name == "Review").Value = InnerItem.Body;
					Repository.UnitOfWork.CommitAndRefreshChanges();
				}
				else
				{
					var item =
						(Repository as IReviewRepository).ReviewComments.Where(comment => comment.ReviewCommentId == InnerItem.ReviewBaseId).Expand(x => x.Review)
							.SingleOrDefault();
					OnUIThread(() => item.InjectFrom<CloneInjection>(InnerItem));
					item.Comment = InnerItem.Body;
					Repository.UnitOfWork.CommitAndRefreshChanges();
				}
				OnUIThread(() => OriginalItem.InjectFrom<CloneInjection>(InnerItem));
			}
			catch (Exception ex)
			{
				ShowErrorDialog(ex, string.Format("An error occurred when trying to save {0}".Localize(null, LocalizationScope.DefaultCategory), ExceptionContextIdentity));
			}
		}

		#endregion
	}
}
