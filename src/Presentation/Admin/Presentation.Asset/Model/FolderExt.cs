using System.Runtime.Serialization;
using VirtoCommerce.ManagementClient.Asset.ViewModel;
using VirtoCommerce.Foundation.Assets.Model;
using VirtoCommerce.ManagementClient.Asset.ViewModel.Interfaces;

namespace VirtoCommerce.ManagementClient.Asset.Model
{
    public class FolderExt : Folder
    {

        private string _FileSystemPath;
        public string FileSystemPath
        {
            get
            {
                return _FileSystemPath;
            }
            set
            {
                SetValue(ref _FileSystemPath, () => this.FileSystemPath, value);
            }
        }

        [IgnoreDataMember]
        public IFolderViewModel ViewModel { get; set; }
    }
}
