using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Security.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.DataVirtualization;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Reviews.ViewModel.Interfaces;
using viewModel = VirtoCommerce.ManagementClient.Reviews.Model;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Reviews.Model;
using VirtoCommerce.Foundation.Reviews.Repositories;
using vm = VirtoCommerce.ManagementClient.Reviews.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Tiles;
using System.Threading.Tasks;

namespace VirtoCommerce.ManagementClient.Reviews.ViewModel.Implementations
{
	public class ReviewsHomeViewModel : ViewModelBase, IReviewsHomeViewModel, IVirtualListLoader<IReviewEditViewModel>, ISupportDelayInitialization
	{
		#region ViewModel properties
		public string SearchFilterKeyword { get; set; }
		public string SearchFilterAuthorName { get; set; }
		public string SearchFilterItemName { get; set; }
		public object SearchFilterReviewStatus { get; set; }
		public object SearchFilterReviewType { get; set; }
		#endregion

		#region Commands
		public DelegateCommand ClearFiltersCommand { get; private set; }
		public DelegateCommand ApproveSelectedCommand { get; private set; }
		public DelegateCommand DeclineSelectedCommand { get; private set; }

		public void RaiseCanExecuteChanged()
		{
			ApproveSelectedCommand.RaiseCanExecuteChanged();
			DeclineSelectedCommand.RaiseCanExecuteChanged();
		}
		#endregion

		#region Dependencies
		private readonly IReviewRepository _repository;
		private readonly IViewModelsFactory<IReviewEditViewModel> _itemVmFactory;
		private readonly NavigationManager _navManager;
		private readonly TileManager _tileManager;
		private readonly IAuthenticationContext _authContext;
		private readonly IRepositoryFactory<ICatalogRepository> _catalogRepositoryFactory;
		#endregion

		#region Constructor

		public ReviewsHomeViewModel(IRepositoryFactory<ICatalogRepository> catalogRepositoryFactory, IReviewRepository repository, IViewModelsFactory<IReviewEditViewModel> vmFactory,
			IAuthenticationContext authContext, NavigationManager navManager, TileManager manager)
		{
			_catalogRepositoryFactory = catalogRepositoryFactory;
			_repository = repository;
			_itemVmFactory = vmFactory;
			_authContext = authContext;
			_navManager = navManager;
			_tileManager = manager;

			ItemsListRefreshCommand = new DelegateCommand(RaiseItemsListRefreshInteractionRequest);
			ApproveSelectedCommand = new DelegateCommand(() => RaiseApproveReviewsInteractionRequest(true), IsAnySelected);
			DeclineSelectedCommand = new DelegateCommand(() => RaiseApproveReviewsInteractionRequest(false), IsAnySelected);
			SearchItemsCommand = new DelegateCommand<string>(SearchItems);
			ClearFiltersCommand = new DelegateCommand(DoClearFilters);
			SearchFilterReviewStatus = ReviewStatus.Pending;
            ViewTitle = new ViewTitleBase() { Title = "Reviews", SubTitle = "Catalog".Localize() };

			PopulateTiles();
		}

		#endregion

		#region IReviewsHomeViewModel Members

		private bool _setAll;
		public bool SetAll
		{
			get
			{
				return _setAll;
			}
			set
			{
				_setAll = value;
				SetItemsStatus();
				OnPropertyChanged();
			}
		}

		private void SetItemsStatus()
		{
			ItemsSource.SourceCollection.Cast<VirtualListItem<IReviewEditViewModel>>().
					Where(rev => rev != null && rev.Data != null && rev.Data.InnerItem.Status == (int)ReviewStatus.Pending).
					ToList().ForEach(item => item.Data.InnerItem.SetStatus = _setAll);
			RaiseCanExecuteChanged();
		}

		private bool IsAnySelected()
		{
			return ItemsSource != null && ItemsSource.SourceCollection.Cast<VirtualListItem<IReviewEditViewModel>>().
					Any(rev => rev != null && rev.Data != null && rev.Data.InnerItem.SetStatus);
		}

		public DelegateCommand ItemsListRefreshCommand { get; private set; }
		public DelegateCommand<string> SearchItemsCommand
		{
			get;
			private set;
		}


