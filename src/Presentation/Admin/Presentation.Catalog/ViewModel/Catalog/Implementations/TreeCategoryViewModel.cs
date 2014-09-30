using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.AppConfig.Repositories;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Controls.StatusIndicator.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.EventAggregation;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;
using catalogModel = VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
    public class TreeCategoryViewModel : CatalogEntityViewModelBase, ITreeCategoryViewModel, IHierarchy
    {
        #region Dependencies

        private readonly IViewModelsFactory<ITreeCategoryViewModel> _treeCategoryVmFactory;
        private readonly IRepositoryFactory<IAppConfigRepository> _seoRepositoryFactory;

        #endregion

        public TreeCategoryViewModel(
            CategoryBase item,
            IRepositoryFactory<IAppConfigRepository> seoRepositoryFactory,
            IRepositoryFactory<ICatalogRepository> repositoryFactory,
            IViewModelsFactory<ICategoryViewModel> categoryVmFactory,
            IViewModelsFactory<ILinkedCategoryViewModel> linkedCategoryVmFactory,
            IViewModelsFactory<ITreeCategoryViewModel> treeCategoryVmFactory,
            IAuthenticationContext authContext,
            INavigationManager navigationManager)
            : base(repositoryFactory, authContext)
        {
            _treeCategoryVmFactory = treeCategoryVmFactory;
            _seoRepositoryFactory = seoRepositoryFactory;

            InnerItem = item;
            EmbeddedHierarchyEntry = this;
            ViewTitle = new ViewTitleBase
            {
                Title = "Category",
                SubTitle = GetDisplayName(item).ToUpper(CultureInfo.InvariantCulture)
            };

            PriorityChangeCommand = new DelegateCommand<string>(RaisePriorityChangeInteractionRequest);

            OpenItemCommand = new DelegateCommand(() =>
            {
                if (NavigationData == null)
                {
                    var param = new KeyValuePair<string, object>("item", InnerItem);
                    IViewModel editVM;
                    if (InnerItem is Category)
                        editVM = categoryVmFactory.GetViewModelInstance(param, new KeyValuePair<string, object>("parentTreeVM", this));
                    else
                        editVM = linkedCategoryVmFactory.GetViewModelInstance(param);

                    NavigationData = ((IClosable)editVM).NavigationData;
                }
                navigationManager.Navigate(NavigationData);
            });
        }

        public string ParentCategory
        {
            get
            {
                var parentCategory = InnerItem.ParentCategory as Category;
                return parentCategory == null ? null : parentCategory.Name;
            }
        }

        #region ITreeCategoryViewModel Members

        private CategoryBase _innerItem;
        public CategoryBase InnerItem
        {
            get { return _innerItem; }
            set { _innerItem = value; OnPropertyChanged(); }
        }

        public new IViewModel Parent
        {
            get
            {
                return base.Parent;
            }
            set
            {
                base.Parent = value as HierarchyViewModelBase;
            }
        }

        public DelegateCommand<string> PriorityChangeCommand { get; private set; }

        public void RefreshUI()
        {
            OnPropertyChanged("DisplayName");
        }

        #endregion

        #region CatalogEntityViewModelBase members
        public override string IconSource
        {
            get
            {
                string result = string.Empty;
                if (InnerItem is Category)
                    result = "Icon_Category";
                else if (InnerItem is LinkedCategory)
                    result = "Icon_LinkedCategory";

                return result;
            }
        }

        public override string DisplayName
        {
            get
            {
                return GetDisplayName(InnerItem);
            }
        }

        public override Brush ShellDetailItemMenuBrush
        {
            get
            {
                var result = (SolidColorBrush)Application.Current.TryFindResource("CatalogDetailItemMenuBrush");

                return result ?? base.ShellDetailItemMenuBrush;
            }
        }

        protected override IViewModel CreateChildrenModel(object children)
        {
            var category = children as CategoryBase;
            if (category == null)
            {
                throw new NullReferenceException("category");
            }
            var retVal = _treeCategoryVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", category));
            retVal.Parent = this;
            return retVal;
        }

        public override void Delete(ICatalogRepository repository, InteractionRequest<Confirmation> commonConfirmRequest, InteractionRequest<Notification> errorNotifyRequest, Action onSuccess)
        {
            var countBuffer = new List<string>();
            int itemCount;

            // count: items in category. Don't try counting items for VirtualCatalog (nor linked category in it). 
            var isThisCategoryInRealCatalog = CatalogHomeViewModel.GetCatalog(this) is catalogModel.Catalog;
            if (isThisCategoryInRealCatalog)
            {
                // checking current category and level 1 (direct) subcategories.
                itemCount = repository.Items
                                      .Where(x =>
                                          x.CategoryItemRelations.Any(y =>
                                              y.CategoryId == InnerItem.CategoryId || y.Category.ParentCategoryId == InnerItem.CategoryId))
                                      .Count();

                if (itemCount > 0)
                {
                    countBuffer.Add(string.Format("has {0} item(s), won't be deleted".Localize(), itemCount));
                }
            }

            // count: direct sub-categories
            itemCount = repository.Categories
                .Where(x => x.ParentCategoryId == InnerItem.CategoryId)
                .Count();

            if (itemCount > 0)
            {
                countBuffer.Add(string.Format("has {0} sub-category(ies), will be deleted".Localize(), itemCount));
            }

            if (isThisCategoryInRealCatalog)
            {
                // count: linked categories			
                itemCount = repository.Categories
                    .OfType<LinkedCategory>()
                    .Where(x => x.LinkedCategoryId == InnerItem.CategoryId)
                    .Count();

                if (itemCount > 0)
                {
                    countBuffer.Add(string.Format("is coupled with {0} Linked Category(ies), will be deleted".Localize(), itemCount));
                }
            }

            // construct nice message
            var typeName = ((InnerItem is Category) ? "category" : "linked category").Localize();
            var content = string.Empty;
            var warnings = countBuffer.Select(x => "\n\t- " + x).ToArray();
            if (warnings.Length > 0)
            {
                content = string.Format("ATTENTION: This {0} {1}.\n\n".Localize(), typeName, string.Join("", warnings));
            }

            content += string.Format("Are you sure you want to delete {0} '{1}'?".Localize(), typeName, DisplayName);

            var confirmation = new ConditionalConfirmation
            {
                Content = content,
                Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
            };

            commonConfirmRequest.Raise(confirmation, async (x) =>
            {
                if (x.Confirmed)
                {
                    await Task.Run(() =>
                    {
                        // Removing item by attaching makes DataServiceRequest exception.
                        var categoryItem = repository.Categories.Where(c => c.CategoryId == InnerItem.CategoryId).FirstOrDefault();
                        //repository.Attach(InnerItem);
                        repository.Remove(categoryItem);

                        // report status
                        var id = Guid.NewGuid().ToString();
                        var item = new StatusMessage { ShortText = string.Format("A {0} '{1}' deletion in progress".Localize(), typeName, DisplayName), StatusMessageId = id };
                        EventSystem.Publish(item);

                        try
                        {
                            if (DeleteSeoKeywords())
                            {
                                repository.UnitOfWork.Commit();
                            }
                            item = new StatusMessage { ShortText = string.Format("A {0} '{1}' deleted successfully".Localize(), typeName, DisplayName), StatusMessageId = id, State = StatusMessageState.Success };
                            EventSystem.Publish(item);
                        }
                        catch (Exception e)
                        {
                            item = new StatusMessage
                            {
                                ShortText = string.Format("Failed to delete {0} '{1}'".Localize(), typeName, DisplayName),
                                Details = e.ToString(),
                                StatusMessageId = id,
                                State = StatusMessageState.Error
                            };
                            EventSystem.Publish(item);
                        }
                    });

                    var parentHierarchyVM = (HierarchyViewModelBase)Parent;
                    parentHierarchyVM.Refresh();
                }
            });
        }

        private bool DeleteSeoKeywords()
        {
            var retVal = false;

            using (var seoRepository = _seoRepositoryFactory.GetRepositoryInstance())
            {
                _repositoryFactory.GetRepositoryInstance().Categories
                    .Where(x => x.ParentCategoryId == InnerItem.CategoryId).ToList().ForEach(y => seoRepository.SeoUrlKeywords.Where(z => z.KeywordValue.Equals(y.CategoryId, StringComparison.InvariantCultureIgnoreCase)).ToList().ForEach(seoRepository.Remove));
                seoRepository.SeoUrlKeywords.Where(x => x.KeywordValue.Equals(InnerItem.CategoryId, StringComparison.InvariantCultureIgnoreCase)).ToList().ForEach(seoRepository.Remove);
                seoRepository.UnitOfWork.Commit();
                retVal = true;
            }

            return retVal;
        }

        #endregion

        #region IHierarchy Members

        public IEnumerable<object> GetChildren(object item)
        {
            return GetChildren(item, 0, -1);
        }

        public IEnumerable<object> GetChildren(object item, int startIndex, int endIndex)
        {
            IEnumerable<CategoryBase> retVal;
            using (var repository = _repositoryFactory.GetRepositoryInstance())
            {
                retVal = repository.Categories;
                if (CatalogHomeViewModel.GetCatalog(this) is VirtualCatalog)
                {
                    if (InnerItem is Category) // category in virtual catalog
                    {
                        // get real categories
                        retVal = retVal
                            .OfType<Category>()
                            .Where(x => x.CatalogId == InnerItem.CatalogId && x.ParentCategoryId == InnerItem.CategoryId);

                        // get linked categories.
                        // check if this category is not inside another linked category. No linked categories in that case.
                        bool noLinkedCategoryInHierarchy = true;
                        var categoryVM = this.Parent as ITreeCategoryViewModel;
                        while (noLinkedCategoryInHierarchy && categoryVM != null)
                        {
                            noLinkedCategoryInHierarchy = !(categoryVM.InnerItem is LinkedCategory);
                            categoryVM = categoryVM.Parent as ITreeCategoryViewModel;
                        }

                        if (noLinkedCategoryInHierarchy)
                        {
                            var linkedCategoryList = repository.Categories
                              .OfType<LinkedCategory>()
                              .Where(x => x.CatalogId == InnerItem.CatalogId && x.ParentCategoryId == InnerItem.CategoryId)
                              .Expand("CategoryLink")
                              .OrderByDescending(x => x.Priority);

                            var localResults = retVal.ToList();
                            localResults.AddRange(linkedCategoryList);
                            retVal = localResults.AsQueryable();
                        }
                    }
                    else
                    {
                        var typedItem = (LinkedCategory)InnerItem;
                        retVal = retVal.Where(x => x.CatalogId == typedItem.LinkedCatalogId && x.ParentCategoryId == typedItem.LinkedCategoryId);
                    }
                }
                else // category in real catalog
                {
                    retVal = retVal
                        .Where(x => x.CatalogId == InnerItem.CatalogId && x.ParentCategoryId == InnerItem.CategoryId);
                }
                retVal = retVal.OrderByDescending(x => x.Priority);
            }

            return retVal;
        }

        public object Item
        {
            get
            {
                return this;
            }
        }


        #region Not Implemented

        public void AddChild(object parent, object child)
        {
            throw new NotImplementedException();
        }

        public bool Contains(object item)
        {
            throw new NotImplementedException();
        }

        public object GetParent(object child)
        {
            throw new NotImplementedException();
        }

        public bool IsLeaf(object item)
        {
            throw new NotImplementedException();
        }

        public void Remove(object child)
        {
            throw new NotImplementedException();
        }

        public void SetLeaf(object item, bool leaf)
        {
            throw new NotImplementedException();
        }

        public void SetParent(object child, object parent)
        {
            using (var repository = _repositoryFactory.GetRepositoryInstance())
            {
                _innerItem = repository.Categories.Where(x => x.CategoryId == _innerItem.CategoryId).First();

                if (parent is ITreeCategoryViewModel)
                {
                    var targetCategoryVM = parent as ITreeCategoryViewModel;

                    InnerItem.ParentCategoryId = targetCategoryVM.InnerItem.CategoryId;
                    // InnerItem.ParentCategory = targetCategoryVM.InnerItem;

                    Parent = targetCategoryVM;
                }
                else if (parent is ITreeCatalogViewModel || parent is ITreeVirtualCatalogViewModel)
                {
                    // var targetCatalogVM = parent as ICatalogViewModel;
                    InnerItem.ParentCategoryId = null;
                    InnerItem.ParentCategory = null;
                    //InnerItem.Catalog = targetCatalogVM.InnerCatalog;
                    //InnerItem.CatalogId = targetCatalogVM.InnerCatalog.CatalogId;

                    Parent = (IViewModel)parent;
                }

                repository.UnitOfWork.Commit();
            }
        }

        public object Root
        {
            get { throw new NotImplementedException(); }
        }
        #endregion

        #endregion

        #region private members

        private string GetDisplayName(CategoryBase item)
        {
            var result = String.Empty;

            var category = item as Category;
            if (category != null)
            {
                result = category.Name;
            }
            else
            {
                var linkedCategory = (LinkedCategory)item;
                if (linkedCategory != null)
                {
                    var link = linkedCategory.CategoryLink;
                    if (link != null)
                        result = (link is Category)
                            ? String.Format("{0} ({1})", ((Category)link).Name, linkedCategory.LinkedCatalogId)
                            : String.Format("{0} ({1})", link.CategoryId, linkedCategory.LinkedCatalogId);
                }
            }
            return result;
        }

        private void RaisePriorityChangeInteractionRequest(string parameter)
        {
            var moveUp = parameter == "up";

            var parentHierarchyVM = (HierarchyViewModelBase)Parent;
            var indexThis = parentHierarchyVM.ChildrenModels.IndexOf(this);
            if ((moveUp && indexThis > 0) ||
                (!moveUp && indexThis < parentHierarchyVM.ChildrenModels.Count - 1))
            {
                var indexNext = indexThis + (moveUp ? -1 : 1);
                var siblingVM = (ITreeCategoryViewModel)parentHierarchyVM.ChildrenModels[indexNext];

                using (var repository = _repositoryFactory.GetRepositoryInstance())
                {
                    _innerItem = repository.Categories.Where(x => x.CategoryId == _innerItem.CategoryId).First();
                    var siblingItem = repository.Categories.Where(x => x.CategoryId == siblingVM.InnerItem.CategoryId).First();

                    var tmpPriority = _innerItem.Priority;
                    if (_innerItem.Priority == siblingItem.Priority)
                        _innerItem.Priority = siblingItem.Priority + (moveUp ? 1 : -1);
                    else
                        _innerItem.Priority = siblingItem.Priority;
                    siblingItem.Priority = tmpPriority;

                    repository.UnitOfWork.Commit();
                }

                parentHierarchyVM.Refresh();
            }
        }
        #endregion
    }
}
