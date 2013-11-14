using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Microsoft.Practices.Prism.Commands;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Customers.Factories;
using VirtoCommerce.Foundation.Frameworks;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CasePropertySets.Interfaces;

namespace VirtoCommerce.ManagementClient.Customers.ViewModel.Settings.CasePropertySets.Implementations
{
    public class CasePropertySetsSettingsViewModel : HomeSettingsEditableViewModel<CasePropertySet>,
                                                     ICasePropertySetsSettingsViewModel
    {

        #region Dependencies

        private readonly IRepositoryFactory<ICustomerRepository> _repositoryFactory;

        #endregion

        #region Constructor

		public CasePropertySetsSettingsViewModel(
			IRepositoryFactory<ICustomerRepository> repositoryFactory, 
			ICustomerEntityFactory entityFactory, 
			IViewModelsFactory<ICasePropertySetViewModel> editVmFactory)
            : base(entityFactory, null, editVmFactory)
        {
            _repositoryFactory = repositoryFactory;
            CommandsInit();
        }

        #endregion

        #region ICasePropertySetsSettingsViewModel

        public ICollectionView ItemsView { get; private set; }


        public DelegateCommand<CasePropertySet> ItemMoveUpCommand { get; private set; }
        public DelegateCommand<CasePropertySet> ItemMoveDownCommand { get; private set; }


        public override void RaiseCanExecuteChanged()
        {
            base.RaiseCanExecuteChanged();
            if (ItemMoveUpCommand != null)
            {
                ItemMoveUpCommand.RaiseCanExecuteChanged();
            }
            if (ItemMoveDownCommand != null)
            {
                ItemMoveDownCommand.RaiseCanExecuteChanged();
            }

            if (ItemsView != null)
            {
                ItemsView.Refresh();
            }

        }

        #endregion

        #region HomeSettingsViewModel members

        protected override object LoadData()
        {
            using (var repository = _repositoryFactory.GetRepositoryInstance())
            {
                if (repository != null)
                {
                    var items = repository.CasePropertySets.OrderBy(cr => cr.Name).ToList();
                    ItemsView = CollectionViewSource.GetDefaultView(items);
                    ItemsView.SortDescriptions.Add(new SortDescription("Priority", ListSortDirection.Ascending));

                    return items;
                }
            }

            return null;
        }

        public override void RefreshItem(object item)
        {
            var itemToUpdate = item as CasePropertySet;
            if (itemToUpdate != null)
            {
                CasePropertySet itemFromInnerItem =
                    Items.SingleOrDefault(cps => cps.CasePropertySetId == itemToUpdate.CasePropertySetId);

                if (itemFromInnerItem != null)
                {
                    OnUIThread(() =>
                    {
                        itemFromInnerItem.InjectFrom<CloneInjection>(itemToUpdate);
                        OnPropertyChanged("Items");
                    });
                }
            }
        }

        #endregion

        #region HomeSettingsEditableViewModel members

        protected override void RaiseItemAddInteractionRequest()
        {
            throw new System.NotImplementedException();
        }

        protected override void RaiseItemEditInteractionRequest(CasePropertySet item)
        {
            var itemVM = EditVmFactory.GetViewModelInstance(
                new KeyValuePair<string, object>("item", item),
                new KeyValuePair<string, object>("parent", this));

            var openTracking = (IOpenTracking)itemVM;
            openTracking.OpenItemCommand.Execute();
        }

        protected override void RaiseItemDeleteInteractionRequest(CasePropertySet item)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Private Methods

        private void CommandsInit()
        {
            ItemMoveUpCommand = new DelegateCommand<CasePropertySet>(RaiseItemMoveUpInteractionRequest, x =>
                {
                    var result = false;
                    if (x != null && Items != null)
                    {
                        var priorityMin = Items.Min(cps => cps.Priority);

                        if (x.Priority > priorityMin)
                        {
                            result = true;
                        }
                    }

                    return result;
                });

            ItemMoveDownCommand = new DelegateCommand<CasePropertySet>(RaiseItemMoveDownInteractionRequest, x =>
                {
                    var result = false;
                    if (x != null && Items != null)
                    {
                        var priorityMax = Items.Max(cps => cps.Priority);

                        if (x.Priority < priorityMax)
                        {
                            result = true;
                        }
                    }

                    return result;
                });
        }


        private void RaiseItemMoveUpInteractionRequest(CasePropertySet item)
        {
            CaseInfoChangePriority(item, true);
        }

        private void RaiseItemMoveDownInteractionRequest(CasePropertySet item)
        {
            CaseInfoChangePriority(item, false);
        }

        private void CaseInfoChangePriority(CasePropertySet item, bool moveUp)
        {
	        var siblingItem = moveUp ? Items.FirstOrDefault(
			                                      x => x.Priority <= item.Priority && x.CasePropertySetId != item.CasePropertySetId)
		                                      : Items.FirstOrDefault(x => x.Priority > item.Priority);

	        if (siblingItem != null)
            {
                var tmpPriority = item.Priority;
                if (item.Priority == siblingItem.Priority)
                    item.Priority = siblingItem.Priority + (moveUp ? 1 : -1);
                else
                    item.Priority = siblingItem.Priority;
                siblingItem.Priority = tmpPriority;

                using (var repository = _repositoryFactory.GetRepositoryInstance())
                {
                    var it1 = repository.CasePropertySets.Where(x => x.CasePropertySetId == item.CasePropertySetId).SingleOrDefault();
                    var it2 =
                        repository.CasePropertySets.Where(x => x.CasePropertySetId == siblingItem.CasePropertySetId).SingleOrDefault();
                    it1.Priority = item.Priority;
                    it2.Priority = siblingItem.Priority;
                    repository.UnitOfWork.Commit();
                }
                OnUIThread(() => ItemsView.Refresh());

                ItemMoveUpCommand.RaiseCanExecuteChanged();
                ItemMoveDownCommand.RaiseCanExecuteChanged();
            }
        }

	    #endregion
    }
}