		private ICollectionView _itemsSource;
		public ICollectionView ItemsSource
		{
			get
			{
				return _itemsSource;
			}
			private set
			{
				_itemsSource = value;
				OnPropertyChanged();
			}

		}
		#endregion

		#region private methods
		private void RaiseItemsListRefreshInteractionRequest()
		{
			ItemsSource.Refresh();
		}

		private void RaiseApproveReviewsInteractionRequest(bool setTo)
		{
			var selectedItems = ItemsSource.SourceCollection.Cast<VirtualListItem<IReviewEditViewModel>>().Where(item => item.Data.InnerItem.SetStatus);
			selectedItems.ToList().ForEach(y =>
			{
				var item = y.Data.InnerItem;
				if (item.ReviewType == vm.ReviewType.Review)
				{
					var originalItem = _repository.Reviews.Where(rev => rev.ReviewId == item.ReviewBaseId).First();
					originalItem.Status = setTo ? (int)ReviewStatus.Approved : (int)ReviewStatus.Declined;
					_repository.Update(originalItem);
					_repository.UnitOfWork.Commit();
				}
				else
				{
					var originalItem = _repository.ReviewComments.Where(rev => rev.ReviewCommentId == item.ReviewBaseId).First();
					originalItem.Status = setTo ? (int)ReviewStatus.Approved : (int)ReviewStatus.Declined;
					_repository.Update(originalItem);
					_repository.UnitOfWork.Commit();
				}
			});
			ItemsSource.Refresh();
			SetAll = false;
		}

		private void SearchItems(string parameter)
		{
			ItemsSource.Refresh();
		}

		private void DoClearFilters()
		{
			SearchFilterKeyword = null;
			SearchFilterAuthorName = null;
			SearchFilterItemName = null;
			SearchFilterReviewStatus = ReviewStatus.Pending;
			SearchFilterReviewType = null;

			OnPropertyChanged("SearchFilterKeyword");
			OnPropertyChanged("SearchFilterAuthorName");
			OnPropertyChanged("SearchFilterItemName");
			OnPropertyChanged("SearchFilterReviewStatus");
			OnPropertyChanged("SearchFilterReviewType");
		}
		#endregion

		#region IVirtualListLoader<IReviewEditViewModel> Members

		public bool CanSort
		{
			get
			{
				return true;
			}
		}

		public IList<IReviewEditViewModel> LoadRange(int startIndex, int count, SortDescriptionCollection sortDescriptions, out int overallCount)
		{
			var retVal = new List<IReviewEditViewModel>();
			var sortBy = sortDescriptions.ToDictionary(sortDesc => sortDesc.PropertyName, sortDesc => (sortDesc.Direction == ListSortDirection.Ascending ? "ASC" : "DESC"));

			if (!(SearchFilterReviewType is vm.ReviewType))
			{
				overallCount = AddReviews(retVal, startIndex, count);
				overallCount += AddComments(retVal, startIndex, count);
			}
			else if (SearchFilterReviewType is vm.ReviewType && (vm.ReviewType)SearchFilterReviewType == vm.ReviewType.Review)
			{
				overallCount = AddReviews(retVal, startIndex, count);
			}
			else if (SearchFilterReviewType is vm.ReviewType && (vm.ReviewType)SearchFilterReviewType == vm.ReviewType.Comment)
			{
				overallCount = AddComments(retVal, startIndex, count);
			}
			else
				overallCount = 0;

			//set SetAll status to false
			SetAll = false;
			_repository.UnitOfWork.CommitAndRefreshChanges();
			return retVal;
		}

		private int AddComments(List<IReviewEditViewModel> retVal, int startIndex, int count)
		{
			var query = _repository.ReviewComments.Expand(rc => rc.Review);

			if (SearchFilterReviewStatus is ReviewStatus)
			{
				query = query.Where(x => x.Status == (int)SearchFilterReviewStatus);
			}

			if (!string.IsNullOrEmpty(SearchFilterItemName))
			{
				query = query.Where(x => x.Review.ItemId.Contains(SearchFilterItemName));
			}

			if (!string.IsNullOrEmpty(SearchFilterAuthorName))
			{
				query = query.Where(x => (x.AuthorName.Contains(SearchFilterAuthorName)));
			}

			if (!string.IsNullOrEmpty(SearchFilterKeyword))
			{
				query = query.Where(x => x.Review.Title.Contains(SearchFilterKeyword));
			}

			var overallCount = query.Count();
			var result = query.OrderByDescending(x => x.Created).Skip(startIndex).Take(count).ToList();
			retVal.AddRange(result.Select(reviewComment =>
			{
				var ret = new viewModel.ReviewComment(reviewComment);
				return ret;
			}).Select(rev => _itemVmFactory.GetViewModelInstance(
					new KeyValuePair<string, object>("item", rev))));
			return overallCount;
		}

