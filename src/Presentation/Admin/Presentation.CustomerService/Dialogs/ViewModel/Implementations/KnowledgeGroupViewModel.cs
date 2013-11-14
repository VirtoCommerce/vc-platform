using System;
using System.Collections.Generic;
using System.Linq;
using VirtoCommerce.ManagementClient.Core.Controls.ViewModel;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Commands;
using System.ComponentModel;
using VirtoCommerce.Foundation.Frameworks.Extensions;
using VirtoCommerce.Foundation.Frameworks;
using Omu.ValueInjecter;
using VirtoCommerce.Foundation.Frameworks.ConventionInjections;
using VirtoCommerce.Foundation.Customers.Repositories;
using VirtoCommerce.Foundation.Customers.Model;
using VirtoCommerce.Foundation.Customers.Factories;
using VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Customers.Dialogs.ViewModel.Implementations
{
    public class KnowledgeGroupViewModel : VirtualFolderTreeItemViewModel, IKnowledgeGroupViewModel, IHierarchy, IClosable, ISupportAcceptChanges, IOpenTracking, ISupportDelayInitialization
	{
		#region Dependencies

	    private readonly IViewModelsFactory<IKnowledgeGroupViewModel> _groupVmFactory;
		protected ICustomerRepository CustomersRepository { get; private set; }
	    private readonly IRepositoryFactory<ICustomerRepository> _repositoryFactory;

		#endregion
		

		public KnowledgeGroupViewModel(
			IRepositoryFactory<ICustomerRepository> repositoryFactory,
			IViewModelsFactory<IKnowledgeGroupViewModel> groupVmFactory,
			ICustomerRepository customerRepository, 
			ICustomerEntityFactory entityFactory, 
			KnowledgeBaseGroup item)
            : base(null, null)
		{
			_repositoryFactory = repositoryFactory;
			_groupVmFactory = groupVmFactory;
            CustomersRepository = customerRepository;
            _originalItem = item;
			InnerItem = _originalItem.DeepClone(entityFactory as IKnownSerializationTypes);
            _innerItem.PropertyChanged += _innerItem_PropertyChanged;
            EmbeddedHierarchyEntry = this;
        }

        #region Handlers

        void _innerItem_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _innerItem.PropertyChanged -= _innerItem_PropertyChanged;
            OnUIThread(() => { OnPropertyChanged("IsValid"); });
            _innerItem.PropertyChanged += _innerItem_PropertyChanged;
        }

        #endregion

        #region IKnowledgeBaseGroup Members

        private readonly KnowledgeBaseGroup _originalItem;
        public KnowledgeBaseGroup OriginalItem { get { return _originalItem; } }

        private KnowledgeBaseGroup _innerItem;
        public KnowledgeBaseGroup InnerItem
        {
            get { return _innerItem; }
            set { _innerItem = value; OnPropertyChanged(); }
        }

        public bool IsValid
        {
            get
            {
                return !String.IsNullOrEmpty(InnerItem.Name) && !String.IsNullOrEmpty(InnerItem.Title);
            }
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

        #endregion

        #region CatalogEntityViewModelBase members

        public override string DisplayName
        {
            get
            {
                return OriginalItem.Name;
            }
        }

        protected override IViewModel CreateChildrenModel(object children)
        {
            var knowledgeGroup = children as KnowledgeBaseGroup;
            if (knowledgeGroup == null)
            {
                throw new NullReferenceException("KnowledgeBaseGroup");
            }

            var retVal = _groupVmFactory.GetViewModelInstance(new KeyValuePair<string, object>("item", knowledgeGroup));
            retVal.Parent = this;
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
            var retVal = CustomersRepository.KnowledgeBaseGroups.Where(x => x.Parent.KnowledgeBaseGroupId == InnerItem.KnowledgeBaseGroupId)
                                                                .OfType<KnowledgeBaseGroup>();
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

            if (parent is IKnowledgeGroupViewModel)
            {
                var targetCategoryVM = parent as IKnowledgeGroupViewModel;
                InnerItem.ParentId = targetCategoryVM.InnerItem.KnowledgeBaseGroupId;
                ApplyToDB();
                Parent = targetCategoryVM;
            }

        }

        public object Root
        {
            get { throw new NotImplementedException(); }
        }
        #endregion

        #endregion

        #region IClosable Members

        public event EventHandler CloseViewRequestedEvent;
        public NavigationItem NavigationData { get; protected set; }

        protected void OnCloseViewRequestedEvent(EventArgs args)
        {
            var handler = CloseViewRequestedEvent;
            if (handler != null)
                handler(this, args);
        }

        #endregion

        #region ISupportAcceptChanges Members

        public InteractionRequest<Confirmation> CancelConfirmRequest
        {
            get;
            private set;
        }

        public DelegateCommand<object> CancelCommand
        {
            get;
            private set;
        }


        public DelegateCommand<object> SaveChangesCommand
        {
            get;
            protected set;
        }

        private bool _isModified;
        public bool IsModified
        {
            get
            {
                return _isModified;
            }

            protected set
            {
                _isModified = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region IOpenTracking Members

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; OnPropertyChanged(); }
        }

        public DelegateCommand OpenItemCommand
        {
            get;
            protected set;
        }

        #endregion

        #region ISupportDelayInitialization Members

        public virtual void InitializeForOpen()
        {
        }

        #endregion

        #region DB operation

        public void InsertToDB()
        {
	        using (var repository = _repositoryFactory.GetRepositoryInstance())
	        {
		        OnUIThread(() => OriginalItem.InjectFrom<CloneInjection>(InnerItem));
		        repository.Add(OriginalItem);
		        repository.UnitOfWork.Commit();
	        }
        }

        public void ApplyToDB()
        {
	        using (var repository = _repositoryFactory.GetRepositoryInstance())
	        {
		        repository.Attach(OriginalItem);
		        OnUIThread(() => OriginalItem.InjectFrom<CloneInjection>(InnerItem));
		        OnPropertyChanged("DisplayName");
		        repository.UnitOfWork.Commit();
	        }
        }

        public void RemoveFromDB()
        {
	        using (var repository = _repositoryFactory.GetRepositoryInstance())
	        {
		        repository.Attach(OriginalItem);
		        repository.Remove(OriginalItem);
		        repository.UnitOfWork.Commit();
	        }
        }

        #endregion

    }
}


