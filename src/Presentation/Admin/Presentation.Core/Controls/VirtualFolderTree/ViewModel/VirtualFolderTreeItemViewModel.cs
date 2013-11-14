using System.Linq;
using VirtoCommerce.ManagementClient.Core.Infrastructure;

namespace VirtoCommerce.ManagementClient.Core.Controls.ViewModel
{
    public class VirtualFolderTreeItemViewModel : HierarchyViewModelBase
    {
        public static readonly VirtualFolderTreeItemViewModel DummyNode = new VirtualFolderTreeItemViewModel();

        private VirtualFolderTreeItemViewModel()
            : base(null, null)
        {
        }

        public VirtualFolderTreeItemViewModel(HierarchyViewModelBase parent, IHierarchy embeddedFolderEntry)
            : base(parent, embeddedFolderEntry)
        {
            ChildrenModels.Add(DummyNode);
        }

        #region HierarchyViewModelBase members

        public override bool IsLoaded
        {
            get
            {
                return !(this.ChildrenModels.Count() == 1 && this.ChildrenModels.First() == DummyNode);
            }
        }

        protected override IViewModel CreateChildrenModel(object children)
        {
            IViewModel retVal = null;
            var hierarchyChild = children as IHierarchy;
            if (hierarchyChild != null)
            {
                retVal = new VirtualFolderTreeItemViewModel(this, hierarchyChild);
            }

            return retVal;

        }

        protected override void OnExpanded()
        {
            LoadChildrens();
        }

        protected override void OnCollapsed()
        {
        }

        public override void OnSelected()
        {
            IsExpanded = true;
        }

        public override void OnUnselected()
        {
        }

        #endregion


    }
}
