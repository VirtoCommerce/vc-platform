using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirtoCommerce.Client.Globalization;
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

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
    public class TreeVirtualCatalogViewModel : CatalogEntityViewModelBase, ITreeVirtualCatalogViewModel, IHierarchy
    {
        private const string _catalogImageSource = "Icon_VirtualCatalog";

        #region Dependencies

        protected readonly IViewModelsFactory<ITreeCategoryViewModel> _vmFactory;
        private readonly IViewModelsFactory<ICatalogDeleteViewModel> _catalogDeleteVmFactory;

        #endregion

        public TreeVirtualCatalogViewModel(
            IViewModelsFactory<ITreeCategoryViewModel> vmFactory,
            IViewModelsFactory<IVirtualCatalogViewModel> virtualCatalogVmFactory,
            IViewModelsFactory<ICatalogDeleteViewModel> catalogDeleteVmFactory,
            CatalogBase item,
            IRepositoryFactory<ICatalogRepository> repositoryFactory,
            IAuthenticationContext authContext,
            INavigationManager navigationManager)
            : base(repositoryFactory, authContext)
        {
            InnerItem = item;
            _vmFactory = vmFactory;
            _catalogDeleteVmFactory = catalogDeleteVmFactory;
            EmbeddedHierarchyEntry = this;
            ViewTitle = new ViewTitleBase
                {
                    Title = "Virtual Catalog",
                    SubTitle = (item != null && !string.IsNullOrEmpty(item.Name)) ? item.Name : ""
                };

            OpenItemCommand = new DelegateCommand(() =>
            {
                if (NavigationData == null)
                {
                    var editVM = virtualCatalogVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", InnerItem)
                        , new KeyValuePair<string, object>("parentTreeVM", this));
                    NavigationData = ((IClosable)editVM).NavigationData;
                }
                navigationManager.Navigate(NavigationData);
            });
        }

        #region ICatalogViewModel Members

        private CatalogBase _innerItem;
        public CatalogBase InnerItem
        {
            get { return _innerItem; }
            private set { _innerItem = value; OnPropertyChanged(); }
        }

        public void RefreshUI()
        {
            OnPropertyChanged("DisplayName");
        }

        #endregion

        #region ViewModelBase members
        public override string IconSource
        {
            get
            {
                return _catalogImageSource;
            }
        }

        public override string DisplayName
        {
            get
            {
                return InnerItem.Name;
            }
        }

        protected override IViewModel CreateChildrenModel(object children)
        {
            var category = children as CategoryBase;
            if (category == null)
            {
                throw new NullReferenceException("category");
            }

            var retVal = _vmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", category));
            retVal.Parent = this;
            return retVal;
        }

        public override void Delete(ICatalogRepository repository, InteractionRequest<Confirmation> commonConfirmRequest, InteractionRequest<Notification> errorNotifyRequest, Action onSuccess)
        {
            var content = string.Empty;

            // count: categories in VirtualCatalog
            var itemCount = repository.Categories
                .Where(x => x.CatalogId == InnerItem.CatalogId)
                .Count();

            if (itemCount > 0)
            {
                content = string.Format("ATTENTION: This Virtual Catalog contains {0} category(ies).\n\n".Localize(), itemCount);
            }

            content += string.Format("Are you sure you want to delete Virtual Catalog '{0}'?".Localize(), DisplayName);

            var item = repository.Catalogs.Where(x => x.CatalogId == InnerItem.CatalogId).Single();
            var itemVM = _catalogDeleteVmFactory.GetViewModelInstance(
                new KeyValuePair<string, object>("item", item),
                new KeyValuePair<string, object>("contentText", content));

            var confirmation = new ConditionalConfirmation(itemVM.Validate)
            {
                Content = itemVM,
                Title = "Delete confirmation".Localize(null, LocalizationScope.DefaultCategory)
            };
            commonConfirmRequest.Raise(confirmation, async (x) =>
            {
                if (x.Confirmed)
                {
                    await Task.Run(() =>
                        {
                            repository.Remove(item);

                            // report status
                            var id = Guid.NewGuid().ToString();
                            var statusUpdate = new StatusMessage { ShortText = string.Format("A Virtual Catalog '{0}' deletion in progress".Localize(), DisplayName), StatusMessageId = id };
                            EventSystem.Publish(statusUpdate);

                            try
                            {
                                repository.UnitOfWork.Commit();
                                statusUpdate = new StatusMessage { ShortText = string.Format("A Virtual Catalog '{0}' deleted successfully".Localize(), DisplayName), StatusMessageId = id, State = StatusMessageState.Success };
                                EventSystem.Publish(statusUpdate);
                            }
                            catch (Exception e)
                            {
                                statusUpdate = new StatusMessage
                                {
                                    ShortText = string.Format("Failed to delete Virtual Catalog '{0}'".Localize(), DisplayName),
                                    Details = e.ToString(),
                                    StatusMessageId = id,
                                    State = StatusMessageState.Error
                                };
                                EventSystem.Publish(statusUpdate);
                            }
                        });

                    onSuccess();
                }
            });
        }

        #endregion

        #region IHierarchy Members

        public IEnumerable<object> GetChildren(object item)
        {
            return GetChildren(item, 0, -1);
        }

        public IEnumerable<object> GetChildren(object item, int startIndex, int endIndex)
        {
            List<CategoryBase> retVal;
            using (var repository = _repositoryFactory.GetRepositoryInstance())
            {
                retVal = repository.Categories
                .OfType<Category>()
                .Where(x => x.CatalogId == InnerItem.CatalogId && x.ParentCategoryId == null)
                .OrderByDescending(x => x.Priority)
                .Cast<CategoryBase>()
                .ToList();
                var linkedCategoryList = repository.Categories
                    .OfType<LinkedCategory>()
                    .Where(x => x.CatalogId == InnerItem.CatalogId && x.ParentCategoryId == null)
                    .Expand("CategoryLink")
                    .OrderByDescending(x => x.Priority);

                retVal.AddRange(linkedCategoryList);
            }

            return retVal.OrderByDescending(x => x.Priority);
        }

        public object Item
        {
            get
            {
                return this;
            }
        }

        #region Not implemented

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
            throw new NotImplementedException();
        }

        public object Root
        {
            get { throw new NotImplementedException(); }
        }


        #endregion

        #endregion
    }
}
