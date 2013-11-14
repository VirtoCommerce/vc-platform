using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace VirtoCommerce.ManagementClient.Core.Infrastructure
{
	public abstract class HierarchyViewModelBase : ViewModelBase // ViewModelDetailBase<T>
	{
		private bool _workerInitialized = false;

		#region Constructor

		public HierarchyViewModelBase(HierarchyViewModelBase parent)
			: this(parent, parent.EmbeddedHierarchyEntry)
		{
		}

		public HierarchyViewModelBase(IHierarchy embeddedEntry)
			: this(null, embeddedEntry)
		{
		}

		public HierarchyViewModelBase(HierarchyViewModelBase parent, IHierarchy embeddedHierarchyEntry)
		{
			_parent = parent;
			EmbeddedHierarchyEntry = embeddedHierarchyEntry;
			LoadChildrensWorker = new BackgroundWorker();
		}
		#endregion

		#region Methods

		public virtual void LoadChildrens()
		{
			if (!IsLoading)
			{
				if (!_workerInitialized)
				{
					SetupLoadChildrensWorker();
					_workerInitialized = true;
				}

				ChildrenModels.Clear();
				LoadChildrensWorker.RunWorkerAsync();
			}
		}

		public virtual void Refresh()
		{
			IsLoaded = false;
			LoadChildrens();
		}

		protected virtual void SetupLoadChildrensWorker()
		{
			LoadChildrensWorker.DoWork += (sender, args) =>
			{
				IsLoading = true;

				var results = EmbeddedHierarchyEntry.GetChildren(EmbeddedHierarchyEntry.Item);
				args.Result = results;
			};

			LoadChildrensWorker.RunWorkerCompleted += (sender, args) =>
			{
				var results = args.Result as IEnumerable<object>;
				foreach (var child in results)
				{
					var childModel = CreateChildrenModel(child);
					if (childModel != null)
					{
						ChildrenModels.Add(childModel);
					}
				}

				IsLoading = false;
				IsLoaded = true;
			};
		}

		protected abstract void OnExpanded();

		protected abstract void OnCollapsed();

		public abstract void OnSelected();

		public abstract void OnUnselected();

		protected abstract IViewModel CreateChildrenModel(object children);

		#endregion

		#region Properties
		private ObservableCollection<IViewModel> _childrenModels = new ObservableCollection<IViewModel>();
		public ObservableCollection<IViewModel> ChildrenModels
		{
			get
			{
				return _childrenModels;
			}
		}

		private bool _IsLoaded = false;
		public virtual bool IsLoaded
		{
			get
			{
				return _IsLoaded;
			}
			private set
			{
				_IsLoaded = value;
				OnPropertyChanged("IsLoaded");
			}
		}

		private bool _isLoading = false;
		public bool IsLoading
		{
			get
			{
				return _isLoading;
			}
			set
			{
				_isLoading = value;
				OnPropertyChanged("IsLoading");
			}
		}


		public IHierarchy EmbeddedHierarchyEntry
		{
			get;
			set;
		}

		protected BackgroundWorker LoadChildrensWorker
		{
			get;
			private set;
		}

		private bool _isExpanded = false;
		public bool IsExpanded
		{
			get { return _isExpanded; }
			set
			{
				if (value != _isExpanded)
				{
					_isExpanded = value;
					OnPropertyChanged("IsExpanded");
					if (value)
					{
						OnExpanded();
					}
					else
					{
						OnCollapsed();
					}

					// #161 tree expand shouldn't change selection
					// IsSelected = true;
				}

				// Expand all the way up to the root.
				if (_isExpanded && _parent != null)
					_parent.IsExpanded = true;
			}
		}


		private bool _isSelected = false;
		public bool IsSelected
		{
			get { return _isSelected; }
			set
			{
				if (value != _isSelected)
				{
					_isSelected = value;
					if (value && this.Parent != null)
					{
						this.Parent.IsExpanded = true;
					}
					OnPropertyChanged("IsSelected");
					if (value)
					{
						OnSelected();
					}
					else
					{
						OnUnselected();
					}
				}
			}
		}

		private HierarchyViewModelBase _parent;
		public HierarchyViewModelBase Parent
		{
			get
			{
				return _parent;
			}
			protected set
			{
				_parent = value;
				OnPropertyChanged("Parent");
			}
		}

		public bool IsRoot
		{
			get
			{
				return Parent == null;
			}
		}

		#endregion

	}
}
