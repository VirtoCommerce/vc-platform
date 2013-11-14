using System;
using Microsoft.Practices.Prism.Commands;
using VirtoCommerce.ManagementClient.Core.Controls.ViewModel;
using VirtoCommerce.ManagementClient.Core.Infrastructure;
using VirtoCommerce.Foundation.Assets.Services;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using VirtoCommerce.ManagementClient.Core.Infrastructure.Navigation;

namespace VirtoCommerce.ManagementClient.Asset.ViewModel
{
    public abstract class AssetEntityViewModelBase : VirtualFolderTreeItemViewModel, IClosable, ISupportAcceptChanges, IOpenTracking
	{
		#region Dependencies
		
		private readonly IAssetService _repository;

		#endregion

		protected AssetEntityViewModelBase(IAssetService repository)
            : base(null, null)
        {
            _repository = repository;
        }
        
		protected IAssetService Repository
		{
			get
			{
				return _repository;
			}
		}

        public virtual string Size
        {
            get { return ""; }
        }

        public abstract DateTime Created { get; }
        public abstract DateTime? Modified  { get; }
        
        #region VirtualFolderTreeItemViewModel methods
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

        protected virtual void OnSaveChangesCommand()
        {
            CloseViewRequestedEvent(this, EventArgs.Empty);
        }


        #region IClosable Members

        public event EventHandler CloseViewRequestedEvent;
        public NavigationItem NavigationData { get; protected set; }

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

	}

}