		private int AddReviews(List<IReviewEditViewModel> retVal, int startIndex, int count)
		{
			var query = _repository.Reviews.
					Expand(r => r.ReviewFieldValues);
			if (SearchFilterReviewStatus is ReviewStatus)
			{
				query = query.Where(x => x.Status == (int)SearchFilterReviewStatus);
			}

			if (!string.IsNullOrEmpty(SearchFilterItemName))
			{
				query = query.Where(x => x.ItemId.Contains(SearchFilterItemName));
			}

			if (!string.IsNullOrEmpty(SearchFilterAuthorName))
			{
				query = query.Where(x => (x.AuthorName.Contains(SearchFilterAuthorName)));
			}

			if (!string.IsNullOrEmpty(SearchFilterKeyword))
			{
				query = query.Where(x => x.Title.Contains(SearchFilterKeyword));
			}

			var overallCount = query.Count();
			var result = query.OrderByDescending(x => x.Created).Skip(startIndex).Take(count).ToList();
			retVal.AddRange(result.Select(review =>
			{
				var ret = new viewModel.Review(review);
				return ret;
			}).Select(rev => _itemVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", rev))));
			return overallCount;
		}

		#endregion

		#region ISupportDelayInitialization Members
		public void InitializeForOpen()
		{
			if (ItemsSource == null)
			{
				OnUIThread(() => ItemsSource = new VirtualList<IReviewEditViewModel>(this, 25, SynchronizationContext.Current));
			}
		}
		#endregion

		#region Tiles

		private bool NavigateToTabPage(string id)
		{
			var navigationData = _navManager.GetNavigationItemByName(NavigationNames.CatalogHome);
			if (navigationData != null)
			{
				_navManager.Navigate(navigationData);
				var mainViewModel = _navManager.GetViewFromRegion(navigationData) as SubTabsDefaultViewModel;

				return (mainViewModel != null && mainViewModel.SetCurrentTabById(id));
			}
			return false;
		}

		private void PopulateTiles()
		{
			if (_authContext.CheckPermission(PredefinedPermissions.CatalogCustomerReviewsManage)
				|| _authContext.CheckPermission(PredefinedPermissions.CatalogEditorialReviewsCreateEdit)
				|| _authContext.CheckPermission(PredefinedPermissions.CatalogEditorialReviewsPublish)
				|| _authContext.CheckPermission(PredefinedPermissions.CatalogEditorialReviewsRemove)
				)
			{
				_tileManager.AddTile(new NumberTileItem()
				{
					IdModule = NavigationNames.CatalogMenu,
					IdTile = "Reviews",
                    TileTitle = "Reviews",
                    TileCategory = NavigationNames.ModuleName,
					Order = 3,
					IdColorSchema = TileColorSchemas.Schema2,
					NavigateCommand = new DelegateCommand(() => NavigateToTabPage(NavigationNames.HomeName)),
					Refresh = async (tileItem) =>
					{
						try
						{
							using (var repository = _repository)
							{
								if (tileItem is NumberTileItem)
								{
									var query = await Task.Factory.StartNew(() =>
									{
										var retVal = 0;
										//TODO pending amount should be taken from Statistics
										retVal = repository.Reviews.Where(review => review.Status == (int)ReviewStatus.Pending).Count();
										retVal += repository.ReviewComments.Where(comment => comment.Status == (int)ReviewStatus.Pending).Count();
										return retVal;
									});
									(tileItem as NumberTileItem).TileNumber = query.ToString();
								}
							}
						}
						catch
						{
						}
					}
				});
			}
		}

		#endregion
	}
}
