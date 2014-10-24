using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using VirtoCommerce.Client.Globalization;
using VirtoCommerce.Foundation.Catalogs.Model;
using VirtoCommerce.Foundation.Catalogs.Repositories;
using VirtoCommerce.Foundation.DataManagement.Model;
using VirtoCommerce.Foundation.DataManagement.Services;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Interfaces;
using VirtoCommerce.ManagementClient.Core.Controls.StatusIndicator.Model;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.EventAggregation;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using VirtoCommerce.ManagementClient.Localization;
using catalogModel = VirtoCommerce.Foundation.Catalogs.Model;

namespace VirtoCommerce.ManagementClient.Catalog.ViewModel.Catalog.Implementations
{
    public class TreeCatalogViewModel : CatalogEntityViewModelBase, ITreeCatalogViewModel, IHierarchy
    {
        #region Dependencies

        private readonly IViewModelsFactory<ITreeCategoryViewModel> _treeCategoryVmFactory;
        private readonly IViewModelsFactory<ICatalogDeleteViewModel> _catalogDeleteVmFactory;
        private readonly IDataManagementService _exportService;

        #endregion

        public TreeCatalogViewModel(
            IViewModelsFactory<ICatalogViewModel> vmFactory,
            catalogModel.Catalog item,
            IRepositoryFactory<ICatalogRepository> repositoryFactory,
            IAuthenticationContext authContext,
            INavigationManager navigationManager,
            IViewModelsFactory<ICatalogDeleteViewModel> catalogDeleteVmFactory,
            IViewModelsFactory<ITreeCategoryViewModel> treeCategoryVmFactory,
            IDataManagementService exportService)
            : base(repositoryFactory, authContext)
        {
            InnerItem = item;
            EmbeddedHierarchyEntry = this;

            _catalogDeleteVmFactory = catalogDeleteVmFactory;
            _treeCategoryVmFactory = treeCategoryVmFactory;
            _exportService = exportService;

            OpenItemCommand = new DelegateCommand(() =>
            {
                if (NavigationData == null)
                {
                    var editVM = vmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", InnerItem)
                        , new KeyValuePair<string, object>("parentTreeVM", this));
                    NavigationData = ((IClosable)editVM).NavigationData;
                }
                navigationManager.Navigate(NavigationData);
            });

            ExportItemCommand = new DelegateCommand(() => RaiseExportItemCommand());
        }

        #region ICatalogTreeViewModel Members

        private catalogModel.Catalog _innerItem;
        public catalogModel.Catalog InnerItem
        {
            get { return _innerItem; }
            private set { _innerItem = value; OnPropertyChanged(); }
        }

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
                return "Icon_Catalog";
            }
        }

        public override string DisplayName
        {
            get
            {
                return InnerItem.Name;
            }
        }

        public override Brush ShellDetailItemMenuBrush
        {
            get
            {
                SolidColorBrush result =
                        (SolidColorBrush)Application.Current.TryFindResource("CatalogDetailItemMenuBrush");

                return result ?? base.ShellDetailItemMenuBrush;
            }
        }

        protected override IViewModel CreateChildrenModel(object children)
        {
            var category = children as Category;
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

            // count: categories in Catalog
            int itemCount = repository.Categories.Where(x => x.CatalogId == InnerItem.CatalogId).Count();
            if (itemCount > 0)
            {
                countBuffer.Add(string.Format("contains {0} category(ies)".Localize(), itemCount));
            }

            // count: items in Catalog
            itemCount = repository.Items.Where(x => x.CatalogId == InnerItem.CatalogId).Count();
            if (itemCount > 0)
            {
                countBuffer.Add(string.Format("has {0} item(s)".Localize(), itemCount));
            }

            var content = string.Empty;
            var warnings = countBuffer.Select(x => "\n\t- " + x).ToArray();
            if (warnings.Length > 0)
            {
                content = string.Format("ATTENTION: This Catalog {0}.\n\n".Localize(), string.Join("", warnings));
            }
            content += string.Format("Are you sure you want to delete Catalog '{0}'?".Localize(), DisplayName);

            var item = LoadItem(InnerItem.CatalogId, repository);
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
                        var statusUpdate = new StatusMessage { ShortText = string.Format("A Catalog '{0}' deletion in progress".Localize(), DisplayName), StatusMessageId = id };
                        EventSystem.Publish(statusUpdate);

                        try
                        {
                            repository.UnitOfWork.Commit();
                            statusUpdate = new StatusMessage { ShortText = string.Format("A Catalog '{0}' deleted successfully".Localize(), DisplayName), StatusMessageId = id, State = StatusMessageState.Success };
                            EventSystem.Publish(statusUpdate);
                        }
                        catch (Exception e)
                        {
                            statusUpdate = new StatusMessage
                            {
                                ShortText = string.Format("Failed to delete Catalog '{0}'".Localize(), DisplayName),
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
            IEnumerable<Category> retVal;
            using (var repository = _repositoryFactory.GetRepositoryInstance())
            {
                retVal = repository.Categories.Where(x => x.CatalogId == InnerItem.CatalogId && x.ParentCategoryId == null)
                    .OfType<Category>()
                    .OrderByDescending(x => x.Priority);
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

        #region private members

        private catalogModel.Catalog LoadItem(string id, ICatalogRepository repository)
        {
            return repository.Catalogs.OfType<catalogModel.Catalog>()
                .Where(x => x.CatalogId == id)
                //.Expand(x => x.CatalogLanguages)
                //.Expand("PropertySets/PropertySetProperties/Property/PropertyValues")
                .SingleOrDefault();
        }

        private void RaiseExportItemCommand()
        {
            var catalogType = EntityType.Catalog;
            var assetType = EntityType.ItemAsset;
            //var testLocalization = EntityType.Localization;
            var operationId = _exportService.ExportData(new List<EntityType>() { catalogType, assetType }, "tratata", new Dictionary<string, object>() { { "CatalogId", InnerItem.CatalogId }, { "SourceLanguage", "en-US" }, { "TargetLanguage", "ru-RU" } });

            var statusUpdate = new StatusMessage
            {
                ShortText = string.Format("Catalog '{0}' export.".Localize(), InnerItem.CatalogId),
                StatusMessageId = operationId
            };
            EventSystem.Publish(statusUpdate);

            var progress = new Progress<OperationStatus>();
            progress.ProgressChanged += ExportProgressChanged;
            PerformExportAsync(operationId, progress);
        }

        private void ExportProgressChanged(object sender, OperationStatus e)
        {
            if (e != null)
            {
                if (e.OperationState != OperationState.Finished)
                {
                    var statusUpdate = new StatusMessage
                    {
                        ShortText =
                            string.Format("Catalog '{0}' export. Processed {1} items.".Localize(), InnerItem.CatalogId, e.Processed),
                        StatusMessageId = e.OperationId
                    };
                    EventSystem.Publish(statusUpdate);
                }
                else
                {
                    if (e.Errors != null && e.Errors.Count > 0)
                    {
                        var statusUpdate = new StatusMessage
                        {
                            ShortText = string.Format("Catalog '{0}' exported with errors".Localize(), InnerItem.CatalogId),
                            StatusMessageId = e.OperationId,
                            Details = e.Errors.Cast<object>()
                                       .Where(val => val != null)
                                       .Aggregate(string.Empty, (current, val) => current + (val.ToString() + Environment.NewLine)),
                            State = StatusMessageState.Error
                        };
                        EventSystem.Publish(statusUpdate);
                    }
                    else
                    {
                        var statusUpdate = new StatusMessage
                        {
                            ShortText = string.Format("Catalog '{0}' exported successfully".Localize(), InnerItem.CatalogId),
                            StatusMessageId = e.OperationId,
                            State = StatusMessageState.Success
                        };
                        EventSystem.Publish(statusUpdate);
                    }
                }
            }
        }

        private async void PerformExportAsync(string operationId, IProgress<OperationStatus> progress)
        {
            if (progress != null)
            {
                var finished = false;
                while (!finished)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(100));

                    var res = _exportService.GetOperationStatus(operationId);
                    progress.Report(res);

                    if (res != null && res.OperationState == OperationState.Finished)
                        finished = true;
                }
            }
        }

        #endregion
    }
}
